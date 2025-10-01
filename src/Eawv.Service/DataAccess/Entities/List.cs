// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Eawv.Service.DataAccess.Entities;

public class List : BaseEntity
{
    public Election Election { get; set; }

    public string ResponsiblePartyTenantId { get; set; }

    public string Indenture { get; set; }

    public DateTime? SubmitDate { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int SortOrder { get; set; }

    public ListState State { get; set; }

    public Guid ElectionId { get; set; }

    public string Representative { get; set; }

    public List<string> DeputyUsers { get; set; }

    public List<string> MemberUsers { get; set; }

    public bool Locked { get; set; }

    public bool Validated { get; set; }

    public int Version { get; set; }

    public ICollection<Candidate> Candidates { get; set; }

    public ICollection<ListComment> Comments { get; set; }

    public ListUnion RootListUnion { get; set; }

    public ListUnion ListUnion { get; set; }

    public Guid? ListUnionId { get; set; }

    public ListUnion ListSubUnion { get; set; }

    public Guid? ListSubUnionId { get; set; }

    public static void Map(ModelBuilder modelBuilder)
    {
        Map<List>(modelBuilder);
        modelBuilder.Entity<List>(entity =>
        {
            entity.HasOne(d => d.Election)
                .WithMany(p => p.Lists)
                .HasForeignKey(d => d.ElectionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.ListUnion)
                .WithMany(p => p.UnionLists)
                .HasForeignKey(d => d.ListUnionId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.ListSubUnion)
                .WithMany(p => p.SubUnionLists)
                .HasForeignKey(d => d.ListSubUnionId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.Property(e => e.ResponsiblePartyTenantId)
                .IsRequired();
            entity.Property(e => e.Representative)
                .IsRequired();
            entity.Property(e => e.Indenture)
                .IsRequired(false);
            entity.Property(e => e.SubmitDate)
                .IsRequired(false)
                .HasUtcConversion();
            entity.Property(e => e.Name)
                .IsRequired();
            entity.Property(e => e.Description)
                .IsRequired(false);
            entity.Property(e => e.SortOrder)
                .IsRequired();
            entity.Property(e => e.State)
                .IsRequired();

            entity.HasIndex(e => new { e.ElectionId, e.Indenture })
                .IsUnique();
        });
    }
}
