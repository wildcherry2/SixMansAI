using System.Text.Json.Serialization;

namespace Database.Structs;

public struct FField {
    [JsonPropertyName("name")]
    public string name;

    [JsonPropertyName("value")]
    public string value;
}