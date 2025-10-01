// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Eawv.Service.DataAccess.Entities;

/// <summary>
/// Represents the list state.
/// </summary>
public enum ListState
{
    /// <summary>
    /// Draft.
    /// </summary>
    Draft,

    /// <summary>
    /// Submitted.
    /// </summary>
    Submitted,

    /// <summary>
    /// Formally submitted.
    /// </summary>
    FormallySubmitted,

    /// <summary>
    /// Valid.
    /// </summary>
    Valid,

    /// <summary>
    /// Archived.
    /// </summary>
    Archived,
}
