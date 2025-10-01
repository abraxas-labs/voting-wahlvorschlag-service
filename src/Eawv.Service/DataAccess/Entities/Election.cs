// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Voting.Lib.Common;

namespace Eawv.Service.DataAccess.Entities;

public class Election : BaseEntity
{
    private const int NumberOfDaysToArchiveAfterDeadlineEnd = 14;

    public string TenantId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime SubmissionDeadlineBegin { get; set; }

    public DateTime SubmissionDeadlineEnd { get; set; }

    public DateTime ContestDate { get; set; }

    public DateTime? AvailableFrom { get; set; }

    public ElectionType ElectionType { get; set; }

    public int? QuorumSignaturesCount { get; set; }

    public int State { get; set; } // TODO enum values?

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1819:Properties should not return arrays",
        Justification = "EF Core property that maps a byte array from the database must be of type byte[].")]
    public byte[] TenantLogo { get; set; }

    public ICollection<BallotDocument> Documents { get; set; }

    public ICollection<DomainOfInfluenceElection> DomainsOfInfluence { get; set; }

    public ICollection<InfoText> InfoTexts { get; set; }

    public ICollection<List> Lists { get; set; }

    public static void Map(ModelBuilder modelBuilder)
    {
        Map<Election>(modelBuilder);
        modelBuilder.Entity<Election>(entity =>
        {
            entity.Property(x => x.TenantId)
                .IsRequired();
            entity.Property(x => x.Name)
                .IsRequired();
            entity.Property(x => x.Description)
                .IsRequired(false);
            entity.Property(x => x.SubmissionDeadlineBegin)
                .IsRequired()
                .HasUtcConversion()
                .HasDateType();
            entity.Property(x => x.SubmissionDeadlineEnd)
                .IsRequired()
                .HasUtcConversion()
                .HasDateType();
            entity.Property(x => x.ContestDate)
                .IsRequired()
                .HasUtcConversion()
                .HasDateType();
            entity.Property(x => x.AvailableFrom)
                .IsRequired(false)
                .HasUtcConversion()
                .HasDateType();
            entity.Property(x => x.ElectionType)
                .IsRequired();
            entity.Property(x => x.QuorumSignaturesCount)
                .IsRequired(false);
            entity.Property(x => x.State)
                .IsRequired();
        });
    }

    public bool IsArchived(IClock clock)
    {
        return SubmissionDeadlineEnd.AddDays(NumberOfDaysToArchiveAfterDeadlineEnd) < clock.UtcNow;
    }
}
