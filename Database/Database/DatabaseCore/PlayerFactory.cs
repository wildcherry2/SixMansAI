﻿
using Database.Database.DatabaseCore.Season;
using Database.Enums;
using Database.Structs;

namespace Database.Database.DatabaseCore; 

public class PlayerFactory {
    //public         List<DPlayer>  player_list { get; set; }
    private static PlayerFactory? singleton   { get; set; }

    private PlayerFactory() {
        //player_list = new List<DPlayer>();
    }

    public static PlayerFactory GetSingleton() {
        if (singleton == null) singleton = new PlayerFactory();
        return singleton;
    }

    // Precondition: expects chat_messages to have already run through ChatCleaner.ProcessChat
    public List<DPlayer> ProcessChat(FMessageList chat_messages) {
        var players = DDatabaseCore.GetSingleton().all_players;
        for (var i = 2; i < chat_messages.messages.Count - 2; i++) {
            var previous_previous = chat_messages.messages[i - 2];
            var previous = chat_messages.messages[i - 1];
            var current = chat_messages.messages[i];
            var next = chat_messages.messages[i + 1];
            var next_next = chat_messages.messages[i + 2];
            if (FollowsValidPattern(ref previous_previous, ref previous, ref current, ref next, ref next_next)) {
                var search = DDatabaseCore.GetSingleton().GetPlayerIfExists(ulong.Parse(current.author.id));
                var link = next.GetPlayerNameFromEmbeddedLink(next.GetEmbeddedDescription());
                if (ReferenceEquals(search, null)) {
                    var player = new DPlayer(ulong.Parse(current.author.id), current.author.name, current.author.nickname, link ?? "");
                    players.Add(player);
                }
                else {
                    if(!search.HasName(current.author.name)) 
                        search.TryAddName(current.author.name);
                    if(!search.HasName(current.author.nickname)) 
                        search.TryAddName(current.author.nickname);
                    if (link != null && !search.HasName(link)) 
                        search.TryAddName(link);
                }
            }
        }

        DDatabaseCore.GetSingleton().all_players = players;
        return players;
    }

    private bool FollowsValidPattern(ref DDiscordMessage previous_previous, ref DDiscordMessage previous, ref DDiscordMessage current, ref DDiscordMessage next, ref DDiscordMessage next_next) {
        if (current.type == EMessageType.PLAYER_Q && (previous.IsBotNotification() || previous.IsBotResponse()) && next.IsBotResponse()
            && (!previous_previous.IsBotNotification() && !previous_previous.IsBotResponse()) && !next_next.IsBotResponse())
            return true;


        return false;
    }
}