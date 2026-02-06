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

public class GetUserTest : BaseRestTest
{
    private const string Url = "api/users/";

    public GetUserTest(TestApplicationFactory factory)
        : base(factory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await ElectionMockData.Seed(RunScoped);
    }

    [Fact]
    public async Task ShouldReturnUserWithAllFieldsWhenElectionAdminRequestsUser()
    {
        var user = await GetSuccessfulResponse<UserModel>(() => ElectionAdminClient.GetAsync(Url + UserMockData.FdpUser.Id));
        user.Email.Should().Be(UserMockData.FdpUser.Email);
        user.Username.Should().Be(UserMockData.FdpUser.Username);
        user.MatchSnapshot();
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
