using Backbone.Language.Core.Extensions.Enums.Extensions;
using Newtonsoft.Json;

namespace Backbone.Language.Features.Serialization.Json.Newtonsoft.Converters.Converters.Enums;

/// <summary>
/// Represents an enum converter by enum field for Newtonsoft JSON Serializer
/// </summary>
/// <typeparam name="T">The enum type to be converted.</typeparam>
public class EnumDescriptionNewtonsoftSerializerConverter<T> : JsonConverter where T : struct, Enum
{
    private readonly Dictionary<string, T> _descriptionToEnum;
    private readonly Dictionary<T, string> _enumToDescription;

    /// <summary>
    /// Initializes field enum converter and enum to description and vice versa mappings
    /// </summary>
    public EnumDescriptionNewtonsoftSerializerConverter()
    {
        _descriptionToEnum = Enum.GetValues<T>()
            .ToDictionary(
                e => e.GetDescription(),
                e => e,
                StringComparer.OrdinalIgnoreCase);

        _enumToDescription = _descriptionToEnum.ToDictionary(
            kvp => kvp.Value,
            kvp => kvp.Key);
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is T enumValue)
        {
            writer.WriteValue(_enumToDescription[enumValue]);
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
        if (enumString != null && _descriptionToEnum.TryGetValue(enumString, out var result))
            return result;

        throw new JsonSerializationException($"Unknown enum description '{enumString}' for type '{typeof(T)}'.");
    }

    /// <inheritdoc />
    public override bool CanConvert(Type objectType)
    {
        return objectType.IsEnum;
    }
}