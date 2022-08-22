using Newtonsoft.Json;

namespace Database.Messages.DiscordMessage; 

public class Embed {
    [JsonProperty("title")]       public string      title       { get; set; } = "";
    [JsonProperty("description")] public string      description { get; set; } = "";
    [JsonProperty("fields")]      public List<Field> fields      { get; set; }

    public class Field
    {
        [JsonProperty("name")]  public string name  { get; set; } = "";
        [JsonProperty("value")] public string value { get; set; } = "";
    }
}