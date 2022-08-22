﻿using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Database.Messages.DiscordMessage;

public class DiscordMessage {
    [JsonProperty("content")]   public  string         content   { get; set; } = "";
    [JsonProperty("author")]    public  Author         author    { get; set; }
    [JsonProperty("reactions")] public  List<Reaction> reactions { get; set; }
    [JsonProperty("mentions")]  public  List<Author>   mentions  { get; set; }
    [JsonProperty("embeds")]    private List<Embed>    embeds    { get; set; }

    public int GetLobbyId() {
        if (!((IsVotingCompleteMessage() && !IsJoinGameMessage()) || (!IsVotingCompleteMessage() && IsJoinGameMessage()))) return -1;
        try {
            var rx = new Regex(@"[0-9]+", RegexOptions.Compiled);
            return int.Parse(rx.Match(embeds[0].title).Value);
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return -1;
        }
    }
    public string GetEmbeddedTitle()       { return embeds[0].title; }
    public string GetEmbeddedDescription() { return embeds[0].description; }
    public Embed.Field GetEmbeddedField(int index) {
        return embeds[0].fields[index];
    }
    public bool HasSubs() { return mentions.Count > 0; }
    public bool IsJoinGameMessage() {
        if (author.IsBot() && embeds.Count > 0 && embeds[0].description == "You may now join the team channels") return true;

        return false;
    }
    public bool IsQMessage() {
        if (content == @"!q") return true;

        return false;
    }
    public bool IsLeaveMessage() {
        if (content == @"!leave") return true;

        return false;
    }
    public bool IsVotingCompleteMessage() {
        if (author.IsBot() && embeds.Count > 0 && embeds[0].description.Contains("All players must join within 7 minutes and then teams will be chosen.\n**Vote result:**")) return true;

        return false;
    }
    public bool IsBotResponsePlayerJoinedMessage() {
        if (author.IsBot() && embeds.Count > 0 && embeds[0].description.Contains(") has joined.")) return true;

        return false;
    }
    public bool IsBotResponsePlayerLeftMessage() {
        if (author.IsBot() && embeds.Count > 0 && embeds[0].description.Contains(") has left (using command).")) return true;

        return false;
    }
    public bool IsBotResponseMessage() { return IsBotResponsePlayerJoinedMessage() || IsBotResponsePlayerLeftMessage(); }
    public List<DiscordMessage> raw_messages { get; set; }
    public static DiscordMessage RawDataToDiscordMessage(ref StreamReader sr) { return JsonConvert.DeserializeObject<DiscordMessage>(sr.ReadToEnd()); }
}