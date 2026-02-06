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

public class GetUsersTest : BaseRestTest
{
    private const string Url = "api/users/";

    public GetUsersTest(TestApplicationFactory factory)
        : base(factory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await ElectionMockData.Seed(RunScoped);
    }

    [Fact]
    public async Task ShouldReturnAllUsersWithSensitiveDataWhenElectionAdminRequestsUsers()
    {
        var users = await GetSuccessfulResponse<IEnumerable<UserModel>>(() => ElectionAdminClient.GetAsync(Url));
        users.Should().AllSatisfy(u => u.Username.Should().NotBeNull());
        users.MatchSnapshot();
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.GetAsync(Url);
    }
}
