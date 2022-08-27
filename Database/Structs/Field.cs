using System.Text.Json.Serialization;

namespace Database.Structs;

public class FField {
    [JsonPropertyName("name")]
    public string name { get; set; }

    [JsonPropertyName("value")]
    public string value { get; set; }
}