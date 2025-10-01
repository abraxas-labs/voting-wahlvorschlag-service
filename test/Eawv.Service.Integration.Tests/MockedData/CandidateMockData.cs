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

public static class CandidateMockData
{
    public static Candidate ProporzFdpListCandidate => new()
    {
        Id = new Guid("2e391efd-9269-4308-9a5f-8a5130cf423f"),
        CreationDate = MockedClock.UtcNowDate,
        CreatedBy = UserMockData.TestUser.Id,
        ListId = ListMockData.ProporzFdpList.Id,
        FirstName = "Max",
        FamilyName = "Muster",
        BallotFirstName = "Max",
        BallotFamilyName = "Muster",
        Locality = "St. Gallen",
        OccupationalTitle = "Bäcker",
        BallotLocality = "St. Gallen",
        BallotOccupationalTitle = "Bäcker",
        Index = 1,
        DateOfBirth = new DateTime(1956, 2, 3, 0, 0, 0, DateTimeKind.Utc),
        OrderIndex = 1,
        ZipCode = "9000",
        Sex = SexType.Male,
        Street = "Teststreet",
    };

    public static Candidate ProporzSpListCandidate => new()
    {
        Id = new Guid("61358ac5-6831-4eaf-93d3-a285580df729"),
        CreationDate = MockedClock.UtcNowDate,
        CreatedBy = UserMockData.SpUser.Id,
        ListId = ListMockData.ProporzSpList.Id,
        FirstName = "Frieda",
        FamilyName = "Frei",
        BallotFirstName = "Frieda",
        BallotFamilyName = "Frieda",
        Locality = "St. Gallen",
        OccupationalTitle = "CEO",
        BallotLocality = "St. Gallen",
        BallotOccupationalTitle = "CEO",
        Index = 1,
        DateOfBirth = new DateTime(1962, 12, 23, 0, 0, 0, DateTimeKind.Utc),
        OrderIndex = 1,
        ZipCode = "9000",
        Sex = SexType.Female,
        Street = "AtHome",
    };

    public static IEnumerable<Candidate> All
    {
        get
        {
            yield return ProporzFdpListCandidate;
            yield return ProporzSpListCandidate;
        }
    }

    public static Task Seed(Func<Func<IServiceProvider, Task>, Task> runScoped)
    {
        var all = All.ToList();
        return runScoped(async sp =>
        {
            var db = sp.GetRequiredService<EawvContext>();
            db.Candidates.AddRange(all);
            await db.SaveChangesAsync();
        });
    }
}
