using Newtonsoft.Json;

public class DiscordMessage {
    [JsonProperty("messages")] public List<Message> messages { get; set; }

    public class Message {
        [JsonProperty("content")]   public string         content   { get; set; } = "";
        [JsonProperty("author")]    public Author         author    { get; set; }
        [JsonProperty("reactions")] public List<Reaction> reactions { get; set; }
        [JsonProperty("mentions")]  public List<Author>   mentions  { get; set; }
        [JsonProperty("embeds")]    public List<Embed>    embeds    { get; set; }

        public bool HasSubs() {
            return mentions.Count > 0;
        }

        public bool IsBot() {
            return (author.is_bot == "true");
        }

        public class Author {
            [JsonProperty("id")]       public string id       { get; set; } = "";
            [JsonProperty("name")]     public string name     { get; set; } = "";
            [JsonProperty("nickname")] public string nickname { get; set; } = "";
            [JsonProperty("isBot")]    public string is_bot   { get; set; } = "";
        }

        public class Reaction {
            [JsonProperty("emoji")] public Emoji emojis { get; set; }

            public class Emoji {
                [JsonProperty("id")]   public string id   { get; set; } = "";
                [JsonProperty("name")] public string name { get; set; } = "";
            }
        }

        public class Embed {
            [JsonProperty("title")]       public string      title       { get; set; } = "";
            [JsonProperty("description")] public string      description { get; set; } = "";
            [JsonProperty("fields")]      public List<Field> fields      { get; set; }

            public class Field {
                [JsonProperty("name")]  public string name  { get; set; } = "";
                [JsonProperty("value")] public string value { get; set; } = "";
            }
        }
    }
}