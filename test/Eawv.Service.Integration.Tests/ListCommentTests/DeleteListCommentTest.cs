// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Integration.Tests.MockedData;
using Xunit;

namespace Eawv.Service.Integration.Tests.ListCommentTests;

public class DeleteListCommentTest : BaseRestTest
{
    private static readonly string Url = $"api/elections/{ElectionMockData.ProporzElection.Id}/lists/{ListMockData.ProporzFdpList.Id}/comments/{ListCommentMockData.ProporzFdpListComment.Id}?theme=sg";
    private static readonly string UrlArchivedElection = $"api/elections/{ElectionMockData.ArchivedElection.Id}/lists/{ListMockData.ArchivedElectionFdpList.Id}/comments/{ListCommentMockData.ArchivedElectionListComment.Id}?theme=sg";

    public DeleteListCommentTest(TestApplicationFactory factory)
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
        await AssertStatus(
            () => ElectionAdminClient.DeleteAsync(Url),
            HttpStatusCode.OK);
    }

    [Fact]
    public async Task TestAsUser()
    {
        await AssertStatus(
            () => UserClient.DeleteAsync(Url),
            HttpStatusCode.OK);
    }

    [Fact]
    public async Task TestDifferentTenantShouldNotWork()
    {
        await AssertStatus(
            () => UserClient.DeleteAsync(
                $"api/elections/{ElectionMockData.GossauElection.Id}/lists/{ListMockData.GossauList.Id}/comments/{ListCommentMockData.GossauListComment.Id}?theme=sg"),
            HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task TestWrongElectionIdShouldNotWork()
    {
        await AssertStatus(
            () => UserClient.DeleteAsync(
                $"api/elections/{ElectionMockData.FutureAvailableElection.Id}/lists/{ListMockData.ProporzFdpListFutureAvailableElection.Id}/comments/{ListCommentMockData.ProporzFdpListComment.Id}?theme=sg"),
            HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Ensures that deleting a comment on an archived election returns BadRequest.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task TestWithArchivedElectionShouldThrow()
    {
        await AssertStatus(
            () => ElectionAdminClient.DeleteAsync(UrlArchivedElection),
            HttpStatusCode.BadRequest);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.User;
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.DeleteAsync(Url);
    }
}
