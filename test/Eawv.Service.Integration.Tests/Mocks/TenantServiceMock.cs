// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Exceptions;
using Eawv.Service.Integration.Tests.MockedData;
using Eawv.Service.Services;
using Voting.Lib.Iam.Services.ApiClient.Permission;

namespace Eawv.Service.Integration.Tests.Mocks;

public class TenantServiceMock : ITenantService
{
    private readonly AuthService _authService;

    public TenantServiceMock(AuthService authService)
    {
        _authService = authService;
    }

    public async Task AssertChildParent(string childId, string parentId)
    {
        if (parentId == null)
        {
            throw new PartyTenantDoesNotMatchParentException(childId, parentId);
        }

        var childsParentId = await GetParentTenantId(childId);
        if (childsParentId != parentId)
        {
            throw new PartyTenantDoesNotMatchParentException(childId, parentId);
        }
    }

    public Task<V1Tenant> Get(string tenantId)
    {
        return Task.FromResult(GetTenant(tenantId));
    }

    public async Task<string> GetParentOrCurrentTenantId()
    {
        // In theory, a Wahlverwalter should never have a parent tenant, as he must be a Wahlverwalter on the parent tenant itself.
        // But we make sure that we can handle rare edge cases (misconfiguration?) where a Wahlverwalter has a parent tenant
        // by ignoring it explicitly.
        var tenantId = _authService.GetTenantId();
        if (_authService.IsWahlverwalter)
        {
            return tenantId;
        }

        // Users should always have a parent tenant in theory, so GetParentTenantId() should return it.
        // We still support the edge case / misconfiguration where a parent tenant has not been set (yet).
        return await GetParentTenantId(tenantId) ?? tenantId;
    }

    public Task<string> GetParentTenantId(string tenantId)
    {
        var foundTenant = TenantMockData.All.FirstOrDefault(x => x.Id == tenantId);
        return Task.FromResult(foundTenant?.ParentId);
    }

    public Task CreateParty(string name)
    {
        return Task.CompletedTask;
    }

    public Task<IEnumerable<V1Tenant>> GetParties()
    {
        var tenantId = _authService.GetTenantId();
        var childTenants = TenantMockData.All
            .Where(x => x.ParentId == tenantId)
            .Select(x => GetTenant(x.Id));
        return Task.FromResult(childTenants);
    }

    public async Task RemoveParty(string id)
    {
        var partyIds = (await GetParties()).Select(p => p.Id).ToList();
        if (!partyIds.Contains(id))
        {
            throw new ForbiddenException(id);
        }
    }

    private V1Tenant GetTenant(string id)
    {
        var foundTenant = TenantMockData.All.FirstOrDefault(x => x.Id == id);
        return foundTenant == null
            ? null
            : new V1Tenant
            {
                Id = foundTenant.Id,
                Name = foundTenant.Name,
            };
    }
}
