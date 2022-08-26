using System.Text.Json.Serialization;

namespace Database.Structs;
public struct FReaction {
    [JsonPropertyName("emoji")]
    public FEmoji emoji;
}