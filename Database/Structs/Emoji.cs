using System.Text.Json.Serialization;

namespace Database.Structs;
public class FEmoji {
    [JsonPropertyName("id")]
    public string id { get; init; }

    [JsonPropertyName("name")]
    public string name { get; init; }
}