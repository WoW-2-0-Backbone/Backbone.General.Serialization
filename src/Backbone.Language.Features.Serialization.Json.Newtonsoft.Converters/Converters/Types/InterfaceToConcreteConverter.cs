using Newtonsoft.Json;

namespace Backbone.Language.Features.Serialization.Json.Newtonsoft.Converters.Converters.Types;

/// <summary>
/// Represents a Newtonsoft JSON converter to deserialize concrete type to interface.
/// </summary>
/// <typeparam name="TInterface">The interface type to which the object will be deserialized.</typeparam>
/// <typeparam name="TConcrete">The concrete implementation of the interface type.</typeparam>
public class InterfaceToConcreteConverter<TInterface, TConcrete> : JsonConverter where TConcrete : TInterface
{
    static InterfaceToConcreteConverter()
    {
        if (typeof(TInterface) == typeof(TConcrete))
            throw new InvalidOperationException($"typeof({typeof(TInterface)}) == typeof({typeof(TConcrete)})");
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override object? ReadJson(
        JsonReader reader, 
        Type objectType, 
        object? existingValue,
        JsonSerializer serializer)
    {
        return serializer.Deserialize(reader, typeof(TConcrete));
    }

    /// <inheritdoc />
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(TInterface);
    }

    /// <inheritdoc />
    public override bool CanWrite => false;
}