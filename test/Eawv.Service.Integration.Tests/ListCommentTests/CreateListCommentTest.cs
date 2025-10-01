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

namespace Eawv.Service.Integration.Tests.ListCommentTests;

public class CreateListCommentTest : BaseRestTest
{
    private static readonly string Url = $"api/elections/{ElectionMockData.ProporzElection.Id}/lists/{ListMockData.ProporzFdpList.Id}/comments?theme=sg";

    public CreateListCommentTest(TestApplicationFactory factory)
        : base(factory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await ElectionMockData.Seed(RunScoped);
        await ListMockData.Seed(RunScoped);
    }

    [Fact]
    public async Task TestAsElectionAdmin()
    {
        var comment = await GetSuccessfulResponse<ListCommentModel>(() => ElectionAdminClient.PostAsJsonAsync(Url, NewValidRequest()));
        comment.MatchSnapshot(x => x.Id);
    }

    [Fact]
    public async Task TestAsUser()
    {
        var comment = await GetSuccessfulResponse<ListCommentModel>(() => UserClient.PostAsJsonAsync(Url, NewValidRequest()));
        comment.MatchSnapshot(x => x.Id);
    }

    [Fact]
    public async Task TestDifferentTenantShouldNotWork()
    {
        await AssertStatus(
            () => UserClient.PostAsJsonAsync(
                $"api/elections/{ElectionMockData.GossauElection.Id}/lists/{ListMockData.GossauList.Id}/comments?theme=sg",
                NewValidRequest()),
            HttpStatusCode.Forbidden);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.User;
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.PostAsJsonAsync(Url, NewValidRequest());
    }

    private ModifyListCommentModel NewValidRequest()
    {
        return new ModifyListCommentModel
        {
            Content = "created comment",
        };
    }
}
