// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Models;
using Voting.Lib.Testing.Utils;
using Xunit;

namespace Eawv.Service.Integration.Tests.UserTests;

public class CreateUserTest : BaseRestTest
{
    private const string Url = "api/users";

    public CreateUserTest(TestApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task TestAsElectionAdmin()
    {
        var user = await GetSuccessfulResponse<CreateUserModel>(() => ElectionAdminClient.PostAsJsonAsync(Url, NewValidRequest()));
        user.MatchSnapshot();
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.PostAsJsonAsync(Url, NewValidRequest());
    }

    private CreateUserModel NewValidRequest(Action<CreateUserModel> customizer = null)
    {
        var election = new CreateUserModel
        {
            Firstname = "Tester",
            Lastname = "Tester",
            Username = "Tester",
            Email = "tester@user.invalid",
            Tenants = [],
        };
        customizer?.Invoke(election);
        return election;
    }
}
