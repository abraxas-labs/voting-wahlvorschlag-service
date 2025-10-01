// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eawv.Service.DataAccess;
using Eawv.Service.DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Iam.Testing.AuthenticationScheme;
using Voting.Lib.Testing.Mocks;

namespace Eawv.Service.Integration.Tests.MockedData;

public static class DomainOfInfluenceMockData
{
    public static DomainOfInfluence StGallen => new()
    {
        Id = new Guid("e273bf99-22fc-4a68-85f5-2172628fcef0"),
        CreationDate = MockedClock.UtcNowDate,
        CreatedBy = SecureConnectTestDefaults.MockedUserDefault.Loginid,
        Name = "St. Gallen",
        TenantId = SecureConnectTestDefaults.MockedTenantDefault.Id,
        DomainOfInfluenceType = DomainOfInfluenceType.MU,
        OfficialId = "St. Gallen",
        ShortName = "SG",
    };

    public static DomainOfInfluence Gossau => new()
    {
        Id = new Guid("b00c1032-c65b-4f2e-b75a-3acea8cc8351"),
        CreationDate = MockedClock.UtcNowDate.AddDays(2),
        CreatedBy = UserMockData.GossauUser.Id,
        Name = "Gossau",
        TenantId = TenantMockData.Gossau.Id,
        DomainOfInfluenceType = DomainOfInfluenceType.MU,
        OfficialId = "Gossau",
        ShortName = "Gossau",
    };

    public static IEnumerable<DomainOfInfluence> All
    {
        get
        {
            yield return StGallen;
            yield return Gossau;
        }
    }

    public static Task Seed(Func<Func<IServiceProvider, Task>, Task> runScoped)
    {
        var all = All.ToList();
        return runScoped(async sp =>
        {
            var db = sp.GetRequiredService<EawvContext>();
            db.DomainsOfInfluence.AddRange(all);
            await db.SaveChangesAsync();
        });
    }
}
