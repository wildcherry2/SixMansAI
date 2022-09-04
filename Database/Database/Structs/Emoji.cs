using System.Text.Json.Serialization;

namespace Database.Database.Structs; 

public class FEmoji {
    [JsonPropertyName("id")] public string id { get; init; }

    [JsonPropertyName("name")] public string name { get; init; }
}