using Newtonsoft.Json;

namespace Backbone.Language.Features.Serialization.Json.Newtonsoft.Brokers;

/// <summary>
/// Defines JSON serialization settings provider for Newtonsoft serialization provider to customize serialization.
/// </summary>
public interface INewtonsoftJsonSerializationSettingsProvider
{
    /// <summary>
    /// Gets clone of default serialization settings.
    /// </summary>
    /// <returns>Instance of <see cref="JsonSerializerSettings"/> with default configurations.</returns>
    JsonSerializerSettings Get();

    /// <summary>
    /// Gets clone of specified serialization settings.
    /// </summary>
    /// <param name="serializationSettingsKey">The key of serialization settings.</param>
    /// <returns>Instance of <see cref="JsonSerializerSettings"/> with default configurations.</returns>
    JsonSerializerSettings Get(string serializationSettingsKey);

    /// <summary>
    /// Gets serialization settings for general use but with type handling.
    /// </summary>
    /// <returns>Instance of <see cref="JsonSerializerSettings"/> with default configurations including type handling.</returns>
    JsonSerializerSettings GetWithTypeHandling();

    /// <summary>
    /// Adds serialization settings with a key.
    /// </summary>
    /// <param name="serializationSettingsKey">The key of serialization settings.</param>
    /// <param name="settings">The JSON serialization settings to add.</param>
    void Add(string serializationSettingsKey, JsonSerializerSettings settings);

    /// <summary>
    /// Updates existing serialization settings with a key.
    /// </summary>
    /// <param name="serializationSettingsKey">The key of serialization settings to update.</param>
    /// <param name="newSettings">The updated JSON serialization settings.</param>
    void Update(string serializationSettingsKey, JsonSerializerSettings newSettings);

    /// <summary>
    /// Removes serialization settings associated with a key.
    /// </summary>
    /// <param name="serializationSettingsKey">The key of serialization settings to remove.</param>
    /// <returns>True if the key was found and removed; otherwise, false.</returns>
    bool Remove(string serializationSettingsKey);

    /// <summary>
    /// Configures existing serialization settings for general use.
    /// </summary>
    /// <param name="settings">Instance of <see cref="JsonSerializerSettings"/> to configure.</param>
    /// <returns>Same instance of <see cref="JsonSerializerSettings"/> after default configuration.</returns>
    JsonSerializerSettings Configure(JsonSerializerSettings settings);

    /// <summary>
    /// Configures existing serialization settings for general use but with type handling.
    /// </summary>
    /// <param name="settings">Instance of <see cref="JsonSerializerSettings"/> to configure.</param>
    /// <returns>Same instance of <see cref="JsonSerializerSettings"/> after default configuration including type handling.</returns>
    JsonSerializerSettings ConfigureWithTypeHandling(JsonSerializerSettings settings);
}