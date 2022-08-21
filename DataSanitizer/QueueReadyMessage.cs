using DataSanitizer;

class QueueReadyMessage {
    public int match_id { get; set; } = -1;
    public List<string> team_one { get; set; } = new List<string>();
    public List<string> team_two { get; set; } = new List<string>();

    public QueueReadyMessage(){}
    public QueueReadyMessage(DiscordMessage.Message msg) {
        var title_tokens = msg.embeds[0].title.Split(' ');
        match_id = int.Parse(title_tokens[1].Substring(1));

        var team_one_tokens = msg.embeds[0].fields[0].value.Split(',');
        foreach(var token in team_one_tokens)
            team_one.Add(token);

        var team_two_tokens = msg.embeds[0].fields[1].value.Split(',');
        foreach(var token in team_two_tokens)
            team_two.Add(token);
    }

    public static QueueReadyMessage ParseBlockIntoQueue(ref Database.QueueBlock block) {
        QueueReadyMessage qrm_return = new QueueReadyMessage();

        return qrm_return;
    }

    public static bool IsLobbyMessage(ref DiscordMessage.Message message) {
        if (message.embeds.Count > 0 && message.embeds[0].description == "You may now join the team channels") return true;

        return false;
    }

    public static bool IsQMessage(ref DiscordMessage.Message message) {
        if (message.content == @"!q") return true;

        return false;
    }

    public static bool IsLeaveMessage(ref DiscordMessage.Message message) {
        if (message.content == @"!leave") return true;

        return false;
    }

    public static bool IsVotingCompleteMessage(ref DiscordMessage.Message message) {
        if (message.embeds.Count > 0 && message.embeds[0].description ==
            "All players must join within 7 minutes and then teams will be chosen.\n**Vote result:**")
            return true;

        return false;
    }

    public static bool IsBotMessage(ref DiscordMessage.Message message) {
        return message.author.name == "6MansBot";
    }

    public static bool IsBotResponsePlayerJoinedMessage(ref DiscordMessage.Message message) {
        if (message.embeds.Count > 0 && message.embeds[0].description.Contains(") has joined.")) return true;

        return false;
    }

    public static bool IsBotResponsePlayerLeftMessage(ref DiscordMessage.Message message) {
        if (message.embeds.Count > 0 && message.embeds[0].description.Contains(") has left (using command).")) return true;

        return false;
    }
}