// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
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

namespace Eawv.Service.Integration.Tests.ListTests;

public class UpdateListTest : BaseRestTest
{
    private static readonly string Url = $"api/elections/{ElectionMockData.ProporzElection.Id}/lists/";

    public UpdateListTest(TestApplicationFactory factory)
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
        var list = await GetSuccessfulResponse<ListModel>(() => ElectionAdminClient.PutAsJsonAsync(Url + ListMockData.ProporzFdpList.Id, NewValidRequest()));
        list.MatchSnapshot();
    }

    [Fact]
    public async Task TestAsUser()
    {
        var list = await GetSuccessfulResponse<ListModel>(() => UserClient.PutAsJsonAsync(Url + ListMockData.ProporzFdpList.Id, NewValidRequest()));
        list.MatchSnapshot();
    }

    [Fact]
    public async Task TestWithRepresentativeOfDifferentTenantShouldThrow()
    {
        await AssertStatus(
            () => UserClient.PutAsJsonAsync(Url + ListMockData.ProporzFdpList.Id, NewValidRequest(x => x.Representative = UserMockData.SpUser.Id)),
            HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task TestWithDeputiesOfDifferentTenantShouldThrow()
    {
        await AssertStatus(
            () => UserClient.PutAsJsonAsync(Url + ListMockData.ProporzFdpList.Id, NewValidRequest(x => x.DeputyUsers = [UserMockData.SpUser.Id])),
            HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task TestWithMembersOfDifferentTenantShouldThrow()
    {
        await AssertStatus(
            () => UserClient.PutAsJsonAsync(Url + ListMockData.ProporzFdpList.Id, NewValidRequest(x => x.MemberUsers = [UserMockData.SpUser.Id])),
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

    private ModifyListModel NewValidRequest(Action<ModifyListModel> customizer = null)
    {
        var list = new ModifyListModel
        {
            Name = "Updated list",
            Description = "Updated list description",
            ResponsiblePartyTenantId = TenantMockData.FdpStGallen.Id,
            Indenture = "test",
            Representative = UserMockData.FdpUser.Id,
        };
        customizer?.Invoke(list);
        return list;
    }
}
