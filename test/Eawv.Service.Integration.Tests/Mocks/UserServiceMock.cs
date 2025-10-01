// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Exceptions;
using Eawv.Service.Integration.Tests.MockedData;
using Eawv.Service.Models;
using Eawv.Service.Services;
using IdentityClient = Voting.Lib.Iam.Services.ApiClient.Identity;
using PermissionClient = Voting.Lib.Iam.Services.ApiClient.Permission;

namespace Eawv.Service.Integration.Tests.Mocks;

public class UserServiceMock : IUserService
{
    private readonly AuthService _authService;
    private readonly ITenantService _tenantService;

    public UserServiceMock(AuthService authService, ITenantService tenantService)
    {
        _authService = authService;
        _tenantService = tenantService;
    }

    public Task<TenantUser> CreateUser(IdentityClient.V1User user, string email, List<string> tenantIds)
    {
        return Task.FromResult(new TenantUser
        {
            User = new IdentityClient.V1User
            {
                Id = user.Id,
                Emails =
                [
                    new IdentityClient.Apiv1Email
                    {
                        Primary = true,
                        Email = email,
                    },
                ],
                Lifecycle = IdentityClient.ApiStorageLifecycle.ACTIVE,
            },
            Tenants = tenantIds.ConvertAll(x => new PermissionClient.V1Tenant
            {
                Id = x,
                Name = TenantMockData.All.FirstOrDefault(t => t.Id == x)?.Name ?? x,
            }),
        });
    }

    public Task<IdentityClient.V1User> Get(string id)
    {
        return Task.FromResult(GetUser(id));
    }

    public async Task<string> GetCurrentUserName()
    {
        return (await Get(_authService.GetUserId())).Username;
    }

    public async Task<List<PermissionClient.Abraxaspermissionapiv1Authorization>> GetParentAuthorizationsForUser(string loginId)
    {
        var user = UserMockData.All.FirstOrDefault(u => u.Id == loginId);
        if (user == null)
        {
            throw new InvalidOperationException($"User with id {loginId} not found");
        }

        var currentTenantId = _authService.GetTenantId();
        var childAuths = new List<PermissionClient.Abraxaspermissionapiv1Authorization>();
        foreach (var (tenantId, _) in user.RoleByTenantId)
        {
            var parentTenantId = await _tenantService.GetParentTenantId(tenantId);
            if (parentTenantId == currentTenantId)
            {
                childAuths.Add(new PermissionClient.Abraxaspermissionapiv1Authorization
                {
                    LoginId = loginId,
                    Tenant = await _tenantService.Get(tenantId),
                });
            }
        }

        return childAuths;
    }

    public async Task<IEnumerable<TenantUser>> GetUsersForTenant(string tenantId)
    {
        await EnsureCanAccessTenant(tenantId);

        var tenant = await _tenantService.Get(tenantId);
        return UserMockData.All
            .Where(u => u.RoleByTenantId.ContainsKey(tenantId))
            .Select(u => GetUser(u.Id))
            .Select(u => new TenantUser(u, tenant));
    }

    public async Task<IEnumerable<TenantUser>> GetUsersFromChildTenants(string parentTenantId)
    {
        var result = new List<TenantUser>();

        var childTenants = await _tenantService.GetParties();
        foreach (var childTenant in childTenants)
        {
            var activeUsers = UserMockData.All
                .Where(u => u.RoleByTenantId.ContainsKey(childTenant.Id))
                .Select(u => GetUser(u.Id));

            foreach (var user in activeUsers)
            {
                var existingUser = result.SingleOrDefault(u => u.User.Id == user.Id);

                if (existingUser != null)
                {
                    existingUser.Tenants.Add(childTenant);
                }
                else
                {
                    result.Add(new TenantUser(user, childTenant));
                }
            }
        }

        return result;
    }

    public Task<IEnumerable<PermissionClient.V1User>> GetWahlverwaltersForTenant(string tenantId)
    {
        var users = UserMockData.All
            .Where(u => u.RoleByTenantId.TryGetValue(tenantId, out var role) && role == Role.Wahlverwalter)
            .Select(u => new PermissionClient.V1User
            {
                Id = u.Id,
                Emails =
                [
                    new PermissionClient.Apiv1Email
                    {
                        Primary = true,
                        Email = u.Email,
                    },
                ],
                Lifecycle = PermissionClient.ApiStorageLifecycle.ACTIVE,
            });
        return Task.FromResult(users);
    }

    public Task RemoveRelevantAuthorizations(string loginId)
    {
        // Currently a no-op
        return Task.CompletedTask;
    }

    public Task<TenantUser> UpdateUser(string loginId, List<string> tenantIds)
    {
        // No-op
        return Task.FromResult(new TenantUser());
    }

    private IdentityClient.V1User GetUser(string id)
    {
        var user = UserMockData.All.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return null;
        }

        var mappedUser = new IdentityClient.V1User
        {
            Id = user.Id,
            Loginid = user.Id,
            Emails =
            [
                new IdentityClient.Apiv1Email
                {
                    Primary = true,
                    Email = user.Email,
                },
            ],
            Lifecycle = IdentityClient.ApiStorageLifecycle.ACTIVE,
        };
        return mappedUser;
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
}
