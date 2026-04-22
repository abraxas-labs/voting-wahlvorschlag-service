// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Integration.Tests.MockedData;
using Eawv.Service.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Voting.Lib.Testing.Utils;
using Xunit;

namespace Eawv.Service.Integration.Tests.ListTests;

public class UpdateListTest : BaseRestTest
{
    private static readonly string Url = $"api/elections/{ElectionMockData.ProporzElection.Id}/lists/";
    private static readonly string UrlArchivedElection = $"api/elections/{ElectionMockData.ArchivedElection.Id}/lists/";

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

    /// <summary>
    /// Ensures that updating a list on an archived election returns BadRequest.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task TestWithArchivedElectionShouldThrow()
    {
        await AssertStatus(
            () => ElectionAdminClient.PutAsJsonAsync(UrlArchivedElection + ListMockData.ArchivedElectionFdpList.Id, NewValidRequest()),
            HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Ensures that updating a list as Authority does not clear existing list union connections.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task TestAsElectionAdminShouldPreserveListUnionConnection()
    {
        var listUnionId = await SeedListUnion();

        await GetSuccessfulResponse<ListModel>(() => ElectionAdminClient.PutAsJsonAsync(
            Url + ListMockData.ProporzFdpList.Id, NewValidRequest()));

        var listUnionId2 = await GetListUnionId(ListMockData.ProporzFdpList.Id);
        listUnionId2.Should().Be(listUnionId);
    }

    /// <summary>
    /// Ensures that updating a list as Party does not clear existing list union connections.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task TestAsUserShouldPreserveListUnionConnection()
    {
        var listUnionId = await SeedListUnion();

        await GetSuccessfulResponse<ListModel>(() => UserClient.PutAsJsonAsync(
            Url + ListMockData.ProporzFdpList.Id, NewValidRequest()));

        var listUnionId2 = await GetListUnionId(ListMockData.ProporzFdpList.Id);
        listUnionId2.Should().Be(listUnionId);
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

    private async Task<Guid> SeedListUnion()
    {
        var listUnionId = Guid.NewGuid();
        await RunOnDb(async db =>
        {
            var union = new ListUnion
            {
                Id = listUnionId,
                CreatedBy = UserMockData.TestUser.Id,
                CreationDate = DateTime.UtcNow,
            };
            db.ListUnions.Add(union);
            await db.SaveChangesAsync();

            await db.Lists
                .Where(l => l.Id == ListMockData.ProporzFdpList.Id || l.Id == ListMockData.ProporzSpList.Id)
                .ExecuteUpdateAsync(s => s.SetProperty(l => l.ListUnionId, listUnionId));
        });
        return listUnionId;
    }

    private Task<Guid?> GetListUnionId(Guid listId)
    {
        return RunOnDb(db => db.Lists
            .Where(l => l.Id == listId)
            .Select(l => l.ListUnionId)
            .SingleAsync());
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
