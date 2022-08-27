using System.Text.Json.Serialization;

namespace Database.Structs;
public class FEmoji {
    [JsonPropertyName("id")]
    public string id { get; set; }

    [JsonPropertyName("name")]
    public string name { get; set; }
}