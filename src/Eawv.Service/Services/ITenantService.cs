// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Threading.Tasks;
using Voting.Lib.Iam.Services.ApiClient.Permission;

namespace Eawv.Service.Services;

public interface ITenantService
{
    Task<V1Tenant> Get(string tenantId);

    Task AssertChildParent(string childId, string parentId);

    Task<string> GetParentOrCurrentTenantId();

    Task<string> GetParentTenantId(string tenantId);

    Task<IEnumerable<V1Tenant>> GetParties();

    Task CreateParty(string name);

    Task RemoveParty(string id);
}
