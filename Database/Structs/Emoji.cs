using System.Text.Json.Serialization;

namespace Database.Structs;
public struct FEmoji {
    [JsonPropertyName("id")]
    public string id;

    [JsonPropertyName("name")]
    public string name;
}