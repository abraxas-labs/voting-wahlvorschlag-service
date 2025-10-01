// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Integration.Tests.MockedData;
using Eawv.Service.Models;
using Voting.Lib.Testing.Utils;
using Xunit;

namespace Eawv.Service.Integration.Tests.BallotDocumentTests;

public class CreateBallotDocumentTest : BaseRestTest
{
    private static readonly string Url = $"api/elections/{ElectionMockData.ProporzElection.Id}/documents";

    public CreateBallotDocumentTest(TestApplicationFactory factory)
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
        var document = await GetSuccessfulResponse<EmptyBallotDocumentModel>(() => ElectionAdminClient.PostAsJsonAsync(Url, NewValidRequest()));
        document.MatchSnapshot(x => x.Id);
    }

    [Fact]
    public async Task TestWithDifferentTenantElectionShouldNotWork()
    {
        await AssertStatus(
            () => ElectionAdminClient.PostAsJsonAsync($"api/elections/{ElectionMockData.GossauElection.Id}/documents", NewValidRequest()),
            HttpStatusCode.NotFound);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.PostAsJsonAsync(Url, NewValidRequest());
    }

    private ModifyBallotDocumentModel NewValidRequest()
    {
        return new ModifyBallotDocumentModel
        {
            Name = "testing.pdf",
            Document = "%PDF-1.0\n1 0 obj<</Type/Catalog/Pages 2 0 R>>endobj 2 0 obj<</Type/Pages/Kids[3 0 R]/Count 1>>endobj 3 0 obj<</Type/Page/MediaBox[0 0 3 3]>>endobj"u8.ToArray(),
        };
    }
}
