using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Backbone.Language.Features.Serialization.Json.Abstractions.Constants;

namespace Backbone.Language.Features.Serialization.Json.System.Brokers;

/// <summary>
/// Provides JSON serialization options for System.Text.Json serialization provider to customize serialization.
/// </summary>
public class SystemJsonSerializationOptionsProvider : ISystemJsonSerializationOptionsProvider
{
    private readonly Dictionary<string, JsonSerializerOptions> _optionsDictionary;

    /// <summary>
    /// Initializes a new instance of the <see cref="SystemJsonSerializationOptionsProvider"/> class with default settings.
    /// </summary>
    public SystemJsonSerializationOptionsProvider()
    {
        _optionsDictionary = new Dictionary<string, JsonSerializerOptions>
        {
            [JsonSerializationConstants.GeneralSerializationSettings] =
                Configure(new JsonSerializerOptions()),
            [JsonSerializationConstants.GeneralSerializationWithTypeHandlingSettings] =
                ConfigureWithTypeHandling(new JsonSerializerOptions())
        };
    }

    /// <inheritdoc />
    public JsonSerializerOptions Get()
    {
        return new JsonSerializerOptions(_optionsDictionary[JsonSerializationConstants.GeneralSerializationSettings]);
    }

    /// <inheritdoc />
    public JsonSerializerOptions Get(string serializationOptionsKey)
    {
        return _optionsDictionary.TryGetValue(serializationOptionsKey, out var value)
            ? new JsonSerializerOptions(value)
            : throw new KeyNotFoundException("The specified JSON serialization options with key does not exist.");
    }

    /// <inheritdoc />
    public JsonSerializerOptions GetWithTypeHandling()
    {
        return new JsonSerializerOptions(
            _optionsDictionary[JsonSerializationConstants.GeneralSerializationWithTypeHandlingSettings]);
    }

    /// <inheritdoc />
    public void Add(string serializationOptionsKey, JsonSerializerOptions options)
    {
        _optionsDictionary[serializationOptionsKey] = options;
    }

    /// <inheritdoc />
    public void Update(string serializationOptionsKey, JsonSerializerOptions newOptions)
    {
        _optionsDictionary[serializationOptionsKey] = _optionsDictionary.ContainsKey(serializationOptionsKey)
            ? newOptions
            : throw new KeyNotFoundException("The specified JSON serialization options with key does not exist.");
    }

    /// <inheritdoc />
    public bool Remove(string serializationOptionsKey)
    {
        return _optionsDictionary.Remove(serializationOptionsKey);
    }

    /// <inheritdoc />
    public JsonSerializerOptions Configure(JsonSerializerOptions options)
    {
        // Configures the output JSON formatting for readability
        options.WriteIndented = true;

        // Configures the property naming policy to use camelCase
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

        // Ignores null values during serialization
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

        return options;
    }

    /// <inheritdoc />
    public JsonSerializerOptions ConfigureWithTypeHandling(JsonSerializerOptions options)
    {
        Configure(options);

        // Configures the type information handling
        options.TypeInfoResolver = new DefaultJsonTypeInfoResolver
        {
            Modifiers = { AddPolymorphismHandling }
        };

        return options;
    }

    private static void AddPolymorphismHandling(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Type is { IsClass: true, IsSealed: false } && typeInfo.Type != typeof(string))
        {
            typeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType,
                TypeDiscriminatorPropertyName = "$type"
            };
        }
    }
}