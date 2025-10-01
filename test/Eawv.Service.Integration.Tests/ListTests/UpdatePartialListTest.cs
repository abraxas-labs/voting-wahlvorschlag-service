// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Integration.Tests.MockedData;
using Eawv.Service.Integration.Tests.Mocks;
using Eawv.Service.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Voting.Lib.Testing.Utils;
using Xunit;

namespace Eawv.Service.Integration.Tests.ListTests;

public class UpdatePartialListTest : BaseRestTest
{
    private static readonly string Url = $"api/elections/{ElectionMockData.ProporzElection.Id}/lists/";

    public UpdatePartialListTest(TestApplicationFactory factory)
        : base(factory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await ElectionMockData.Seed(RunScoped);
        await ListMockData.Seed(RunScoped);

        NotificationServiceMock.SentEmails.Clear();
    }

    [Fact]
    public async Task TestAsElectionAdmin()
    {
        await RunOnDb(db =>
        {
            return db.Lists
                .Where(l => l.Id == ListMockData.ProporzFdpList.Id)
                .ExecuteUpdateAsync(setters => setters.SetProperty(l => l.State, ListState.Submitted));
        });
        var list = await GetSuccessfulResponse<ListModel>(
            () => ElectionAdminClient.PatchAsJsonAsync(Url + ListMockData.ProporzFdpList.Id + "?theme=sg", new PatchListModel { State = ListState.FormallySubmitted }));
        list.MatchSnapshot("list");

        NotificationServiceMock.SentEmails.Count.Should().Be(1);
        NotificationServiceMock.SentEmails[0].MatchSnapshot("notification");
    }

    [Fact]
    public async Task TestAsUser()
    {
        var list = await GetSuccessfulResponse<ListModel>(() => UserClient.PatchAsJsonAsync(Url + ListMockData.ProporzFdpList.Id + "?theme=sg", NewValidRequest()));
        list.MatchSnapshot("list");

        NotificationServiceMock.SentEmails.Count.Should().Be(1);
        NotificationServiceMock.SentEmails[0].MatchSnapshot("notification");
    }

    [Theory]
    [InlineData(ListState.FormallySubmitted)]
    [InlineData(ListState.Archived)]
    [InlineData(ListState.Valid)]
    public async Task TestWithUserInvalidStates(ListState state)
    {
        await AssertStatus(
            () => UserClient.PatchAsJsonAsync(Url + ListMockData.ProporzFdpList.Id + "?theme=sg", new PatchListModel { State = state }),
            HttpStatusCode.BadRequest);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.User;
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.PatchAsJsonAsync(Url + ListMockData.ProporzFdpList.Id + "?theme=sg", NewValidRequest());
    }

    private PatchListModel NewValidRequest()
    {
        return new PatchListModel
        {
            State = ListState.Submitted,
        };
    }
}
