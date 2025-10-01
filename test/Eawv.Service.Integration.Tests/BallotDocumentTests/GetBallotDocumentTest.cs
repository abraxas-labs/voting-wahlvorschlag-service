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

namespace Eawv.Service.Integration.Tests.BallotDocumentTests;

public class GetBallotDocumentTest : BaseRestTest
{
    private static readonly string Url = $"api/elections/{ElectionMockData.ProporzElection.Id}/documents/";

    public GetBallotDocumentTest(TestApplicationFactory factory)
        : base(factory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await ElectionMockData.Seed(RunScoped);
        await BallotDocumentMockData.Seed(RunScoped);
    }

    [Fact]
    public async Task TestAsElectionAdmin()
    {
        var document = await GetSuccessfulResponse<BallotDocumentModel>(() => ElectionAdminClient.GetAsync(Url + BallotDocumentMockData.ProporzDocument.Id));
        document.MatchSnapshot();
    }

    [Fact]
    public async Task TestAsUser()
    {
        var document = await GetSuccessfulResponse<BallotDocumentModel>(() => UserClient.GetAsync(Url + BallotDocumentMockData.ProporzDocument.Id));
        document.MatchSnapshot();
    }

    [Fact]
    public async Task TestListOfDifferentTenantShouldNotWork()
    {
        await AssertStatus(
            () => UserClient.GetAsync($"api/elections/{ElectionMockData.GossauElection.Id}/documents/{BallotDocumentMockData.GossauDocument.Id}"),
            HttpStatusCode.NotFound);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.User;
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.GetAsync(Url + BallotDocumentMockData.ProporzDocument.Id);
    }
}
