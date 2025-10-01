// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Eawv.Service.Models.TemplateServiceModels;

namespace Eawv.Service.Models;

public enum ElectionExportTemplateType
{
    /// <summary>
    /// Template to list empty candidates.
    /// </summary>
    EmptyCandidates = TemplateType.ListEmptyCandidates,

    /// <summary>
    /// Template to list empty unions.
    /// </summary>
    EmptyListUnions = TemplateType.EmptyListUnions,

    /// <summary>
    /// Template for multiple lists.
    /// </summary>
    Candidates = TemplateType.ListsCandidates,

    /// <summary>
    /// Template for the federal chancellery.
    /// </summary>
    FederalChancellery = TemplateType.ListsCandidatesFederalChancellery,

    /// <summary>
    /// Template for empty signatories.
    /// </summary>
    EmptySignatories = TemplateType.Signatories,

    /// <summary>
    /// Template for ECH157.
    /// </summary>
    ECH157 = TemplateType.ECH157,
}
