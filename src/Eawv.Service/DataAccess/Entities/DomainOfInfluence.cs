// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Eawv.Service.DataAccess.Entities;

public class DomainOfInfluence : BaseEntity
{
    public string TenantId { get; set; }

    public string OfficialId { get; set; }

    public string Name { get; set; }

    public string ShortName { get; set; }

    public DomainOfInfluenceType DomainOfInfluenceType { get; set; }

    public ICollection<DomainOfInfluenceElection> ParticipatingElections { get; set; }

    public static void Map(ModelBuilder modelBuilder)
    {
        Map<DomainOfInfluence>(modelBuilder);
        modelBuilder.Entity<DomainOfInfluence>(entity =>
        {
            entity.Property(x => x.TenantId)
                .IsRequired();
            entity.Property(x => x.OfficialId)
                .IsRequired();
            entity.Property(x => x.Name)
                .IsRequired();
            entity.Property(x => x.ShortName)
                .IsRequired();
            entity.Property(x => x.DomainOfInfluenceType)
                .IsRequired();
        });
    }
}
