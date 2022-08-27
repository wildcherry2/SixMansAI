using System.Text.Json.Serialization;

namespace Database.Structs;
public class FAuthor {
    [JsonPropertyName("id")]
    public string id { get; set; } = "";

    [JsonPropertyName("name")]
    public string name { get; set; } = "";

    [JsonPropertyName("nickname")]
    public string nickname { get; set; } = "";

    [JsonPropertyName("isBot")]
    public bool isBot { get; set; }
}