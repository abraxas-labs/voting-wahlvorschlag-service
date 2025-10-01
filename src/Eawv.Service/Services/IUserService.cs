// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Threading.Tasks;
using Eawv.Service.Models;
using IdentityClient = Voting.Lib.Iam.Services.ApiClient.Identity;
using PermissionClient = Voting.Lib.Iam.Services.ApiClient.Permission;

namespace Eawv.Service.Services;

/// <summary>
/// Service for handling users and user data.
/// </summary>
public interface IUserService
{
    Task<string> GetCurrentUserName();

    /// <summary>
    /// Gets a user by its id. This method does NOT check whether the current user has access to it.
    /// </summary>
    /// <param name="id">The user id.</param>
    /// <returns>The user.</returns>
    Task<IdentityClient.V1User> Get(string id);

    Task<IEnumerable<PermissionClient.V1User>> GetWahlverwaltersForTenant(string tenantId);

    Task<TenantUser> CreateUser(IdentityClient.V1User user, string email, List<string> tenantIds);

    Task<TenantUser> UpdateUser(string loginId, List<string> tenantIds);

    /// <summary>
    /// Removes EAWv authorizations for a specific user, but only those on child tenants.
    /// </summary>
    /// <param name="loginId">The users login id to remove the authorization for.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RemoveRelevantAuthorizations(string loginId);

    /// <summary>
    /// Returns all EAWv and Adminpanel authorizations of the specified user,
    /// but only where the tenant is a child tenant of the current tenant.
    /// </summary>
    /// <param name="loginId">The login id of the user.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, including the requested <see cref="Authorization"/>.</returns>
    Task<List<PermissionClient.Abraxaspermissionapiv1Authorization>> GetParentAuthorizationsForUser(string loginId);

    Task<IEnumerable<TenantUser>> GetUsersForTenant(string tenantId);

    Task<IEnumerable<TenantUser>> GetUsersFromChildTenants(string parentTenantId);
}
