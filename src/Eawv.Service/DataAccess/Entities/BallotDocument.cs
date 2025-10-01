// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore;

namespace Eawv.Service.DataAccess.Entities;

public class BallotDocument : BaseEntity
{
    public Election Election { get; set; }

    public string Name { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1819:Properties should not return arrays",
        Justification = "EF Core property that maps a byte array from the database must be of type byte[].")]
    public byte[] Document { get; set; }

    public Guid ElectionId { get; set; }

    public static void Map(ModelBuilder modelBuilder)
    {
        Map<BallotDocument>(modelBuilder);

        modelBuilder.Entity<BallotDocument>(entity =>
        {
            entity.HasOne(x => x.Election)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.ElectionId);

            entity.Property(x => x.Name)
                .IsRequired();
            entity.Property(x => x.Document)
                .IsRequired();
        });
    }
}
