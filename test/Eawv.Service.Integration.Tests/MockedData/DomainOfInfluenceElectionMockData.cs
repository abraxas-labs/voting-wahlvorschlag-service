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

public static class DomainOfInfluenceElectionMockData
{
    public static DomainOfInfluenceElection StGallenProporzElection => new()
    {
        Id = new Guid("0c292f5f-86d3-4dd7-ad5b-28b97902b751"),
        NumberOfMandates = 5,
        CreationDate = MockedClock.UtcNowDate.AddDays(-2),
        CreatedBy = UserMockData.TestUser.Id,
        DomainOfInfluenceId = DomainOfInfluenceMockData.StGallen.Id,
        ElectionId = ElectionMockData.ProporzElection.Id,
    };

    public static DomainOfInfluenceElection GossauElection => new()
    {
        Id = new Guid("6c74708f-d533-463b-8570-c61b60b030de"),
        NumberOfMandates = 4,
        CreationDate = MockedClock.UtcNowDate.AddDays(-2),
        CreatedBy = UserMockData.GossauPartyUser.Id,
        DomainOfInfluenceId = DomainOfInfluenceMockData.Gossau.Id,
        ElectionId = ElectionMockData.GossauElection.Id,
    };

    public static IEnumerable<DomainOfInfluenceElection> All
    {
        get
        {
            yield return StGallenProporzElection;
            yield return GossauElection;
        }
    }

    public static Task Seed(Func<Func<IServiceProvider, Task>, Task> runScoped)
    {
        var all = All.ToList();
        return runScoped(async sp =>
        {
            var db = sp.GetRequiredService<EawvContext>();
            db.DomainOfInfluenceElections.AddRange(all);
            await db.SaveChangesAsync();
        });
    }
}
