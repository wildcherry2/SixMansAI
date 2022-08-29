using System.Text.Json.Serialization;

namespace Database.Structs;

public class FEmbed {
    [JsonPropertyName("title")]
    public string title { get; init; }

    [JsonPropertyName("description")]
    public string description { get; init; }

    [JsonPropertyName("fields")]
    public List<FField> fields { get; init; }
}