
using System.Text.Json.Serialization;
using Database.Enums;
using Database.Structs;

namespace Database.Database.DatabaseCore.Season;
public class DDiscordMessage : IDatabaseComponent {
    [JsonPropertyName("id")]
    public string? id { get; set; }

    [JsonPropertyName("content")]
    public string? content { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime?        timestamp  { get; set; }

    [JsonPropertyName("author")]
    public FAuthor? author { get; set; }

    [JsonPropertyName("reactions")]
    public List<FReaction>? reactions { get; set; }

    [JsonPropertyName("mentions")]
    public List<FAuthor>? mentions { get; set; }

    [JsonPropertyName("embeds")]
    public List<FEmbed>? embeds { get; set; }

    [JsonIgnore]
    public EMessageType    type       { get; set; } = EMessageType.UNKNOWN;

    [JsonConstructor]
    public DDiscordMessage(string? id, string? content, DateTime? timestamp, FAuthor? author, List<FReaction>? reactions, List<FAuthor>? mentions, List<FEmbed>? embeds) : base(ConsoleColor.Magenta, 4, "DDiscordMessage") {
        this.id = id;
        this.content = content;
        this.timestamp = timestamp;
        this.author = author;
        this.reactions = reactions;
        this.mentions = mentions;
        this.embeds = embeds;
        SetMessageType();
    }

    public int GetMatchId() {
        if (IsAuthorHuman()) return -1;
        try {
            switch (type) {
                case EMessageType.VOTING_COMPLETE:
                    return GetMatchIdFromVotingCompleteMessage();
                case EMessageType.TEAMS_PICKED:
                    return GetMatchIdFromTeamsPickedMessage();
                case EMessageType.SCORE_REPORT:
                    return GetMatchIdFromScoreReportMessage();
                default:
                    return -1;
            }
        }
        catch (Exception ex) {
            Log(ex.Message);
            return -1;
        }
    }

    private int GetMatchIdFromScoreReportMessage() {
        try {
            if (!IsScoreReportMessage()) return -1;
            return int.Parse(content.Split(' ')[1]);
        }
        catch (Exception ex) {
            Log("Could not parse match ID from score report message! Content = {0}, Exception = {1}", content, ex.Message);
            return -1;
        }
    }

    private int GetMatchIdFromVotingCompleteMessage() {
        try {
            if (!IsVotingCompleteMessage()) return -1;
            return int.Parse(RegularExpressions.lobby_id_regex.Match(embeds[0].title).Value);
        }
        catch (Exception ex) {
            Log("Could not parse match ID from voting complete message! Content = {0}, Exception = {1}", content, ex.Message);
            return -1;
        }
    }

    private int GetMatchIdFromTeamsPickedMessage() {
        try {
            if (!IsTeamsPickedMessage()) return -1;
            return int.Parse(RegularExpressions.lobby_id_regex.Match(embeds[0].title).Value);
        }
        catch (Exception ex) {
            Log("Could not parse match ID from teams picked message! Content = {0}, Exception = {1}", content, ex.Message);
            return -1;
        }
    }

    public string? GetEmbeddedTitle() {
        return embeds != null && embeds.Count > 0? embeds[0].title : null;
    }

    public string? GetEmbeddedDescription() {
        return embeds != null && embeds.Count > 0? embeds[0].description : null;
    }

    public FField? GetEmbeddedField(int index) {
        return embeds != null && embeds.Count > 0 && embeds[0].fields.Count >= index + 1 ? embeds[0].fields[index] : null;
    }

    // TODO: Unit test GetPlayerNameFromEmbeddedLink
    public string? GetPlayerNameFromEmbeddedLink(string embedded_link) {
        var match = RegularExpressions.name_from_embedded_link_regex.Match(embedded_link);
        if (match.Success) {
            return match.Value;
        }

        return null;
    }

    public bool HasSubstitutes() {
        return IsScoreReportMessage() && mentions.Count > 0;
    }

    // TODO: Unit test GetPlayerNamesFromTeamPickedMessage
    public string[]? GetPlayerNamesFromTeamPickedMessage() {
        if (!IsTeamsPickedMessage()) return null;
        var ret = new string[6];
        var link_strings = GetEmbeddedField(0).value.Split(", ");
        if (link_strings.Length != 3) return null;

        ret[0] = GetPlayerNameFromEmbeddedLink(link_strings[0]);
        ret[1] = GetPlayerNameFromEmbeddedLink(link_strings[1]);
        ret[2] = GetPlayerNameFromEmbeddedLink(link_strings[2]);

        link_strings = GetEmbeddedField(1).value.Split(", ");
        if (link_strings.Length != 3) return null;

        ret[3] = GetPlayerNameFromEmbeddedLink(link_strings[0]);
        ret[4] = GetPlayerNameFromEmbeddedLink(link_strings[1]);
        ret[5] = GetPlayerNameFromEmbeddedLink(link_strings[2]);

        return ret;
    }

    // TODO: Use GetPlayerNamesFromTeamPickedMessage to get names instead, then do a lookup for either team in one function with an ETeamLabel to indicate the team in the parameters
    public List<DPlayer>? GetTeamOne() {
        if (!IsTeamsPickedMessage()) return null;

        var all_players = DDatabaseCore.GetSingleton().all_players;
        var link_strings = GetEmbeddedField(0).value.Split(", ");
        var team_one = new List<DPlayer>();

        foreach (var str in link_strings) {
            var name = RegularExpressions.name_from_embedded_link_regex.Match(str);
            bool found = false;
            if (name.Success) {
                foreach (var player in all_players) {
                    if (player == name.Value) {
                        found = true;
                        team_one.Add(player);
                        break;
                    }
                }

                // All names should have been parsed at this point, so if it's not found that means I missed something and the result is erroneous
                if (!found) {
                    bError = true;
                    Log("Could not find player with name {0}", name.Value);
                    return null;
                }
            }
        }

        return team_one;
    }

    public List<DPlayer>? GetTeamTwo() {
        if (!IsTeamsPickedMessage()) return null;

        var all_players = DDatabaseCore.GetSingleton().all_players;
        var link_strings = GetEmbeddedField(0).value.Split(", ");
        var team_two = new List<DPlayer>();

        foreach (var str in link_strings) {
            var name = RegularExpressions.name_from_embedded_link_regex.Match(str);
            bool found = false;
            if (name.Success) {
                foreach (var player in all_players) {
                    if (player == name.Value) {
                        found = true;
                        team_two.Add(player);
                        break;
                    }
                }

                // All names should have been parsed at this point, so if it's not found that means I missed something and the result is erroneous
                if (!found) {
                    bError = true;
                    Log("Could not find player with name {0}", name.Value);
                    return null;
                }
            }
        }

        return team_two;
    }

    private void SetMessageType() {
        if (IsQMessage()) type = EMessageType.PLAYER_Q;
        else if (IsLeaveMessage()) type = EMessageType.PLAYER_LEAVE;
        else if (IsBotResponseToPlayerQ()) type = EMessageType.BOT_RESPONSE_TO_PLAYER_Q;
        else if (IsBotResponseToPlayerLeave()) type = EMessageType.BOT_RESPONSE_TO_PLAYER_LEAVE;
        else if (IsScoreReportMessage()) type = EMessageType.SCORE_REPORT;
        else if (IsTeamsPickedMessage()) type = EMessageType.TEAMS_PICKED;
        else if (IsVotingCompleteMessage()) type = EMessageType.VOTING_COMPLETE;
        else if (IsLobbyCancelledNoPickMessage()) type = EMessageType.BOT_LOBBY_CANCELLED;
        else {
            type = EMessageType.UNKNOWN;
            bError = true;
        }
    }

    private bool IsQMessage() {
        return content == @"!q";
    }

    private bool IsLeaveMessage() {
        return content == @"!leave";
    }

    private bool IsVotingCompleteMessage() {
        return IsAuthorBot() && embeds.Count > 0 && embeds[0].description.Contains("All players must join within 7 minutes and then teams will be chosen.\n**Vote result:**");
    }

    private bool IsScoreReportMessage() {
        return IsAuthorHuman() && RegularExpressions.score_report_regex.IsMatch(content);
    }

    private bool IsTeamsPickedMessage() {
        return IsAuthorBot() && GetEmbeddedDescription() == "You may now join the team channels";
    }

    // TODO: Unit test lobby IsLobbyCancelledNoPickMessage
    // With NameFactory, if this is true then reset the counter, since captains not picking resets the queue
    private bool IsLobbyCancelledNoPickMessage() {
        return IsAuthorBot() && GetEmbeddedTitle().Contains("Captain failed to pick players for **Match ID");
    }

    private bool IsAuthorBot() {
        return author != null && (author.isBot ? true : false);
    }

    private bool IsAuthorHuman() {
        return !IsAuthorBot();
    }

    // Returns true if this message is a bot response to a player !q or !leave command
    public bool IsBotResponse() {
        return IsBotResponseToPlayerQ() || IsBotResponseToPlayerLeave();
    }

    // Returns true if this message is a voting complete or teams picked message
    public bool IsBotNotification() {
        return IsVotingCompleteMessage() || IsTeamsPickedMessage();
    }

    private bool IsBotResponseToPlayerQ() {
        return IsAuthorBot() && embeds.Count > 0 && embeds[0].description.Contains(") has joined.");
    }

    private bool IsBotResponseToPlayerLeave() {
        return IsAuthorBot() && embeds.Count > 0 && embeds[0].description.Contains(") has left (using command).");
    }

    #region Inherited Overrides
    protected override bool IsEqual(IDatabaseComponent? rhs) {
        var rhs_casted = rhs != null ? (rhs as DDiscordMessage) : null;
        if(rhs_casted) 
            return id == rhs_casted.id;
        return false;
    }
    protected override bool IsLessThan(IDatabaseComponent? rhs) {
        var rhs_casted = rhs != null ? (rhs as DDiscordMessage) : null;
        if (rhs_casted) {
            return ulong.Parse(id) < ulong.Parse(rhs_casted.id);
        }

        return false;
    }
    public override string ToJson() {
        /*
         *
         *  CONVERT TO JSON
         *
         */

        return "";
    }
    public override void ToJson(string save_path) {

    }
    public override void FromJson(string save_path) {

    }
    #endregion
}