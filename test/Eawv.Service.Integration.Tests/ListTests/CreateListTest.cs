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

public class CreateListTest : BaseRestTest
{
    private static readonly string UrlProporzElection = $"api/elections/{ElectionMockData.ProporzElection.Id}/lists";
    private static readonly string UrlFutureElection = $"api/elections/{ElectionMockData.FutureUnavailableElection.Id}/lists";
    private static readonly string UrlFutureAvailableElection = $"api/elections/{ElectionMockData.FutureAvailableElection.Id}/lists";
    private static readonly string UrlPastElection = $"api/elections/{ElectionMockData.PastElection.Id}/lists";
    private static readonly string UrlArchivedElection = $"api/elections/{ElectionMockData.ArchivedElection.Id}/lists";

    public CreateListTest(TestApplicationFactory factory)
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
        var list = await GetSuccessfulResponse<ListModel>(() => ElectionAdminClient.PostAsJsonAsync(UrlProporzElection, NewValidRequest()));
        list.MatchSnapshot(x => x.Id);
    }

    [Fact]
    public async Task TestAsUser()
    {
        var list = await GetSuccessfulResponse<ListModel>(() => UserClient.PostAsJsonAsync(UrlProporzElection, NewValidRequest()));
        list.MatchSnapshot(x => x.Id);
    }

    [Fact]
    public async Task TestWithRepresentativeOfDifferentTenantShouldThrow()
    {
        await AssertStatus(
            () => UserClient.PostAsJsonAsync(UrlProporzElection, NewValidRequest(x => x.Representative = UserMockData.SpUser.Id)),
            HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task TestWithDeputiesOfDifferentTenantShouldThrow()
    {
        await AssertStatus(
            () => UserClient.PostAsJsonAsync(UrlProporzElection, NewValidRequest(x => x.DeputyUsers = [UserMockData.SpUser.Id])),
            HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task TestWithMembersOfDifferentTenantShouldThrow()
    {
        await AssertStatus(
            () => UserClient.PostAsJsonAsync(UrlProporzElection, NewValidRequest(x => x.MemberUsers = [UserMockData.SpUser.Id])),
            HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Ensures that creating a list for a future election that is already visible for parties returns Forbidden.
    /// Lists cannot be created outside the submission time period.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task TestWithFutureAvailableElectionShouldThrow()
    {
        await AssertStatus(
            () => UserClient.PostAsJsonAsync(UrlFutureAvailableElection, NewValidRequest()),
            HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Ensures that creating a list for a future election that is not yet visible for parties returns NotFound.
    /// Lists cannot be seen or created before the election becomes available.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task TestWithFutureElectionShouldThrow()
    {
        await AssertStatus(
            () => UserClient.PostAsJsonAsync(UrlFutureElection, NewValidRequest()),
            HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Ensures that creating a list for a past election that is not yet archieved returns Forbidden.
    /// Lists cannot be created outside the submission time period.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task TestWithPastElectionShouldThrow()
    {
        await AssertStatus(
            () => UserClient.PostAsJsonAsync(UrlPastElection, NewValidRequest()),
            HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Ensures that creating a list for an archived election returns BadRequest.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task TestWithArchivedElectionShouldThrow()
    {
        await AssertStatus(
            () => UserClient.PostAsJsonAsync(UrlArchivedElection, NewValidRequest()),
            HttpStatusCode.BadRequest);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.User;
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.PostAsJsonAsync(UrlProporzElection, NewValidRequest());
    }

    private ModifyListModel NewValidRequest(Action<ModifyListModel> customizer = null)
    {
        var list = new ModifyListModel
        {
            Name = "New list",
            Description = "New list description",
            ResponsiblePartyTenantId = TenantMockData.FdpStGallen.Id,
            Indenture = "test",
            Representative = UserMockData.FdpUser.Id,
        };
        customizer?.Invoke(list);
        return list;
    }
}
