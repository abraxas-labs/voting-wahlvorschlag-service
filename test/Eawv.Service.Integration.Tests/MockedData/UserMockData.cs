// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Eawv.Service.Authentication;
using Voting.Lib.Iam.Testing.AuthenticationScheme;

namespace Eawv.Service.Integration.Tests.MockedData;

public static class UserMockData
{
    public static MockedUser TestUser { get; } = new()
    {
        Id = SecureConnectTestDefaults.MockedUserDefault.Loginid,
        Name = "Test-User",
        Username = "testuser",
        Email = "test@user.invalid",
        RoleByTenantId = new Dictionary<string, string>
        {
            [TenantMockData.StGallen.Id] = Role.Wahlverwalter,
            [TenantMockData.FdpStGallen.Id] = Role.User,
            [TenantMockData.SpStGallen.Id] = Role.User,
        },
    };

    public static MockedUser StGallenUser { get; } = new()
    {
        Id = "sg-wahlverwahlter",
        Name = "SG Wahlverwalter",
        Username = "sgwahlverwalter",
        Email = "sg@wahlverwalter.invalid",
        RoleByTenantId = new Dictionary<string, string>
        {
            [TenantMockData.StGallen.Id] = Role.Wahlverwalter,
        },
    };

    public static MockedUser FdpUser { get; } = new()
    {
        Id = "fdp-user",
        Name = "FDP user",
        Username = "fdpuser",
        Email = "fdp@user.invalid",
        RoleByTenantId = new Dictionary<string, string>
        {
            [TenantMockData.FdpStGallen.Id] = Role.User,
        },
    };

    public static MockedUser SpUser { get; } = new()
    {
        Id = "sp-user",
        Name = "SP user",
        Username = "spuser",
        RoleByTenantId = new Dictionary<string, string>
        {
            [TenantMockData.SpStGallen.Id] = Role.User,
        },
    };

    public static MockedUser GossauUser { get; } = new()
    {
        Id = "gossau-wahlverwahlter",
        Name = "Gossau Wahlverwalter",
        Username = "gossauwahlverwalter",
        Email = "gossau@wahlverwalter.invalid",
        RoleByTenantId = new Dictionary<string, string>
        {
            [TenantMockData.Gossau.Id] = Role.Wahlverwalter,
        },
    };

    public static MockedUser GossauPartyUser { get; } = new()
    {
        Id = "gossau-user",
        Name = "Gossau Party User",
        Username = "gossaupartyuser",
        Email = "gossau@party.invalid",
        RoleByTenantId = new Dictionary<string, string>
        {
            [TenantMockData.GossauParty.Id] = Role.User,
        },
    };

    public static IEnumerable<MockedUser> All
    {
        get
        {
            yield return TestUser;
            yield return StGallenUser;
            yield return FdpUser;
            yield return SpUser;
            yield return GossauUser;
            yield return GossauPartyUser;
        }
    }
}
