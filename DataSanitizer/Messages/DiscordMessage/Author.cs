using Newtonsoft.Json;

namespace Database.Messages.DiscordMessage; 

public class Author {
    [JsonProperty("id")]       public  string id       { get; set; } = "";
    [JsonProperty("name")]     public  string name     { get; set; } = "";
    [JsonProperty("nickname")] public  string nickname { get; set; } = "";
    [JsonProperty("isBot")]    private string is_bot   { get; set; } = "";

    public ulong GetDiscordId() {
        return id.Length == 0 ? 1 : ulong.Parse(id);
    }

    public bool IsBot() {
        return is_bot == "true";
    }

    public bool IsHumanMessage() {
        return !IsBot();
    }
}