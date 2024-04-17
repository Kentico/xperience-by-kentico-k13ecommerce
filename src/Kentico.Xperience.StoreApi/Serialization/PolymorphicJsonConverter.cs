using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kentico.Xperience.StoreApi.Serialization;

/// <summary>
/// Converter which enable polymorphism on models serialized/deserialized by System.Text.Json
/// </summary>
/// <typeparam name="T"></typeparam>
public class PolymorphicJsonConverter<T> : JsonConverter<T> where T : class
{
    public override bool CanConvert(Type typeToConvert) => typeof(T) == typeToConvert;

    protected virtual Type GetObjectType(JsonDocument jsonDocument) => typeof(T);

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        var reader2 = reader;
        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        var objectType = GetObjectType(jsonDocument);
        if (objectType.IsAbstract || objectType.IsInterface)
        {
            throw new JsonException(
                "Json object cannot be serialized into abstract type. Override GetObjectType method to specify non abstract destination type.");
        }

        return JsonSerializer.Deserialize(ref reader2, objectType, options) as T;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) =>
        JsonSerializer.Serialize(writer, value, value?.GetType() ?? typeof(T), options);
}
