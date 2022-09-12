using System.Text.Json.Serialization;
using Database.Database.DatabaseCore.MainComponents;

namespace Database.Database.Structs; 

public class FMessageList {
    public FMessageList() { messages = new List<DDiscordMessage>(); }

    [JsonPropertyName("messages")] public List<DDiscordMessage> messages { get; set; }
}