// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Eawv.Service.DataAccess.Entities;

public class Candidate : BaseEntity
{
    public string FamilyName { get; set; }

    public string FirstName { get; set; }

    public string Title { get; set; }

    public string OccupationalTitle { get; set; }

    public DateTime DateOfBirth { get; set; }

    public SexType Sex { get; set; }

    public string Origin { get; set; }

    public string Street { get; set; }

    public string HouseNumber { get; set; }

    public string ZipCode { get; set; }

    public string Locality { get; set; }

    public string BallotFamilyName { get; set; }

    public string BallotFirstName { get; set; }

    public string BallotOccupationalTitle { get; set; }

    /// <summary>
    /// Gets or sets the political locality for the ballot details.
    /// </summary>
    public string BallotLocality { get; set; }

    public bool Incumbent { get; set; }

    public Guid ListId { get; set; }

    public bool Cloned { get; set; }

    /// <summary>
    /// Gets or Sets the candidate index/number inside a list. Index starts from 1 instead of 0.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Gets or Sets the OrderIndex for background sorting.
    /// </summary>
    public int OrderIndex { get; set; }

    /// <summary>
    /// Gets or Sets the ClonOrderIndex. This is needed for sorting the preCumulated Candidates. Index starts from 1 instead of 0.
    /// </summary>
    public int? CloneOrderIndex { get; set; }

    public string Party { get; set; }

    public List List { get; set; }

    public ICollection<MarkedElement> MarkedElements { get; set; }

    public static void Map(ModelBuilder modelBuilder)
    {
        Map<Candidate>(modelBuilder);

        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.HasOne(x => x.List)
                .WithMany(x => x.Candidates)
                .HasForeignKey(x => x.ListId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.Property(x => x.FamilyName)
                .IsRequired();
            entity.Property(x => x.FirstName)
                .IsRequired();
            entity.Property(x => x.Title)
                .IsRequired(false);
            entity.Property(x => x.OccupationalTitle)
                .IsRequired();
            entity.Property(x => x.DateOfBirth)
                .IsRequired()
                .HasUtcConversion();
            entity.Property(x => x.Sex)
                .IsRequired();
            entity.Property(x => x.Origin)
                .IsRequired(false);
            entity.Property(x => x.Street)
                .IsRequired();
            entity.Property(x => x.HouseNumber)
                .IsRequired(false);
            entity.Property(x => x.ZipCode)
                .IsRequired();
            entity.Property(x => x.Locality)
                .IsRequired();
            entity.Property(x => x.Incumbent)
                .IsRequired();
            entity.Property(x => x.BallotFamilyName)
                .IsRequired();
            entity.Property(x => x.BallotFirstName)
                .IsRequired();
            entity.Property(x => x.BallotOccupationalTitle)
                .IsRequired();
            entity.Property(x => x.BallotLocality)
                .IsRequired();
            entity.Property(x => x.Cloned)
                .IsRequired();
            entity.Property(x => x.Index)
                .IsRequired();
            entity.Property(x => x.Party)
                .IsRequired(false);
        });
    }
}
