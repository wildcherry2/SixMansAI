using System.Text.Json.Serialization;

namespace Database.Structs;
public struct FAuthor {
    [JsonPropertyName("id")]
    public string id;

    [JsonPropertyName("name")]
    public string name;

    [JsonPropertyName("nickname")]
    public string nickname;

    [JsonPropertyName("is_bot")]
    public string is_bot;
}