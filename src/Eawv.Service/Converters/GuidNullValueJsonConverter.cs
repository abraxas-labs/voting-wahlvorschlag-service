// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Eawv.Service.Converters;

/// <summary>
/// Converts the read string value to a nullable Guid, also if the read string value ist <see cref="string.Empty"/>.
/// This is a workaround for https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-migrate-from-newtonsoft-how-to?pivots=dotnet-5-0#deserialize-null-to-non-nullable-type.
/// </summary>
public class GuidNullValueJsonConverter : JsonConverter<Guid?>
{
    public override Guid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    => string.IsNullOrEmpty(reader.GetString()) ? null : Guid.Parse(reader.GetString());

    public override void Write(Utf8JsonWriter writer, Guid? value, JsonSerializerOptions options)
    => writer.WriteStringValue(value ?? Guid.Empty);
}
