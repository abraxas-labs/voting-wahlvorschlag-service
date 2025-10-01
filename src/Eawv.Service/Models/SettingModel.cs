// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Eawv.Service.Models;

public class SettingModel
{
    public bool ShowBallotPaperInfos { get; set; }

    public bool ShowPartyOnProporzElection { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1819:Properties should not return arrays",
        Justification = "Update models for entities require byte[]")]
    public byte[] TenantLogo { get; set; }

    public string WabstiExportTenantTitle { get; set; }
}
