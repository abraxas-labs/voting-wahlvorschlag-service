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

public class UpdateListCommentTest : BaseRestTest
{
    private static readonly string Url = $"api/elections/{ElectionMockData.ProporzElection.Id}/lists/{ListMockData.ProporzFdpList.Id}/comments/{ListCommentMockData.ProporzFdpListComment.Id}?theme=sg";

    public UpdateListCommentTest(TestApplicationFactory factory)
        : base(factory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await ElectionMockData.Seed(RunScoped);
        await ListMockData.Seed(RunScoped);
        await ListCommentMockData.Seed(RunScoped);
    }

    [Fact]
    public async Task TestAsElectionAdmin()
    {
        var comment = await GetSuccessfulResponse<ListCommentModel>(() => ElectionAdminClient.PutAsJsonAsync(Url, NewValidRequest()));
        comment.MatchSnapshot();
    }

    [Fact]
    public async Task TestAsUser()
    {
        var comment = await GetSuccessfulResponse<ListCommentModel>(() => UserClient.PutAsJsonAsync(Url, NewValidRequest()));
        comment.MatchSnapshot();
    }

    [Fact]
    public async Task TestDifferentTenantShouldNotWork()
    {
        await AssertStatus(
            () => UserClient.PutAsJsonAsync(
                $"api/elections/{ElectionMockData.GossauElection.Id}/lists/{ListMockData.GossauList.Id}/comments/{ListCommentMockData.GossauListComment.Id}?theme=sg",
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
        return httpClient.PutAsJsonAsync(Url, NewValidRequest());
    }

    private ModifyListCommentModel NewValidRequest()
    {
        return new ModifyListCommentModel
        {
            Content = "updated comment",
        };
    }
}
