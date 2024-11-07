using Backbone.Language.Core.Extensions.Enums.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Backbone.Language.Features.Serialization.Json.Newtonsoft.Converters.Converters;

/// <summary>
/// Represents a Newtonsoft JSON converter for enums with description attribute value.
/// </summary>
public class DescriptionEnumConverter<T> : StringEnumConverter where T : struct, Enum
{
    private readonly Dictionary<string, T> _descriptionToEnum;
    private readonly Dictionary<T, string> _enumToDescription;

    /// <summary>
    /// Initializes a new instance of the <see cref="DescriptionEnumConverter{T}"/> class.
    /// </summary>
    public DescriptionEnumConverter()
    {
        _descriptionToEnum = Enum.GetValues(typeof(T))
            .Cast<T>()
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
            var description = _enumToDescription[enumValue];
            writer.WriteValue(description);
        }
        else
        {
            throw new JsonSerializationException("Expected enum object value.");
        }
    }

    /// <inheritdoc />
    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var enumString = reader.Value?.ToString();
        if (enumString != null && _descriptionToEnum.TryGetValue(enumString, out var enumValue))
        {
            return enumValue;
        }
        throw new JsonSerializationException($"Unknown enum description '{enumString}' for type '{typeof(T)}'.");
    }
}