// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using IdentityClient = Voting.Lib.Iam.Services.ApiClient.Identity;
using PermissionClient = Voting.Lib.Iam.Services.ApiClient.Permission;

namespace Eawv.Service.Models;

public class TenantUser
{
    public TenantUser()
    {
    }

    public TenantUser(IdentityClient.V1User user, PermissionClient.V1Tenant tenant)
    {
        User = user;
        Tenants = new List<PermissionClient.V1Tenant> { tenant };
    }

    public TenantUser(IdentityClient.V1User user, List<PermissionClient.Abraxaspermissionapiv1Authorization> auths)
    {
        User = user;
        Tenants = auths.Select(a => a.Tenant).ToList();
    }

    public IdentityClient.V1User User { get; set; }

    public List<PermissionClient.V1Tenant> Tenants { get; set; }
}
