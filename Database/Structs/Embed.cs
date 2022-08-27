using System.Text.Json.Serialization;

namespace Database.Structs;

public class FEmbed {
    [JsonPropertyName("title")]
    public string title { get; set; }

    [JsonPropertyName("description")]
    public string description { get; set; }

    [JsonPropertyName("fields")]
    public List<FField> fields { get; set; }
}