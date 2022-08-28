using System.Text.Json.Serialization;

namespace Database.Structs;
public class FReaction {
    [JsonPropertyName("emoji")]
    public FEmoji emoji { get; set; }

    //[JsonPropertyName("count")]

}