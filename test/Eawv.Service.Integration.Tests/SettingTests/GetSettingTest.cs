// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Eawv.Service.Authentication;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Integration.Tests.MockedData;
using Eawv.Service.Models;
using FluentAssertions;
using Voting.Lib.Testing.Utils;
using Xunit;

namespace Eawv.Service.Integration.Tests.SettingTests;

public class GetSettingTest : BaseRestTest
{
    private const string Url = "api/settings";

    public GetSettingTest(TestApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task TestNotExistingShouldWork()
    {
        var setting = await GetSuccessfulResponse<SettingModel>(() => ElectionAdminClient.GetAsync(Url));
        setting.MatchSnapshot();
    }

    [Fact]
    public async Task TestExistingWithElectionAdminShouldWork()
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

        var setting = await GetSuccessfulResponse<SettingModel>(() => ElectionAdminClient.GetAsync(Url));
        setting.MatchSnapshot();
    }

    [Fact]
    public async Task TestExistingWithUserShouldWork()
    {
        await RunOnDb(async db =>
        {
            db.Settings.Add(new Setting
            {
                TenantId = TenantMockData.StGallen.Id,
                ShowBallotPaperInfos = true,
                WabstiExportTenantTitle = "test-title2",
            });
            await db.SaveChangesAsync();
        });

        var setting = await GetSuccessfulResponse<SettingModel>(() => ElectionAdminClient.GetAsync(Url));
        Assert.True(setting.ShowBallotPaperInfos);
        setting.WabstiExportTenantTitle.Should().Be("test-title2");
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
