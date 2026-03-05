// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
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

public class CreateOrUpdateInfoTextBatchTest : BaseRestTest
{
    private const string Url = "api/infotexts/batch";

    public CreateOrUpdateInfoTextBatchTest(TestApplicationFactory factory)
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
        var infoTexts = await GetSuccessfulResponse<List<InfoTextModel>>(() => ElectionAdminClient.PostAsJsonAsync(Url, NewValidRequest()));
        infoTexts.OrderBy(i => i.Key).MatchSnapshot(x => x.Id);
    }

    [Fact]
    public async Task TestWithElectionAsElectionAdmin()
    {
        var infoTexts = await GetSuccessfulResponse<List<InfoTextModel>>(() => ElectionAdminClient.PostAsJsonAsync(Url, NewValidRequestWithElection()));
        infoTexts.OrderBy(i => i.Key).MatchSnapshot(x => x.Id);
    }

    /// <summary>
    /// Ensures that batch creating info texts for an archived election returns BadRequest.
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

    private List<ModifyInfoTextModel> NewValidRequest()
    {
        return
        [
            new ModifyInfoTextModel
            {
                Key = "batch-key-1",
                TenantId = TenantMockData.StGallen.Id,
                Value = "batch value 1",
                Visible = true,
            },
            new ModifyInfoTextModel
            {
                Key = "batch-key-2",
                TenantId = TenantMockData.StGallen.Id,
                Value = "batch value 2",
                Visible = true,
            },
        ];
    }

    private List<ModifyInfoTextModel> NewValidRequestWithElection()
    {
        return
        [
            new ModifyInfoTextModel
            {
                ElectionId = ElectionMockData.ProporzElection.Id,
                TenantId = TenantMockData.StGallen.Id,
                Key = "batch-election-key-1",
                Value = "batch election value 1",
                Visible = true,
            },
            new ModifyInfoTextModel
            {
                ElectionId = ElectionMockData.ProporzElection.Id,
                TenantId = TenantMockData.StGallen.Id,
                Key = "batch-election-key-2",
                Value = "batch election value 2",
                Visible = true,
            },
        ];
    }

    private List<ModifyInfoTextModel> NewValidRequestWithArchivedElection()
    {
        return
        [
            new ModifyInfoTextModel
            {
                ElectionId = ElectionMockData.ArchivedElection.Id,
                TenantId = TenantMockData.StGallen.Id,
                Key = "archived-batch-key-1",
                Value = "archived batch value 1",
                Visible = true,
            },
            new ModifyInfoTextModel
            {
                ElectionId = ElectionMockData.ArchivedElection.Id,
                TenantId = TenantMockData.StGallen.Id,
                Key = "archived-batch-key-2",
                Value = "archived batch value 2",
                Visible = true,
            },
        ];
    }
}
