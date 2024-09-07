using System.Text.Json;

namespace Backbone.General.Serialization.Json.System.Brokers;

/// <summary>
/// Defines JSON serialization options provider for System.Text.Json serialization provider to customize serialization.
/// </summary>
public interface ISystemJsonSerializationOptionsProvider
{
    /// <summary>
    /// Gets clone of default serialization options.
    /// </summary>
    /// <returns>Instance of <see cref="JsonSerializerOptions"/> with default configurations.</returns>
    JsonSerializerOptions Get();

    /// <summary>
    /// Gets clone of specified serialization options.
    /// </summary>
    /// <param name="serializationOptionsKey">The key of serialization options.</param>
    /// <returns>Instance of <see cref="JsonSerializerOptions"/> with default configurations.</returns>
    JsonSerializerOptions Get(string serializationOptionsKey);

    /// <summary>
    /// Gets serialization options for general use but with type handling.
    /// </summary>
    /// <returns>Instance of <see cref="JsonSerializerOptions"/> with default configurations including type handling.</returns>
    JsonSerializerOptions GetWithTypeHandling();

    /// <summary>
    /// Adds serialization options with a key.
    /// </summary>
    /// <param name="serializationOptionsKey">The key of serialization options.</param>
    /// <param name="options">The JSON serialization options to add.</param>
    void Add(string serializationOptionsKey, JsonSerializerOptions options);

    /// <summary>
    /// Updates existing serialization options with a key.
    /// </summary>
    /// <param name="serializationOptionsKey">The key of serialization options to update.</param>
    /// <param name="newOptions">The updated JSON serialization options.</param>
    void Update(string serializationOptionsKey, JsonSerializerOptions newOptions);

    /// <summary>
    /// Removes serialization options associated with a key.
    /// </summary>
    /// <param name="serializationOptionsKey">The key of serialization options to remove.</param>
    /// <returns>True if the key was found and removed; otherwise, false.</returns>
    bool Remove(string serializationOptionsKey);

    /// <summary>
    /// Configures existing serialization options for general use.
    /// </summary>
    /// <param name="options">Instance of <see cref="JsonSerializerOptions"/> to configure.</param>
    /// <returns>Same instance of <see cref="JsonSerializerOptions"/> after default configuration.</returns>
    JsonSerializerOptions Configure(JsonSerializerOptions options);

    /// <summary>
    /// Configures existing serialization options for general use but with type handling.
    /// </summary>
    /// <param name="options">Instance of <see cref="JsonSerializerOptions"/> to configure.</param>
    /// <returns>Same instance of <see cref="JsonSerializerOptions"/> after default configuration including type handling.</returns>
    JsonSerializerOptions ConfigureWithTypeHandling(JsonSerializerOptions options);
}