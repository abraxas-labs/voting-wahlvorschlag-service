// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eawv.Service.DataAccess.Entities;

public static class EfPropertyBuilderExtensions
{
    public static PropertyBuilder<DateTime> HasUtcConversion(this PropertyBuilder<DateTime> builder)
    {
        return builder.HasConversion(d => d, d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
    }

    public static PropertyBuilder<DateTime?> HasUtcConversion(this PropertyBuilder<DateTime?> builder)
    {
        return builder.HasConversion(d => d, d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : (DateTime?)null);
    }

    public static PropertyBuilder<DateTime> HasDateType(this PropertyBuilder<DateTime> builder)
    {
        return builder.HasColumnType("date");
    }

    public static PropertyBuilder<DateTime?> HasDateType(this PropertyBuilder<DateTime?> builder)
    {
        return builder.HasColumnType("date");
    }
}
