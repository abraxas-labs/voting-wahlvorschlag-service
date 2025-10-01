// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Integration.Tests.MockedData;
using Xunit;

namespace Eawv.Service.Integration.Tests.BallotDocumentTests;

public class DeleteBallotDocumentTest : BaseRestTest
{
    private static readonly string Url = $"api/elections/{ElectionMockData.ProporzElection.Id}/documents/";

    public DeleteBallotDocumentTest(TestApplicationFactory factory)
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
        await AssertStatus(
            () => ElectionAdminClient.DeleteAsync(Url + BallotDocumentMockData.ProporzDocument.Id),
            HttpStatusCode.OK);
    }

    [Fact]
    public async Task TestListOfDifferentTenantShouldNotWork()
    {
        await AssertStatus(
            () => ElectionAdminClient.DeleteAsync($"api/elections/{ElectionMockData.GossauElection.Id}/documents/{BallotDocumentMockData.GossauDocument.Id}"),
            HttpStatusCode.NotFound);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.DeleteAsync(Url + BallotDocumentMockData.ProporzDocument.Id);
    }
}
