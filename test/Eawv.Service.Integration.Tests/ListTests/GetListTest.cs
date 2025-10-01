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

namespace Eawv.Service.Integration.Tests.ListTests;

public class GetListTest : BaseRestTest
{
    private static readonly string Url = $"api/elections/{ElectionMockData.MajorzElection.Id}/lists/";

    public GetListTest(TestApplicationFactory factory)
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
        var list = await GetSuccessfulResponse<ListModel>(() => ElectionAdminClient.GetAsync(Url + ListMockData.MajorzFdpList.Id));
        list.MatchSnapshot();
    }

    [Fact]
    public async Task TestAsUser()
    {
        var list = await GetSuccessfulResponse<ListModel>(() => UserClient.GetAsync(Url + ListMockData.MajorzFdpList.Id));
        list.MatchSnapshot();
    }

    [Fact]
    public async Task TestListOfDifferentTenantShouldNotWork()
    {
        await AssertStatus(
            () => UserClient.GetAsync($"api/elections/{ElectionMockData.GossauElection.Id}/lists/{ListMockData.GossauList.Id}"),
            HttpStatusCode.Forbidden);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.User;
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.GetAsync(Url + ListMockData.MajorzFdpList.Id);
    }
}
