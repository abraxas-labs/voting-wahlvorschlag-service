// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Eawv.Service.Models.TemplateServiceModels;

/// <summary>
/// Template type enumerations for the template service, to match the template bag with the view.
/// </summary>
public enum TemplateType
{
    /// <summary>
    /// General Layout.
    /// </summary>
    Layout = 0,

    /// <summary>
    /// For single lists.
    /// </summary>
    ListCandidates = 1,

    /// <summary>
    /// For multiple lists.
    /// </summary>
    ListsCandidates = 2,

    /// <summary>
    /// Specific export for the bundeskanzlei.
    /// </summary>
    ListsCandidatesFederalChancellery = 3,

    /// <summary>
    /// Template to list empty candidates.
    /// </summary>
    ListEmptyCandidates = 4,

    /// <summary>
    /// Template to list unions.
    /// </summary>
    EmptyListUnions = 5,

    /// <summary>
    /// Template for signatories.
    /// </summary>
    Signatories = 6,

    /// <summary>
    /// Email template for notification when list state changed.
    /// </summary>
    EmailListStateChanged = 7,

    /// <summary>
    /// Email template for notification when new comment was added.
    /// </summary>
    EmailListNewComment = 8,

    /// <summary>
    /// Template for ECH157.
    /// </summary>
    ECH157 = 9,

    /// <summary>
    /// Template for Wabsti candidates.
    /// </summary>
    WabstiCandidates = 10,
}
