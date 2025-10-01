// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Integration.Tests.MockedData;
using Eawv.Service.Models;
using Voting.Lib.Testing.Utils;
using Xunit;

namespace Eawv.Service.Integration.Tests.ElectionTests;

public class GetElectionTest : BaseRestTest
{
    private const string Url = "api/elections/";

    public GetElectionTest(TestApplicationFactory factory)
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
        var elections = await GetSuccessfulResponse<ElectionModel>(() => ElectionAdminClient.GetAsync(Url + ElectionMockData.ProporzElection.Id));
        elections.MatchSnapshot();
    }

    [Fact]
    public async Task TestAsUser()
    {
        var elections = await GetSuccessfulResponse<ElectionModel>(() => UserClient.GetAsync(Url + ElectionMockData.MajorzElection.Id));
        elections.MatchSnapshot();
    }

    [Fact]
    public async Task TestDifferentTenantShouldNotWork()
    {
        await AssertStatus(
            () => ElectionAdminClient.GetAsync(Url + ElectionMockData.GossauElection.Id),
            HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task TestFutureElectionAsUserShouldNotWork()
    {
        await AssertStatus(
            () => UserClient.GetAsync(Url + ElectionMockData.FutureElection.Id),
            HttpStatusCode.NotFound);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.Wahlverwalter;
        yield return Role.User;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.GetAsync(Url + ElectionMockData.ProporzElection.Id);
    }
}
