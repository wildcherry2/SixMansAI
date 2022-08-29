using System.Text.Json.Serialization;

namespace Database.Structs;
public class FAuthor {
    [JsonPropertyName("id")]
    public string? id { get; init; }

    [JsonPropertyName("name")]
    public string? name { get; init; }

    [JsonPropertyName("nickname")]
    public string? nickname { get; init; }

    [JsonPropertyName("isBot")]
    public bool? isBot { get; init; }
}