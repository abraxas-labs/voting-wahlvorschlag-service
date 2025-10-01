// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore;

namespace Eawv.Service.DataAccess.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string ModifiedBy { get; set; }

    public bool IsNew => Id == Guid.Empty;

    public static void Map<TEntity>(ModelBuilder modelBuilder)
        where TEntity : BaseEntity
    {
        modelBuilder.Entity<TEntity>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(x => x.CreationDate)
                .IsRequired()
                .HasUtcConversion();
            entity.Property(x => x.CreatedBy)
                .IsRequired();
            entity.Property(x => x.ModifiedDate)
                .IsRequired(false)
                .HasUtcConversion();
            entity.Property(x => x.ModifiedBy)
                .IsRequired(false);
        });
    }
}
