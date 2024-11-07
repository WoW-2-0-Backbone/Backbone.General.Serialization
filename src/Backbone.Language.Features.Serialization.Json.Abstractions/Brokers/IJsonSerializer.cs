namespace Backbone.Language.Features.Serialization.Json.Abstractions.Brokers;

/// <summary>
/// Defines JSON serializer functionality with optional serialization settings.
/// </summary>
public interface IJsonSerializer
{
    /// <summary>
    /// Serializes an object to a JSON representation.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="data">The object to serialize.</param>
    /// <param name="serializationSettingsKey">The key to specify the optional serialization settings.</param>
    /// <returns>Serialized JSON representation of the object.</returns>
    string Serialize<T>(T data, string? serializationSettingsKey = null);

    /// <summary>
    /// Deserializes a JSON representation to an object.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize.</typeparam>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <param name="serializationSettingsKey">The key to specify the optional serialization settings.</param>
    /// <returns>Deserialized instance of the object.</returns>
    T Deserialize<T>(string json, string? serializationSettingsKey = null);

    /// <summary>
    /// Serializes an object to a JSON representation.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="data">The object to serialize.</param>
    /// <param name="serializationSettingsKey">The key to specify the optional serialization settings.</param>
    /// <returns>Serialized JSON representation of the object.</returns>
    ValueTask<string> SerializeAsync<T>(T data, string? serializationSettingsKey = null);

    /// <summary>
    /// Deserializes a JSON representation to an object.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize.</typeparam>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <param name="serializationSettingsKey">The key to specify the optional serialization settings.</param>
    /// <returns>Deserialized instance of the object.</returns>
    ValueTask<T> DeserializeAsync<T>(string json, string? serializationSettingsKey = null);
}