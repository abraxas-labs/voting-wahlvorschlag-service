// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Integration.Tests.MockedData;
using Eawv.Service.Models;
using FluentAssertions;
using Voting.Lib.Testing.Utils;
using Xunit;

namespace Eawv.Service.Integration.Tests.UserTests;

public class GetUsersForTenant : BaseRestTest
{
    private const string Url = "api/users/tenant/";

    public GetUsersForTenant(TestApplicationFactory factory)
        : base(factory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await ElectionMockData.Seed(RunScoped);
    }

    [Fact]
    public async Task ShouldReturnSensitiveUserDataWhenElectionAdminRequestsUsers()
    {
        var users = await GetSuccessfulResponse<IEnumerable<UserModel>>(() => ElectionAdminClient.GetAsync(Url + TenantMockData.FdpStGallen.Id));
        users.Should().AllSatisfy(u => u.Username.Should().NotBeNull());
        users.Should().AllSatisfy(u => u.Email.Should().NotBeNull());
        users.MatchSnapshot();
    }

    [Fact]
    public async Task ShouldHideSensitiveUserDataExceptForOwningUserWhenUserRequestsUsers()
    {
        var users = await GetSuccessfulResponse<IEnumerable<UserModel>>(() => UserClient.GetAsync(Url + TenantMockData.FdpStGallen.Id));

        users.Should().ContainSingle(u =>
            u.Id == UserMockData.TestUser.Id &&
            u.Username == UserMockData.TestUser.Username &&
            u.Email == UserMockData.TestUser.Email);

        users.Should().ContainSingle(u =>
            u.Id == UserMockData.FdpUser.Id &&
            string.IsNullOrEmpty(u.Username) &&
            string.IsNullOrEmpty(u.Email));

        users.MatchSnapshot();
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.Wahlverwalter;
        yield return Role.User;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.GetAsync(Url);
    }
}
