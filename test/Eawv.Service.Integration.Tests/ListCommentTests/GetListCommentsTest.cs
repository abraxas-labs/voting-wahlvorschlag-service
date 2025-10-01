// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Integration.Tests.MockedData;
using Eawv.Service.Models;
using Voting.Lib.Testing.Utils;
using Xunit;

namespace Eawv.Service.Integration.Tests.ListCommentTests;

public class GetListCommentsTest : BaseRestTest
{
    private static readonly string Url = $"api/elections/{ElectionMockData.ProporzElection.Id}/lists/{ListMockData.ProporzFdpList.Id}/comments";

    public GetListCommentsTest(TestApplicationFactory factory)
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
        var comments = await GetSuccessfulResponse<List<ListCommentModel>>(() => ElectionAdminClient.GetAsync(Url));
        comments.MatchSnapshot();
    }

    [Fact]
    public async Task TestAsUser()
    {
        var comments = await GetSuccessfulResponse<List<ListCommentModel>>(() => UserClient.GetAsync(Url));
        comments.MatchSnapshot();
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.User;
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.GetAsync(Url);
    }
}
