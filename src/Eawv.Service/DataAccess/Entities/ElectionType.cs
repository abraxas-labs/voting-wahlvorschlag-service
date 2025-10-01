// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Eawv.Service.DataAccess.Entities;

/// <summary>
/// Represents the official [eCH-0155:typeOfElectionType]. Exchange format is a number.
/// </summary>
public enum ElectionType
{
    /// <summary>
    /// 1 = Proporz election.
    /// </summary>
    Proporz,

    /// <summary>
    /// 2 = Majorz election.
    /// </summary>
    Majorz,
}
