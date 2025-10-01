// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.ComponentModel.DataAnnotations;
using Voting.Lib.RestValidation;

namespace Eawv.Service.Models;

public class ModifyBallotDocumentModel
{
    [Required]
    [MaxLength(100)]
    [ComplexSlText]
    public string Name { get; set; }

    [Required]
    [MaxLength(30_000_000)] // 30 MB
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1819:Properties should not return arrays",
        Justification = "Update models for entities require byte[]")]
    public byte[] Document { get; set; }
}
