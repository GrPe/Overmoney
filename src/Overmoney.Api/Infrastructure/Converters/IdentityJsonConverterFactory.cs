using Overmoney.Api.Features.Common.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Overmoney.Api.Infrastructure.Converters;

public sealed class IntIdentityJsonConverter : JsonConverter<Identity<int>>
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (typeToConvert.BaseType is null)
        {
            return false;
        }

        var type = typeToConvert.BaseType;

        return type == typeof(Identity<int>);
    }

    public override Identity<int>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if(reader.TryGetInt32(out int result))
        {
            return Activator.CreateInstance(typeToConvert, result) as Identity<int>;
        }

        var value = reader.GetString();

        if (int.TryParse(value, out result))
        {
            return Activator.CreateInstance(typeToConvert, result) as Identity<int>;
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, Identity<int> value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Id.ToString());
    }
}