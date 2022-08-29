
using System.Text.Json.Serialization;
using Database.Database.DatabaseCore.MainComponents;

namespace Database.Structs;

public class FMessageList {
    [JsonPropertyName("messages")]
    public List<DDiscordMessage> messages { get; set; }

    public FMessageList() {
        messages = new List<DDiscordMessage>();
    }
}