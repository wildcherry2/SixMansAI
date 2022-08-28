
using Database.Database.DatabaseCore.Season;
using Database.Database.DatabaseCore.Season.Cleaners;
using Database.Enums;
using Database.Structs;

namespace Database.Database.DatabaseCore; 

public class PlayerFactory : ILogger {
    private static PlayerFactory? singleton   { get; set; }
    public         bool           bIsComplete { get; set; } = false;
    private PlayerFactory() : base(ConsoleColor.Yellow, 1, "PlayerFactory"){}

    public static PlayerFactory GetSingleton() {
        if (singleton == null) singleton = new PlayerFactory();
        return singleton;
    }

    // Precondition: expects chat_messages to have already run through ChatCleaner.ProcessChat
    // Postcondition: DDatabaseCore's singleton's all_players field is initialized with data
    public void ProcessChat(FMessageList chat_messages) {
        if (!ChatCleaner.GetSingleton().bIsComplete) {
            Log("Precondition not met! Chat has not been cleaned!");
            return;
        }

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

        if (players.Count > 0) bIsComplete = true;
        else Log("No players have been created");
        DDatabaseCore.GetSingleton().all_players = players;
    }

    private bool FollowsValidPattern(ref DDiscordMessage previous_previous, ref DDiscordMessage previous, ref DDiscordMessage current, ref DDiscordMessage next, ref DDiscordMessage next_next) {
        if (current.type == EMessageType.PLAYER_Q && (previous.IsBotNotification() || previous.IsBotResponse()) && next.IsBotResponse()
            && (!previous_previous.IsBotNotification() && !previous_previous.IsBotResponse()) && !next_next.IsBotResponse())
            return true;


        return false;
    }
}