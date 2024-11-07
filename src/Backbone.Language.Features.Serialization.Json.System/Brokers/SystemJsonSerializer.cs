using System.Text.Json;
using Backbone.Language.Features.Serialization.Json.Abstractions.Brokers;

namespace Backbone.Language.Features.Serialization.Json.System.Brokers;

/// <summary>
/// Provides JSON serializer functionality using Newtonsoft serialization provider with optional serialization settings.
/// </summary>
public class SystemJsonSerializer(ISystemJsonSerializationOptionsProvider serializationOptionsProvider) : IJsonSerializer, IAsyncJsonSerializer
{
    /// <summary>
    /// Serializes an object to a JSON representation using System.Text.Json serializer with optional serialization options.
    /// </summary>
    public string Serialize<T>(T data, string? serializationSettingsKey = null)
    {
        var options = GetOptions(serializationSettingsKey);
        return JsonSerializer.Serialize(data, options);
    }

    /// <summary>
    /// Deserializes a JSON representation to an object using System.Text.Json serializer with optional serialization options.
    /// </summary>
    public T Deserialize<T>(string json, string? serializationSettingsKey = null)
    {
        var options = GetOptions(serializationSettingsKey);
        return JsonSerializer.Deserialize<T>(json, options)
               ?? throw new InvalidOperationException("Failed to deserialize using System.Text.Json serializer.");
    }

    /// <summary>
    /// Serializes an object to a JSON representation asynchronously using System.Text.Json serializer with optional serialization options.
    /// </summary>
    public async ValueTask<string> SerializeAsync<T>(T data, string? serializationSettingsKey = null)
    {
        var options = GetOptions(serializationSettingsKey);
        using var stream = new MemoryStream();
        await JsonSerializer.SerializeAsync(stream, data, options);
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }

    /// <summary>
    /// Deserializes a JSON representation to an object asynchronously using System.Text.Json serializer with optional serialization options.
    /// </summary>
    public async ValueTask<T> DeserializeAsync<T>(string json, string? serializationSettingsKey = null)
    {
        var options = GetOptions(serializationSettingsKey);
        using var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        await writer.WriteAsync(json);
        await writer.FlushAsync();
        stream.Position = 0;
        return await JsonSerializer.DeserializeAsync<T>(stream, options) 
               ?? throw new InvalidOperationException("Failed to deserialize using System.Text.Json serializer.");
    }

    /// <summary>
    /// Gets the appropriate JsonSerializerOptions based on the serialization options key.
    /// </summary>
    private JsonSerializerOptions GetOptions(string? serializationSettingsKey)
    {
        return serializationSettingsKey == null
            ? serializationOptionsProvider.Get()
            : serializationOptionsProvider.Get(serializationSettingsKey);
    }
}