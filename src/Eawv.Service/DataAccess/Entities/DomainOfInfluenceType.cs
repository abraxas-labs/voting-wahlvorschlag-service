// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Eawv.Service.DataAccess.Entities;

/// <summary>
/// Represents the official [eCH-0155:domainOfInfluenceType].
/// </summary>
public enum DomainOfInfluenceType
{
    /// <summary>
    /// Bund.
    /// </summary>
    CH,

    /// <summary>
    /// Kanton.
    /// </summary>
    CT,

    /// <summary>
    /// Bezirk / Amt / Verwaltungskreis.
    /// </summary>
    BZ,

    /// <summary>
    /// Gemeinde.
    /// </summary>
    MU,

    /// <summary>
    /// Schulgemeinde.
    /// </summary>
    SC,

    /// <summary>
    /// Kirchengemeinde.
    /// </summary>
    KI,

    /// <summary>
    /// Ortsbürgergemeinde.
    /// </summary>
    OG,

    /// <summary>
    /// Korporationen.
    /// </summary>
    KO,

    /// <summary>
    /// Stadtkreis.
    /// </summary>
    SK,

    /// <summary>
    /// Andere.
    /// </summary>
    AN,
}
