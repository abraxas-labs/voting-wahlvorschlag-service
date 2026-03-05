// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

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

namespace Eawv.Service.Integration.Tests.InfoTextTests;

public class CreateOrUpdateInfoTextTest : BaseRestTest
{
    private const string Url = "api/infotexts";

    public CreateOrUpdateInfoTextTest(TestApplicationFactory factory)
        : base(factory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await ElectionMockData.Seed(RunScoped);
        await InfoTextMockData.Seed(RunScoped);
    }

    [Fact]
    public async Task TestAsElectionAdmin()
    {
        var infoText = await GetSuccessfulResponse<InfoTextModel>(() => ElectionAdminClient.PostAsJsonAsync(Url, NewValidRequest()));
        infoText.MatchSnapshot(x => x.Id);
    }

    [Fact]
    public async Task TestWithElectionAsElectionAdmin()
    {
        var infoText = await GetSuccessfulResponse<InfoTextModel>(() => ElectionAdminClient.PostAsJsonAsync(Url, NewValidRequestWithElection()));
        infoText.MatchSnapshot(x => x.Id);
    }

    /// <summary>
    /// Ensures that creating an info text for an archived election returns BadRequest.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task TestWithArchivedElectionShouldThrow()
    {
        await AssertStatus(
            () => ElectionAdminClient.PostAsJsonAsync(Url, NewValidRequestWithArchivedElection()),
            HttpStatusCode.BadRequest);
    }

    protected override IEnumerable<string> AuthorizedRoles()
    {
        yield return Role.Wahlverwalter;
    }

    protected override Task<HttpResponseMessage> AuthorizationTestCall(HttpClient httpClient)
    {
        return httpClient.PostAsJsonAsync(Url, NewValidRequest());
    }

    private ModifyInfoTextModel NewValidRequest()
    {
        return new ModifyInfoTextModel
        {
            Key = "new-key",
            TenantId = TenantMockData.StGallen.Id,
            Value = "new value",
            Visible = true,
        };
    }

    private ModifyInfoTextModel NewValidRequestWithElection()
    {
        return new ModifyInfoTextModel
        {
            ElectionId = ElectionMockData.ProporzElection.Id,
            TenantId = TenantMockData.StGallen.Id,
            Key = "new-election-key",
            Value = "new election value",
            Visible = true,
        };
    }

    private ModifyInfoTextModel NewValidRequestWithArchivedElection()
    {
        return new ModifyInfoTextModel
        {
            ElectionId = ElectionMockData.ArchivedElection.Id,
            TenantId = TenantMockData.StGallen.Id,
            Key = "archived-key",
            Value = "archived value",
            Visible = true,
        };
    }
}
