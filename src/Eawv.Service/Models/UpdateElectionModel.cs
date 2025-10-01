// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Eawv.Service.DataAccess.Entities;
using Voting.Lib.RestValidation;

namespace Eawv.Service.Models;

public class UpdateElectionModel : IValidatableObject
{
    [Required]
    [Length(1, 255)]
    [ComplexMlText]
    public string Name { get; set; }

    [MaxLength(2000)]
    [ComplexMlText]
    public string Description { get; set; }

    [Required]
    public DateTime SubmissionDeadlineBegin { get; set; }

    [Required]
    public DateTime SubmissionDeadlineEnd { get; set; }

    [Required]
    public DateTime ContestDate { get; set; }

    public DateTime? AvailableFrom { get; set; }

    [Required]
    [ValidEnum]
    public ElectionType ElectionType { get; set; }

    public int? QuorumSignaturesCount { get; set; }

    public int State { get; set; }

    [MaxLength(30_000_000)] // 30 MB
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1819:Properties should not return arrays",
        Justification = "Update models for entities require byte[]")]
    public byte[] TenantLogo { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SubmissionDeadlineBegin > SubmissionDeadlineEnd)
        {
            yield return new ValidationResult(
                "Submission begin must be before submission end.",
                new[] { nameof(SubmissionDeadlineBegin), nameof(SubmissionDeadlineEnd) });
        }

        if (ContestDate < SubmissionDeadlineEnd)
        {
            yield return new ValidationResult(
                "Contest date must be after submission end.",
                new[] { nameof(ContestDate), nameof(SubmissionDeadlineEnd) });
        }

        if (QuorumSignaturesCount != null && QuorumSignaturesCount < 0)
        {
            yield return new ValidationResult(
                "Quorum signature count should be empty or positive number.",
                new[] { nameof(QuorumSignaturesCount) });
        }
    }
}
