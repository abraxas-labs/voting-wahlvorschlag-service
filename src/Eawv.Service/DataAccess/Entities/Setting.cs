// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.EntityFrameworkCore;

namespace Eawv.Service.DataAccess.Entities;

public class Setting : BaseEntity
{
    public string TenantId { get; set; }

    public bool ShowBallotPaperInfos { get; set; }

    public bool ShowPartyOnProporzElection { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1819:Properties should not return arrays",
        Justification = "EF Core property that maps a byte array from the database must be of type byte[].")]
    public byte[] TenantLogo { get; set; }

    public string WabstiExportTenantTitle { get; set; }

    public static void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.TenantId)
                .IsRequired();

            entity.HasIndex(e => e.TenantId)
                .IsUnique();
        });
    }
}
