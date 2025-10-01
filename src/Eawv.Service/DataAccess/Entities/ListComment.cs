// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore;

namespace Eawv.Service.DataAccess.Entities;

public class ListComment : BaseEntity
{
    public string Content { get; set; }

    public Guid ListId { get; set; }

    public List List { get; set; }

    public static void Map(ModelBuilder modelBuilder)
    {
        Map<ListComment>(modelBuilder);

        modelBuilder.Entity<ListComment>(entity =>
        {
            entity.HasOne(x => x.List)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.ListId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.Property(x => x.Content)
                .IsRequired();
        });
    }
}
