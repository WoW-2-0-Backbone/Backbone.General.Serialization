using Backbone.Language.Core.Extensions.Enums.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Backbone.Language.Features.Serialization.Json.Newtonsoft.Converters.Converters.Enums;

/// <summary>
/// Represents a Newtonsoft JSON converter for enums with description attribute value.
/// </summary>
/// <typeparam name="T">The enum type to be converted.</typeparam>
public class EnumDescriptionOrFieldNewtonsoftSerializerConverter<T> : StringEnumConverter where T : struct, Enum
{
    private readonly Dictionary<string, T> _descriptionToEnum;
    private readonly Dictionary<T, string> _enumToDescription;
    private readonly Dictionary<string, T> _fieldToEnum;

    /// <summary>
    /// Initializes a new instance of the <see cref="DescriptionEnumConverter{T}" /> class.
    /// </summary>
    public EnumDescriptionOrFieldNewtonsoftSerializerConverter()
    {
        _descriptionToEnum = Enum.GetValues<T>()
            .ToDictionary(
                e => e.GetDescription(),
                e => e,
                StringComparer.OrdinalIgnoreCase);

        _enumToDescription = _descriptionToEnum.ToDictionary(
            kvp => kvp.Value,
            kvp => kvp.Key);

        _fieldToEnum = Enum.GetValues(typeof(T))
            .Cast<T>()
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
        if (enumString != null && _descriptionToEnum.TryGetValue(enumString, out var descriptionMatchedEnumValue)) return descriptionMatchedEnumValue;

        if (enumString != null && _fieldToEnum.TryGetValue(enumString, out var fieldMatchedEnumValue)) return fieldMatchedEnumValue;
        throw new JsonSerializationException($"Unknown enum description '{enumString}' for type '{typeof(T)}'.");
    }
}