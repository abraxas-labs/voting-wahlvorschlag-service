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

public static class ElectionMockData
{
    public static Election ArchivedElection => new()
    {
        Id = new Guid("f0bb46db-c8d9-499f-a329-dbf3d3fc64dd"),
        CreationDate = MockedClock.UtcNowDate.AddDays(-103),
        CreatedBy = SecureConnectTestDefaults.MockedUserDefault.Loginid,
        TenantId = TenantMockData.StGallen.Id,
        Name = "Archived election",
        Description = "Archived",
        SubmissionDeadlineBegin = MockedClock.UtcNowDate.AddDays(-100),
        SubmissionDeadlineEnd = MockedClock.UtcNowDate.AddDays(-90),
        ContestDate = MockedClock.UtcNowDate.AddDays(-50),
        ElectionType = ElectionType.Majorz,
        State = 0,
        TenantLogo = [],
        AvailableFrom = MockedClock.UtcNowDate.AddDays(-102),
    };

    public static Election MajorzElection => new()
    {
        Id = new Guid("f1df3d77-1be3-44f1-9e2f-ad20ff72624a"),
        CreationDate = MockedClock.UtcNowDate.AddDays(-1),
        CreatedBy = SecureConnectTestDefaults.MockedUserDefault.Loginid,
        TenantId = TenantMockData.StGallen.Id,
        Name = "Test Majorzwahl",
        Description = "Test Majorz",
        SubmissionDeadlineBegin = MockedClock.UtcNowDate.AddDays(-1),
        SubmissionDeadlineEnd = MockedClock.UtcNowDate.AddDays(10),
        ContestDate = MockedClock.UtcNowDate.AddDays(30),
        ElectionType = ElectionType.Majorz,
        State = 0,
        TenantLogo = [],
        AvailableFrom = MockedClock.UtcNowDate.AddDays(-1),
    };

    public static Election ProporzElection => new()
    {
        Id = new Guid("e8d78202-fbd8-447f-a045-cf6a7402875b"),
        CreationDate = MockedClock.UtcNowDate.AddDays(-2),
        CreatedBy = SecureConnectTestDefaults.MockedUserDefault.Loginid,
        TenantId = TenantMockData.StGallen.Id,
        Name = "Test Proporzwahl",
        Description = "Test Proporz",
        SubmissionDeadlineBegin = MockedClock.UtcNowDate.AddDays(-1),
        SubmissionDeadlineEnd = MockedClock.UtcNowDate.AddDays(10),
        ContestDate = MockedClock.UtcNowDate.AddDays(30),
        ElectionType = ElectionType.Proporz,
        State = 0,
        TenantLogo = [],
        AvailableFrom = MockedClock.UtcNowDate.AddDays(-1),
    };

    public static Election FutureElection => new()
    {
        Id = new Guid("f50e91b5-de35-4e4b-8181-e198a8a3a77e"),
        CreationDate = MockedClock.UtcNowDate,
        CreatedBy = SecureConnectTestDefaults.MockedUserDefault.Loginid,
        TenantId = TenantMockData.StGallen.Id,
        Name = "Future election",
        Description = "Future",
        SubmissionDeadlineBegin = MockedClock.UtcNowDate.AddDays(20),
        SubmissionDeadlineEnd = MockedClock.UtcNowDate.AddDays(40),
        ContestDate = MockedClock.UtcNowDate.AddDays(60),
        ElectionType = ElectionType.Proporz,
        State = 0,
        TenantLogo = [],
        AvailableFrom = MockedClock.UtcNowDate.AddDays(20),
    };

    public static Election GossauElection => new()
    {
        Id = new Guid("d4a87686-4b2c-4485-905a-7aaa35aee97f"),
        CreationDate = MockedClock.UtcNowDate,
        CreatedBy = UserMockData.GossauPartyUser.Id,
        TenantId = TenantMockData.GossauParty.Id,
        Name = "Gossau Election",
        Description = "Gossau",
        SubmissionDeadlineBegin = MockedClock.UtcNowDate.AddDays(-1),
        SubmissionDeadlineEnd = MockedClock.UtcNowDate.AddDays(10),
        ContestDate = MockedClock.UtcNowDate.AddDays(30),
        ElectionType = ElectionType.Proporz,
        State = 0,
        TenantLogo = [],
        AvailableFrom = MockedClock.UtcNowDate.AddDays(-1),
    };

    public static IEnumerable<Election> All
    {
        get
        {
            yield return ArchivedElection;
            yield return MajorzElection;
            yield return ProporzElection;
            yield return FutureElection;
            yield return GossauElection;
        }
    }

    public static Task Seed(Func<Func<IServiceProvider, Task>, Task> runScoped)
    {
        var all = All.ToList();
        return runScoped(async sp =>
        {
            var db = sp.GetRequiredService<EawvContext>();
            db.Elections.AddRange(all);
            await db.SaveChangesAsync();
        });
    }
}
