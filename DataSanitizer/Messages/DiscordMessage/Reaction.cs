using Newtonsoft.Json;

namespace Database.Messages.DiscordMessage; 

public class Reaction {
    [JsonProperty("emoji")] public Emoji emojis { get; set; }

    public class Emoji {
        [JsonProperty("id")]   public string id   { get; set; } = "";
        [JsonProperty("name")] public string name { get; set; } = "";
    }
}