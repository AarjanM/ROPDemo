using System.Text.Json;
using System.Text.Json.Serialization;

namespace RopDemo.Domain;

public readonly record struct AccountId(Guid Value) : IEntityId
{
    public static readonly AccountId Empty = new(Guid.Empty);
    public override string ToString() => Value.ToString();
}

public class AccountIdConverter : JsonConverter<AccountId>
{
    public override AccountId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetGuid();
        return new AccountId(value);
    }

    public override void Write(Utf8JsonWriter writer, AccountId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value.ToString());
    }
}