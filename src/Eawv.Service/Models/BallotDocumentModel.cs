// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Eawv.Service.Models;

public class BallotDocumentModel : BaseEntityModel
{
    public string Name { get; set; }

    public Guid ElectionId { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1819:Properties should not return arrays",
        Justification = "EF Core property that maps a byte array from the database must be of type byte[].")]
    public byte[] Document { get; set; }
}
