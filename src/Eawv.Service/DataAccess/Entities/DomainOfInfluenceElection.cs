// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore;

namespace Eawv.Service.DataAccess.Entities;

public class DomainOfInfluenceElection : BaseEntity
{
    public Election Election { get; set; }

    public DomainOfInfluence DomainOfInfluence { get; set; }

    public int NumberOfMandates { get; set; }

    public Guid ElectionId { get; set; }

    public Guid DomainOfInfluenceId { get; set; }

    public static void Map(ModelBuilder modelBuilder)
    {
        Map<DomainOfInfluenceElection>(modelBuilder);

        modelBuilder.Entity<DomainOfInfluenceElection>(entity =>
        {
            entity.HasAlternateKey(e => new { e.ElectionId, e.DomainOfInfluenceId });

            entity.HasOne(d => d.Election)
                .WithMany(p => p.DomainsOfInfluence)
                .HasForeignKey(d => d.ElectionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.DomainOfInfluence)
                .WithMany(p => p.ParticipatingElections)
                .HasForeignKey(d => d.DomainOfInfluenceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(x => x.NumberOfMandates)
                .IsRequired();
        });
    }
}
