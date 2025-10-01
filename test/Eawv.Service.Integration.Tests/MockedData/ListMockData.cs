// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eawv.Service.DataAccess;
using Eawv.Service.DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Testing.Mocks;

namespace Eawv.Service.Integration.Tests.MockedData;

public static class ListMockData
{
    public static List ProporzFdpList => new()
    {
        Id = new Guid("573cfaef-363f-44b5-b116-c17d23c193eb"),
        CreationDate = MockedClock.UtcNowDate,
        CreatedBy = UserMockData.TestUser.Id,
        ResponsiblePartyTenantId = TenantMockData.FdpStGallen.Id,
        Name = "List FDP",
        Description = "FDP",
        SortOrder = 0,
        State = ListState.Draft,
        ElectionId = ElectionMockData.ProporzElection.Id,
        DeputyUsers = [],
        MemberUsers = [],
        Representative = UserMockData.FdpUser.Id,
    };

    public static List ProporzSpList => new()
    {
        Id = new Guid("7cd4df7d-909b-4ece-885b-9a301a02bc0f"),
        CreationDate = MockedClock.UtcNowDate,
        CreatedBy = UserMockData.TestUser.Id,
        ResponsiblePartyTenantId = TenantMockData.SpStGallen.Id,
        Name = "List SP",
        Description = "SP",
        SortOrder = 1,
        State = ListState.Draft,
        ElectionId = ElectionMockData.ProporzElection.Id,
        DeputyUsers = [],
        MemberUsers = [],
        Representative = UserMockData.SpUser.Id,
    };

    public static List MajorzFdpList => new()
    {
        Id = new Guid("b69a9757-95b1-4bdd-97c0-79de214ba1a5"),
        CreationDate = MockedClock.UtcNowDate,
        CreatedBy = UserMockData.TestUser.Id,
        ResponsiblePartyTenantId = TenantMockData.FdpStGallen.Id,
        Name = "Max Muster",
        Description = "Max Muster (FDP)",
        SortOrder = 0,
        State = ListState.Draft,
        ElectionId = ElectionMockData.MajorzElection.Id,
        DeputyUsers = [UserMockData.TestUser.Id],
        MemberUsers = [],
        Representative = UserMockData.FdpUser.Id,
    };

    public static List GossauList => new()
    {
        Id = new Guid("778120d9-c54a-4fc5-88f5-c57edf1c2f76"),
        CreationDate = MockedClock.UtcNowDate,
        CreatedBy = UserMockData.GossauPartyUser.Id,
        ResponsiblePartyTenantId = TenantMockData.GossauParty.Id,
        Name = "Gossau",
        Description = "Gossau List",
        SortOrder = 0,
        State = ListState.Draft,
        ElectionId = ElectionMockData.GossauElection.Id,
        DeputyUsers = [],
        MemberUsers = [],
        Representative = UserMockData.GossauPartyUser.Id,
    };

    public static IEnumerable<List> All
    {
        get
        {
            yield return ProporzFdpList;
            yield return ProporzSpList;
            yield return MajorzFdpList;
            yield return GossauList;
        }
    }

    public static Task Seed(Func<Func<IServiceProvider, Task>, Task> runScoped)
    {
        var all = All.ToList();
        return runScoped(async sp =>
        {
            var db = sp.GetRequiredService<EawvContext>();
            db.Lists.AddRange(all);
            await db.SaveChangesAsync();
        });
    }
}
