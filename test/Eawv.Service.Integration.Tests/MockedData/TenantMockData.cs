// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.Iam.Testing.AuthenticationScheme;

namespace Eawv.Service.Integration.Tests.MockedData;

public static class TenantMockData
{
    public static MockedTenant StGallen { get; } = new()
    {
        Id = SecureConnectTestDefaults.MockedTenantDefault.Id,
        Name = "St. Gallen",
    };

    public static MockedTenant FdpStGallen { get; } = new()
    {
        Id = "fdp-st-gallen",
        Name = "FDP St. Gallen",
        ParentId = StGallen.Id,
    };

    public static MockedTenant SpStGallen { get; } = new()
    {
        Id = "sp-st-gallen",
        Name = "SP St. Gallen",
        ParentId = StGallen.Id,
    };

    public static MockedTenant Gossau { get; } = new()
    {
        Id = "gossau",
        Name = "Gossau",
    };

    public static MockedTenant GossauParty { get; } = new()
    {
        Id = "gossau-party",
        Name = "Gossau Party",
    };

    public static IEnumerable<MockedTenant> All
    {
        get
        {
            yield return StGallen;
            yield return FdpStGallen;
            yield return SpStGallen;
            yield return Gossau;
            yield return GossauParty;
        }
    }
}
