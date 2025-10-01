// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Eawv.Service.Models.PdfServiceModels;
using Eawv.Service.Models.TemplateServiceModels;
using Microsoft.EntityFrameworkCore;

namespace Eawv.Service.DataAccess.Entities;

public class TemplateEntity
{
    public Guid Id { get; set; }

    public TemplateType Type { get; set; }

    /// <summary>
    /// Gets or sets the template key. Should only be used for template type layout.
    /// </summary>
    public string Key { get; set; }

    public string TenantId { get; set; }

    /// <summary>
    /// Gets or sets the Razor File Name, i.e. 'ListCandidates_Default.cshtml'.
    /// </summary>
    public string TemplateName { get; set; }

    public string Filename { get; set; }

    public PdfFormat Format { get; set; }

    public bool Landscape { get; set; }

    public string TemplateKey => $"{Type}${Key}-{TenantId}";

    public static void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TemplateEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Ignore(e => e.TemplateKey);

            entity.HasIndex(e => new { e.Type, e.TenantId, e.Key })
                .IsUnique();
        });
    }
}
