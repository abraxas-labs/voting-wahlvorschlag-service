// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Integration.Tests.MockedData;
using Eawv.Service.Models;
using Voting.Lib.Testing.Utils;
using Xunit;

namespace Eawv.Service.Integration.Tests.SettingTests;

public class UpdateSettingTest : BaseRestTest
{
    private const string Url = "api/settings";

    public UpdateSettingTest(TestApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task TestCreateShouldWork()
    {
        var setting = await GetSuccessfulResponse<SettingModel>(
            () => ElectionAdminClient.PatchAsJsonAsync(Url, NewValidRequest()));
        setting.MatchSnapshot();
    }

    [Fact]
    public async Task TestUpdateShouldWork()
    {
        await RunOnDb(async db =>
        {
            db.Settings.Add(new Setting
            {
                TenantId = TenantMockData.StGallen.Id,
                ShowBallotPaperInfos = true,
                WabstiExportTenantTitle = "test-title",
            });
            await db.SaveChangesAsync();
        });

        var setting = await GetSuccessfulResponse<SettingModel>(
            () => ElectionAdminClient.PatchAsJsonAsync(Url, NewValidRequest()));
        setting.MatchSnapshot();
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

    private ModifySettingModel NewValidRequest(Action<ModifySettingModel> customizer = null)
    {
        var request = new ModifySettingModel { WabstiExportTenantTitle = "new title" };
        customizer?.Invoke(request);
        return request;
    }
}
