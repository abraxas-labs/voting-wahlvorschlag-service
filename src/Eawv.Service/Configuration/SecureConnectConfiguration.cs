// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Eawv.Service.Configuration;

public class SecureConnectConfiguration
{
    public SecureConnectConfiguration()
    {
        Scopes = [];
    }

    public string AppShortcut { get; set; }

    public string Audience { get; set; }

    public string Authority { get; set; }

    public string ParentLabelName { get; set; }

    public string RoleLabelKey { get; set; }

    public string ServiceAccount { get; set; }

    public string ServiceAccountPassword { get; set; }

    public List<string> Scopes { get; set; }

    public string TenantHeaderName { get; set; }

    public string AppHeaderName { get; set; }
}
