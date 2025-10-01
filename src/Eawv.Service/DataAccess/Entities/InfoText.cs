// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore;

namespace Eawv.Service.DataAccess.Entities;

public class InfoText : BaseEntity
{
    public Election Election { get; set; }

    public string TenantId { get; set; }

    public string Key { get; set; }

    public string Value { get; set; }

    public bool Visible { get; set; }

    public Guid? ElectionId { get; set; }

    public static void Map(ModelBuilder modelBuilder)
    {
        Map<InfoText>(modelBuilder);
        modelBuilder.Entity<InfoText>(entity =>
        {
            entity.HasOne(d => d.Election)
                .WithMany(p => p.InfoTexts)
                .HasForeignKey(d => d.ElectionId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            entity.Property(e => e.TenantId)
                .IsRequired();
            entity.Property(e => e.Key)
                .IsRequired();
            entity.Property(e => e.Value)
                .IsRequired();
            entity.Property(e => e.Visible)
                .IsRequired();
        });
    }
}
