using System.Text.Json.Serialization;

namespace Database.Database.Structs; 

public class FReaction {
    [JsonPropertyName("emoji")] public FEmoji emoji { get; init; }
}