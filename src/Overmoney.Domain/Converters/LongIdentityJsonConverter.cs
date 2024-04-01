using Overmoney.Domain.Features.Common.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Overmoney.Domain.Converters;
internal class LongIdentityJsonConverter : JsonConverter<Identity<long>>
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (typeToConvert.BaseType is null)
        {
            return false;
        }

        var type = typeToConvert.BaseType;

        return type == typeof(Identity<long>);
    }


    public override Identity<long>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TryGetInt64(out long result))
        {
            return Activator.CreateInstance(typeToConvert, result) as Identity<long>;
        }

        var value = reader.GetString();

        if (long.TryParse(value, out result))
        {
            return Activator.CreateInstance(typeToConvert, result) as Identity<long>;
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, Identity<long> value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}
