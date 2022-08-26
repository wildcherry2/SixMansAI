using System.Text.Json.Serialization;

namespace Database.Structs;

public struct FEmbed {
    [JsonPropertyName("title")]
    public string title;

    [JsonPropertyName("description")]
    public string description;

    [JsonPropertyName("fields")]
    public List<FField> fields;
}