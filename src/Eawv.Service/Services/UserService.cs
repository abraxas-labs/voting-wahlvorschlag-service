// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Eawv.Service.Authentication;
using Eawv.Service.Configuration;
using Eawv.Service.Exceptions;
using Eawv.Service.Models;
using Eawv.Service.Utils;
using Microsoft.Extensions.Logging;
using Voting.Lib.Iam.Services.ApiClient.Identity;
using Voting.Lib.Iam.Services.ApiClient.Permission;
using IdentityClient = Voting.Lib.Iam.Services.ApiClient.Identity;
using PermissionClient = Voting.Lib.Iam.Services.ApiClient.Permission;

namespace Eawv.Service.Services;

/// <inheritdoc cref="IUserService"/>
public class UserService : IUserService
{
    private const string TenantUsersCacheKey = "TenantUsers";
    private const string UserAuthorizationsCacheKey = "UserAuthorizations";
    private const string ChildTenantUsersCacheKey = "ChildTenantUsers";
    private const string MyAccountShortcut = "MA";
    private readonly AuthService _authService;
    private readonly CacheService _cache;
    private readonly ITenantService _tenantService;
    private readonly SecureConnectConfiguration _config;
    private readonly ApplicationService _applicationService;
    private readonly ISecureConnectIdentityServiceClient _identityClient;
    private readonly ISecureConnectPermissionServiceClient _permissionClient;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(
        AuthService authService,
        CacheService cache,
        ITenantService tenantService,
        SecureConnectConfiguration config,
        ApplicationService applicationService,
        ISecureConnectIdentityServiceClient identityClient,
        ISecureConnectPermissionServiceClient permissionClient,
        IMapper mapper,
        ILogger<UserService> logger)
    {
        _authService = authService;
        _cache = cache;
        _tenantService = tenantService;
        _config = config;
        _applicationService = applicationService;
        _identityClient = identityClient;
        _permissionClient = permissionClient;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> GetCurrentUserName()
    {
        return (await Get(_authService.GetUserId())).Username;
    }

    public async Task<IdentityClient.V1User> Get(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        return await _cache.GetOrCreate(id, async ()
            => await _identityClient.IdentityService_GetUserByLoginIdAsync(id, false));
    }

    public async Task<IEnumerable<PermissionClient.V1User>> GetWahlverwaltersForTenant(string tenantId)
    {
        return await _cache.GetOrCreate("Wahlverwalters", tenantId, async () =>
        {
            var application = await _applicationService.GetEawvApplication();
            var users = await _permissionClient.PermissionService_GetUsersByAppAndTenant2Async(tenantId, application.Id);
            return await users.Users.WhereAsync(async u => await IsWahlverwalter(tenantId, application.Id, u.Loginid));
        });
    }

    public async Task<TenantUser> CreateUser(IdentityClient.V1User user, string email, List<string> tenantIds)
    {
        IdentityClient.V1User existingUser = null;

        try
        {
            existingUser = await _identityClient.IdentityService_GetUserByUsernameAsync(user.Username, false);
        }
        catch (IdentityClient.ApiException ex) when (ex.StatusCode == (int)HttpStatusCode.NotFound)
        {
            // Ignore api exception when user is not found. Continue creating a new user.
        }

        if (existingUser != null)
        {
            var newAuths = await UpdateUserAuthorizations(existingUser.Loginid, [], tenantIds);
            return new TenantUser(user, newAuths);
        }

        // Do this before creating anything on the IAM side
        var ensureAccessTasks = tenantIds.Select(t => _tenantService.AssertChildParent(t, _authService.GetTenantId()));
        await Task.WhenAll(ensureAccessTasks);

        var resourceOwnerTenantId = tenantIds[0];
        user.Type = "user";
        user.Resource_owner_tenant_id = resourceOwnerTenantId;
        user = await _identityClient.IdentityService_InitUserLoginWithEmailAsync(
            new V1UserloginWithMail
            {
                User = user,
                Email = email,
            },
            resourceOwnerTenantId);

        var auths = await UpdateUserAuthorizations(user.Loginid, [], tenantIds, resourceOwnerTenantId);

        return new TenantUser(user, auths);
    }

    public async Task<TenantUser> UpdateUser(string loginId, List<string> tenantIds)
    {
        var existingAuths = await GetParentAuthorizationsForUser(loginId);

        if (existingAuths.Count == 0)
        {
            throw new ForbiddenException("Forbidden to update a user who doesn't belong to a child tenant.");
        }

        var nonModifiedAuths = new List<Abraxaspermissionapiv1Authorization>();
        var authsToRemove = new List<Abraxaspermissionapiv1Authorization>();
        var newTenants = new List<string>(tenantIds);

        foreach (var existingAuth in existingAuths)
        {
            if (tenantIds.Contains(existingAuth.Tenant.Id))
            {
                nonModifiedAuths.Add(existingAuth);
            }
            else
            {
                authsToRemove.Add(existingAuth);
            }

            newTenants.Remove(existingAuth.Tenant.Id);
        }

        var addedAuths = await UpdateUserAuthorizations(loginId, authsToRemove, newTenants);
        var auths = nonModifiedAuths.Concat(addedAuths).ToList();
        var user = await Get(loginId);
        return new TenantUser(user, auths);
    }

    public async Task RemoveRelevantAuthorizations(string loginId)
    {
        var allAuths = await GetParentAuthorizationsForUser(loginId);
        await UpdateUserAuthorizations(loginId, allAuths, []);
    }

    public async Task<List<Abraxaspermissionapiv1Authorization>> GetParentAuthorizationsForUser(string loginId)
    {
        var currentAuths = await _cache.GetOrCreate(UserAuthorizationsCacheKey, loginId, async () =>
        {
            _logger.LogDebug("No cache entry found for key {CacheKey}. Getting user auths from IAM.", $"{UserAuthorizationsCacheKey}.{loginId}");

            var eawvApp = await _applicationService.GetEawvApplication();
            var myAccountApp = await _applicationService.GetApplication(MyAccountShortcut);

            var userAuthorizations = await _permissionClient.PermissionService_GetUserAuthorizationsAsync(loginId);
            return userAuthorizations?.Authorizations?.Where(a => a.Application.Id == eawvApp.Id || a.Application.Id == myAccountApp.Id) ?? [];
        });

        var currentTenantId = _authService.GetTenantId();
        var childAuths = new List<Abraxaspermissionapiv1Authorization>();
        foreach (var auth in currentAuths)
        {
            var parentTenantId = await _tenantService.GetParentTenantId(auth.Tenant.Id);
            if (parentTenantId == currentTenantId)
            {
                childAuths.Add(auth);
            }
        }

        return childAuths;
    }

    public async Task<IEnumerable<TenantUser>> GetUsersForTenant(string tenantId)
    {
        _logger.LogDebug("Getting users for tenant {TenantId}", tenantId);

        await EnsureCanAccessTenant(tenantId);

        return await _cache.GetOrCreate(TenantUsersCacheKey, tenantId, async () =>
        {
            _logger.LogDebug("No cache entry found for key {CacheKey}. Getting users from IAM.", $"{TenantUsersCacheKey}.{tenantId}");

            var application = await _applicationService.GetEawvApplication();
            var tenant = await _tenantService.Get(tenantId);

            var tenantUsers = await _permissionClient.PermissionService_GetUsersByAppAndTenant2Async(tenant.Id, application.Id);

            return tenantUsers.Users?
                .Where(u => u.Lifecycle != PermissionClient.ApiStorageLifecycle.INACTIVE)
                .Select(u => new TenantUser(_mapper.Map<IdentityClient.V1User>(u), tenant))
                ?? new List<TenantUser>();
        });
    }

    public async Task<IEnumerable<TenantUser>> GetUsersFromChildTenants(string parentTenantId)
    {
        _logger.LogDebug("Getting users from child tenants under parent tenant {ParentTenantId}", parentTenantId);

        return await _cache.GetOrCreate(ChildTenantUsersCacheKey, parentTenantId, async () =>
        {
            _logger.LogDebug("No cache entry found for key {CacheKey}. Getting users from IAM.", $"{ChildTenantUsersCacheKey}.{parentTenantId}");

            var application = await _applicationService.GetEawvApplication();
            var childTenants = await _tenantService.GetParties();

            var users = new List<TenantUser>();
            foreach (var tenant in childTenants)
            {
                var tenantUsers = await _permissionClient.PermissionService_GetUsersByAppAndTenant2Async(tenant.Id, application.Id);
                var activeUsers = tenantUsers?.Users?.Where(u => u.Lifecycle != PermissionClient.ApiStorageLifecycle.INACTIVE) ?? [];

                foreach (var user in activeUsers)
                {
                    var existingUser = users.SingleOrDefault(u => u.User.Id == user.Id);

                    if (existingUser != null)
                    {
                        existingUser.Tenants.Add(tenant);
                    }
                    else
                    {
                        users.Add(new TenantUser(_mapper.Map<IdentityClient.V1User>(user), tenant));
                    }
                }
            }

            return users;
        });
    }

    private async Task EnsureCanAccessTenant(string tenantId)
    {
        if (tenantId == _authService.GetTenantId())
        {
            return;
        }

        if (!_authService.IsWahlverwalter)
        {
            throw new ForbiddenException("Users may only fetch users from their own tenant.");
        }

        await _tenantService.AssertChildParent(tenantId, _authService.GetTenantId());
    }

    private async Task<bool> IsWahlverwalter(string tenantId, string applicationId, string loginId)
    {
        var auths = await _permissionClient.PermissionService_GetAuthorizationsByUserAndTenant2Async(loginId, tenantId);
        return auths.Authorizations.Any(a =>
            a.Application.Id == applicationId &&
            a.Labels.Any(l => l.Key == _config.RoleLabelKey && l.Value == Role.Wahlverwalter));
    }

    private async Task<List<Abraxaspermissionapiv1Authorization>> UpdateUserAuthorizations(
        string loginId,
        IEnumerable<Abraxaspermissionapiv1Authorization> authsToRemove,
        ICollection<string> newTenantIds,
        string resoureOwnerTenant = null)
    {
        var ensureAccessTasks = newTenantIds.Select(t => _tenantService.AssertChildParent(t, _authService.GetTenantId()));
        await Task.WhenAll(ensureAccessTasks);

        var deletionTasks = authsToRemove.Select(a => _permissionClient.PermissionService_DeleteAuthorizationAsync(loginId, a.Id, _authService.GetTenantId()));
        await Task.WhenAll(deletionTasks);

        var eawvRoleLabel = new PermissionClient.CommonapiLabel
        {
            Key = _config.RoleLabelKey,
            Value = Role.User,
        };
        var eawvApplication = await _applicationService.GetEawvApplication();
        var myAccountApplication = await _applicationService.GetApplication(MyAccountShortcut);

        var addedAuths = new List<Abraxaspermissionapiv1Authorization>();
        foreach (var tenantId in newTenantIds)
        {
            var eawvAuth = await CreateAuthorization(eawvApplication, loginId, tenantId, eawvRoleLabel);
            addedAuths.Add(eawvAuth);

            // Add MyAccount authorization for all tenants except the resource owner tenant
            // who is automatically granted access to MyAccount at the time of user creation.
            if (tenantId != resoureOwnerTenant)
            {
                var myAccountAuth = await CreateAuthorization(myAccountApplication, loginId, tenantId, eawvRoleLabel);
                addedAuths.Add(myAccountAuth);
            }
        }

        _cache.Invalidate<IdentityClient.V1User>(loginId);
        _cache.Invalidate(UserAuthorizationsCacheKey, loginId);
        _cache.Invalidate(TenantUsersCacheKey, _authService.GetTenantId());
        _cache.Invalidate(ChildTenantUsersCacheKey, _authService.GetTenantId());

        return addedAuths;
    }

    private async Task<Abraxaspermissionapiv1Authorization> CreateAuthorization(
        V1Application application,
        string loginId,
        string tenantId,
        PermissionClient.CommonapiLabel role)
    {
        var auth = new Abraxaspermissionapiv1Authorization
        {
            Application = application,
            LoginId = loginId,
            Tenant = new V1Tenant { Id = tenantId },
            Labels = [role],
        };

        return await _permissionClient.PermissionService_CreateAuthorizationAsync(loginId, auth, _authService.GetTenantId());
    }
}
