// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Eawv.Service.Models.TemplateServiceModels;

namespace Eawv.Service.Models;

public enum ListExportTemplateType
{
    /// <summary>
    /// For single lists.
    /// </summary>
    Candidates = TemplateType.ListCandidates,

    /// <summary>
    /// For signatories.
    /// </summary>
    Signatories = TemplateType.Signatories,

    /// <summary>
    /// For Wabsti candidates.
    /// </summary>
    WabstiCandidates = TemplateType.WabstiCandidates,
}
