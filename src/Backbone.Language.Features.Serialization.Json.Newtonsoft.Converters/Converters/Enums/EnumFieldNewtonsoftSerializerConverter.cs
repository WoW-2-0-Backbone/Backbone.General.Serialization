using Newtonsoft.Json;

namespace Backbone.Language.Features.Serialization.Json.Newtonsoft.Converters.Converters.Enums;

/// <summary>
/// Represents an enum converter by enum field for Newtonsoft JSON Serializer
/// </summary>
/// <typeparam name="T">The enum type to be converted.</typeparam>
public class EnumFieldNewtonsoftSerializerConverter<T> : JsonConverter where T : struct, Enum
{
    private readonly Dictionary<string, T> _fieldToEnum;

    /// <summary>
    /// Initializes field enum converter and calculates field enum values.
    /// </summary>
    public EnumFieldNewtonsoftSerializerConverter()
    {
        _fieldToEnum = Enum.GetValues<T>()
            .ToDictionary(
                e => e.ToString(),
                e => e,
                StringComparer.OrdinalIgnoreCase);
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is T enumValue)
        {
            writer.WriteValue(enumValue.ToString());
        }
        else
        {
            throw new JsonSerializationException("Expected enum object value.");
        }
    }

    /// <inheritdoc />
    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        var enumString = reader.Value?.ToString();
        if (enumString != null && _fieldToEnum.TryGetValue(enumString, out var result))
            return result;

        throw new JsonSerializationException($"Unknown enum field '{enumString}' for type '{typeof(T)}'.");
    }

    /// <inheritdoc />
    public override bool CanConvert(Type objectType)
    {
        return objectType.IsEnum;
    }
}