// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore;

namespace Eawv.Service.DataAccess.Entities;

public class MarkedElement : BaseEntity
{
    public Guid CandidateId { get; set; }

    public string Field { get; set; }

    public Candidate Candidate { get; set; }

    public static void Map(ModelBuilder modelBuilder)
    {
        Map<MarkedElement>(modelBuilder);

        modelBuilder.Entity<MarkedElement>(entity =>
        {
            entity.Property(e => e.Field)
                .IsRequired();

            entity.HasOne(x => x.Candidate)
                .WithMany(x => x.MarkedElements)
                .HasForeignKey(x => x.CandidateId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasIndex(e => new { e.CandidateId, e.Field })
                .IsUnique();
        });
    }
}
