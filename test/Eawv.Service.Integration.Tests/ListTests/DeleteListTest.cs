// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Integration.Tests.MockedData;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Eawv.Service.Integration.Tests.ListTests;

public class DeleteListTest : BaseRestTest
{
    private static readonly string Url = $"api/elections/{ElectionMockData.MajorzElection.Id}/lists/";

    public DeleteListTest(TestApplicationFactory factory)
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
        await AssertStatus(
            () => ElectionAdminClient.DeleteAsync(Url + ListMockData.MajorzFdpList.Id),
            HttpStatusCode.OK);
    }

    [Fact]
    public async Task TestAsUser()
    {
        await AssertStatus(
            () => UserClient.DeleteAsync(Url + ListMockData.MajorzFdpList.Id),
            HttpStatusCode.OK);
    }

    [Fact]
    public async Task TestListOfDifferentTenantShouldNotWork()
    {
        await AssertStatus(
            () => UserClient.DeleteAsync($"api/elections/{ElectionMockData.GossauElection.Id}/lists/{ListMockData.GossauList.Id}"),
            HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task TestUserCannotDeleteLockedList()
    {
        await RunOnDb(db =>
        {
            return db.Lists
                .Where(l => l.Id == ListMockData.MajorzFdpList.Id)
                .ExecuteUpdateAsync(setters => setters.SetProperty(l => l.Locked, true));
        });
        await AssertStatus(
            () => UserClient.DeleteAsync(Url + ListMockData.MajorzFdpList.Id),
            HttpStatusCode.BadRequest);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.User;
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.DeleteAsync(Url + ListMockData.MajorzFdpList.Id);
    }
}
