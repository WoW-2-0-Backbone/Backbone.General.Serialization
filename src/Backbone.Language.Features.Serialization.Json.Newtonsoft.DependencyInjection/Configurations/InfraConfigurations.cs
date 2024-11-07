using Backbone.Language.Features.Serialization.Json.Abstractions.Brokers;
using Backbone.Language.Features.Serialization.Json.Newtonsoft.Brokers;
using Microsoft.Extensions.DependencyInjection;

namespace Backbone.Language.Features.Serialization.Json.Newtonsoft.DependencyInjection.Configurations;

/// <summary>
/// Provides extension methods to configure the serialization provider.
/// </summary>
public static class InfraConfigurations
{
    /// <summary>
    /// Configures the serialization provider to use Newtonsoft JSON serialization.
    /// </summary>
    /// <param name="services"></param>
    public static void AddNewtonsoftJsonSerializer(this IServiceCollection services)
    {
        services
            .AddSingleton<INewtonsoftJsonSerializationSettingsProvider, NewtonsoftJsonSerializationSettingsProvider>()
            .AddSingleton<IJsonSerializer, NewtonsoftJsonSerializer>();
    }
}