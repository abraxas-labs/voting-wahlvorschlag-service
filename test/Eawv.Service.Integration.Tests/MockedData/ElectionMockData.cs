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

/// <summary>
/// Provides mock election data for integration tests.
/// </summary>
public static class ElectionMockData
{
    /// <summary>
    /// Gets an archived election.
    /// </summary>
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

    /// <summary>
    /// Gets a election that has already taken place but is not yet archived.
    /// </summary>
    public static Election PastElection => new()
    {
        Id = new Guid("228C20F9-CACD-4EA1-ADC5-07CD50F73ED9"),
        CreationDate = MockedClock.UtcNowDate.AddDays(-50),
        CreatedBy = SecureConnectTestDefaults.MockedUserDefault.Loginid,
        TenantId = TenantMockData.StGallen.Id,
        Name = "Past election",
        Description = "Past",
        SubmissionDeadlineBegin = MockedClock.UtcNowDate.AddDays(-40),
        SubmissionDeadlineEnd = MockedClock.UtcNowDate.AddDays(-7),
        ContestDate = MockedClock.UtcNowDate.AddDays(-1),
        ElectionType = ElectionType.Majorz,
        State = 0,
        TenantLogo = [],
        AvailableFrom = MockedClock.UtcNowDate.AddDays(-41),
    };

    /// <summary>
    /// Gets a Majorz election with an active submission period.
    /// </summary>
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

    /// <summary>
    /// Gets a Proporz election with an active submission period.
    /// </summary>
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

    /// <summary>
    /// Gets a future election that is not yet available.
    /// </summary>
    public static Election FutureUnavailableElection => new()
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

    /// <summary>
    /// Gets a future election that is already available.
    /// </summary>
    public static Election FutureAvailableElection => new()
    {
        Id = new Guid("5168FC5E-55EC-432C-98CD-5136A2806A68"),
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
        AvailableFrom = MockedClock.UtcNowDate.AddDays(-20),
    };

    /// <summary>
    /// Gets a Gossau election with an active submission period.
    /// </summary>
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

    /// <summary>
    /// Gets all mock elections.
    /// </summary>
    public static IEnumerable<Election> All
    {
        get
        {
            yield return PastElection;
            yield return ArchivedElection;
            yield return MajorzElection;
            yield return ProporzElection;
            yield return FutureUnavailableElection;
            yield return FutureAvailableElection;
            yield return GossauElection;
        }
    }

    /// <summary>
    /// Seeds all mock elections into the database context.
    /// </summary>
    /// <param name="runScoped">A function to run a scoped service provider.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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
