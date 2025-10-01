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

public static class BallotDocumentMockData
{
    public static BallotDocument ProporzDocument => new()
    {
        Id = new Guid("38e7850f-5f08-45cc-a578-e2bcb7fb60c3"),
        CreationDate = MockedClock.UtcNowDate,
        CreatedBy = UserMockData.StGallenUser.Id,
        ElectionId = ElectionMockData.ProporzElection.Id,
        Name = "test.pdf",
        Document = [1, 2, 3],
    };

    public static BallotDocument GossauDocument => new()
    {
        Id = new Guid("1808f41a-8476-4565-afa5-867c1833582d"),
        CreationDate = MockedClock.UtcNowDate,
        CreatedBy = UserMockData.GossauUser.Id,
        ElectionId = ElectionMockData.GossauElection.Id,
        Name = "gossau.pdf",
        Document = [4, 5, 6],
    };

    public static IEnumerable<BallotDocument> All
    {
        get
        {
            yield return ProporzDocument;
            yield return GossauDocument;
        }
    }

    public static Task Seed(Func<Func<IServiceProvider, Task>, Task> runScoped)
    {
        var all = All.ToList();
        return runScoped(async sp =>
        {
            var db = sp.GetRequiredService<EawvContext>();
            db.BallotDocuments.AddRange(all);
            await db.SaveChangesAsync();
        });
    }
}
