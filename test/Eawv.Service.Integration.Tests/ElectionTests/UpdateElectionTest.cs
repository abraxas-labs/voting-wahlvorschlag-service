// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
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

public class UpdateElectionTest : BaseRestTest
{
    private const string Url = "api/elections/";

    public UpdateElectionTest(TestApplicationFactory factory)
        : base(factory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await ElectionMockData.Seed(RunScoped);
    }

    [Fact]
    public async Task TestAsElectionAdmin()
    {
        var doi = await GetSuccessfulResponse<ElectionOverviewModel>(
            () => ElectionAdminClient.PutAsJsonAsync(Url + ElectionMockData.ProporzElection.Id, NewValidRequest()));
        doi.MatchSnapshot();
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.PutAsJsonAsync(Url + ElectionMockData.ProporzElection.Id, NewValidRequest());
    }

    private CreateElectionModel NewValidRequest(Action<CreateElectionModel> customizer = null)
    {
        var election = new CreateElectionModel
        {
            Name = "Updated election",
            Description = "Updated election description",
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
