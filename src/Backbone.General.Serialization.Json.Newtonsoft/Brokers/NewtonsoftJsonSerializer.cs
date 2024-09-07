using Backbone.General.Serialization.Json.Abstractions.Brokers;
using Backbone.General.Serialization.Json.Abstractions.Constants;
using Newtonsoft.Json;

namespace Backbone.General.Serialization.Json.Newtonsoft.Brokers;

/// <summary>
/// Provides JSON serializer functionality using Newtonsoft serialization provider with optional serialization settings.
/// </summary>
public class NewtonsoftJsonSerializer : IJsonSerializer
{
    private readonly INewtonsoftJsonSerializationSettingsProvider _serializationSettingsProvider;
    private readonly JsonSerializer _defaultSerializer;
    private readonly JsonSerializer _generalSerializer;
    private readonly JsonSerializer _generalWithTypeHandlingSerializer;

    public NewtonsoftJsonSerializer(INewtonsoftJsonSerializationSettingsProvider serializationSettingsProvider)
    {
        _serializationSettingsProvider = serializationSettingsProvider;

        _defaultSerializer = new JsonSerializer();
        _generalSerializer = JsonSerializer.Create(_serializationSettingsProvider.Get(JsonSerializationConstants.GeneralSerializationSettings));
        _generalWithTypeHandlingSerializer = JsonSerializer.Create(
            _serializationSettingsProvider.Get(JsonSerializationConstants.GeneralSerializationWithTypeHandlingSettings));
    }

    /// <summary>
    /// Serializes an object to a JSON representation using Newtonsoft serializer with optional serialization settings.
    /// </summary>
    public string Serialize<T>(T data, string? serializationSettingsKey = null)
    {
        var serializer = GetSerializerBySettingsKey(serializationSettingsKey);
        return SerializeWithSerializer(data, serializer);
    }

    /// <summary>
    /// Deserializes a JSON representation to an object using Newtonsoft serializer with optional serialization settings.
    /// </summary>
    public T Deserialize<T>(string json, string? serializationSettingsKey = null)
    {
        var serializer = GetSerializerBySettingsKey(serializationSettingsKey);
        return DeserializeWithSerializer<T>(json, serializer);
    }

    /// <summary>
    /// Serializes an object to a JSON representation using Newtonsoft serializer with optional serialization settings.
    /// </summary>
    public async ValueTask<string> SerializeAsync<T>(T data, string? serializationSettingsKey = null)
    {
        return await Task.Run(() => Serialize(data, serializationSettingsKey));
    }

    /// <summary>
    /// Deserializes a JSON representation to an object using Newtonsoft serializer with optional serialization settings.
    /// </summary>
    public async ValueTask<T> DeserializeAsync<T>(string json, string? serializationSettingsKey = null)
    {
        return await Task.Run(() => Deserialize<T>(json, serializationSettingsKey));
    }

    /// <summary>
    /// Gets the appropriate JsonSerializer based on the serialization settings key.
    /// </summary>
    private JsonSerializer GetSerializerBySettingsKey(string? serializationSettingsKey)
    {
        return serializationSettingsKey switch
        {
            null => _defaultSerializer,
            JsonSerializationConstants.GeneralSerializationSettings => _generalSerializer,
            JsonSerializationConstants.GeneralSerializationWithTypeHandlingSettings => _generalWithTypeHandlingSerializer,
            _ => JsonSerializer.Create(_serializationSettingsProvider.Get(serializationSettingsKey))
        };
    }

    private string SerializeWithSerializer<T>(T data, JsonSerializer serializer)
    {
        using var stringWriter = new StringWriter();
        using var jsonWriter = new JsonTextWriter(stringWriter);

        serializer.Serialize(jsonWriter, data);
        return stringWriter.ToString();
    }

    private T DeserializeWithSerializer<T>(string json, JsonSerializer serializer)
    {
        using var stringReader = new StringReader(json);
        using var jsonReader = new JsonTextReader(stringReader);

        return serializer.Deserialize<T>(jsonReader)
               ?? throw new InvalidOperationException("Failed to deserialize using Newtonsoft serializer.");
    }
}