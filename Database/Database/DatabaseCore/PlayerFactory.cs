
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
        for (var i = 1; i < chat_messages.messages.Count - 1; i++) {
            
            var previous = chat_messages.messages[i - 1];
            var current = chat_messages.messages[i];
            var next = chat_messages.messages[i + 1];
            if (FollowsValidPattern(ref previous, ref current, ref next)) {
                var search = DDatabaseCore.GetSingleton().GetPlayerIfExists(ulong.Parse(current.author.id));
                var link = next.GetPlayerNameFromEmbeddedLink(next.GetEmbeddedDescription());
                if (ReferenceEquals(search, null)) {
                    var player = new DPlayer(ulong.Parse(current.author.id), current.author.name, current.author.nickname, link ?? "");
                    players.Add(player);
                }
                else {
                    if(!search.HasName(current.author.name)) search.recorded_names.Add(current.author.name);
                    if(!search.HasName(current.author.nickname)) search.recorded_names.Add(current.author.nickname);
                    if (link != null && !search.HasName(link)) search.recorded_names.Add(link);
                }
            }
        }

        return players;
    }

    private bool FollowsValidPattern(ref DDiscordMessage previous, ref DDiscordMessage current, ref DDiscordMessage next) {
        if (current.type == EMessageType.PLAYER_Q && (previous.IsBotNotification() || previous.IsBotResponse()) && (next.IsBotNotification() || next.IsBotResponse()))
            return true;


        return false;
    }
}