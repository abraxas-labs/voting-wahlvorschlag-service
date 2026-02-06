// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.Integration.Tests.MockedData;
using Eawv.Service.Models;
using Voting.Lib.Testing.Utils;
using Xunit;

namespace Eawv.Service.Integration.Tests.UserTests;

public class UpdateUserTest : BaseRestTest
{
    private const string Url = "api/users/";

    public UpdateUserTest(TestApplicationFactory factory)
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
        var user = await GetSuccessfulResponse<UserModel>(
            () => ElectionAdminClient.PutAsJsonAsync(Url + UserMockData.FdpUser.Id, NewValidRequest()));
        user.MatchSnapshot();
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.PutAsJsonAsync(Url + UserMockData.FdpUser.Id, NewValidRequest());
    }

    private ModifyUserModel NewValidRequest(Action<ModifyUserModel> customizer = null)
    {
        var election = new ModifyUserModel
        {
            Firstname = "TesterUpdated",
            Lastname = "TesterUpdated",
            Tenants = [TenantMockData.StGallen.Id],
        };
        customizer?.Invoke(election);
        return election;
    }
}
