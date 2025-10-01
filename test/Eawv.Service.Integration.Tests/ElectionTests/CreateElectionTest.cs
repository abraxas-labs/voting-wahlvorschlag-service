// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Integration.Tests.MockedData;
using Eawv.Service.Models;
using Voting.Lib.Testing.Mocks;
using Voting.Lib.Testing.Utils;
using Xunit;

namespace Eawv.Service.Integration.Tests.ElectionTests;

public class CreateElectionTest : BaseRestTest
{
    private const string Url = "api/elections";

    public CreateElectionTest(TestApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task TestAsElectionAdmin()
    {
        var doi = await GetSuccessfulResponse<ElectionModel>(() => ElectionAdminClient.PostAsJsonAsync(Url, NewValidRequest()));
        doi.MatchSnapshot(x => x.Id);
    }

    [Fact]
    public async Task TestInvalidDomainOfInfluenceShouldNotWork()
    {
        var request = NewValidRequest(r =>
        {
            r.DomainsOfInfluence =
            [
                new CreateDomainOfInfluenceElectionModel
                {
                    Id = DomainOfInfluenceMockData.Gossau.Id,
                    NumberOfMandates = 3,
                },
            ];
        });
        await AssertStatus(
            () => ElectionAdminClient.PostAsJsonAsync(Url, request),
            HttpStatusCode.Forbidden);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.PostAsJsonAsync(Url, NewValidRequest());
    }

    private CreateElectionModel NewValidRequest(Action<CreateElectionModel> customizer = null)
    {
        var election = new CreateElectionModel
        {
            Name = "New election",
            Description = "New election description",
            AvailableFrom = MockedClock.UtcNowDate.AddDays(1),
            ContestDate = MockedClock.UtcNowDate.AddDays(20),
            ElectionType = ElectionType.Proporz,
            SubmissionDeadlineBegin = MockedClock.UtcNowDate.AddDays(2),
            SubmissionDeadlineEnd = MockedClock.UtcNowDate.AddDays(15),
        };
        customizer?.Invoke(election);
        return election;
    }
}
