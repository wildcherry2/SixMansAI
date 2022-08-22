using System.Text.RegularExpressions;
using Database.Messages.DiscordMessage;

namespace Database;

public partial class Database {
    /*  TODO: FIX THIS  */
    private void RegisterPlayerNames() {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[RegisterPlayerNames] Registering player names...");
        foreach (var block in queue_blocks) {
            for (int i = 0; i < block.messages.Count; i++) {
                var message = block.messages[i];
                if (message.IsQMessage() || message.IsLeaveMessage()) {
                    LookupAndTryInsertPlayer(ref message);
                }
                else if(/*message.author.IsHumanMessage() && */message.IsBotResponseMessage()){
                    //string linkname = message.embeds[0].description.Substring(1, (message.embeds[0].description.IndexOf(']')) - 1);
                    string linkname = GetPlayerNameFromEmbeddedLink(message.GetEmbeddedDescription());
                    bool found = false;
                    foreach (var player in players) {
                        if (block.messages[i-1].author.GetDiscordId() == player.discord_id) {
                            Console.WriteLine("\t[RegisterPlayerNames] Registered linkname " + linkname + " for " +
                                              player.discord_id);
                            player.recorded_names.Add(linkname);
                            found = true;
                            break;
                        }
                    }

                    if (!found) {
                        Console.WriteLine("\t[RegisterPlayerNames] Could not find player for link name {0}", linkname);
                    }
                }
            }
        }
        Console.ForegroundColor = ConsoleColor.White;
    }
    public static string GetPlayerNameFromEmbeddedLink(string link) {
        return new Regex(@"(?:(?!^\[|\]\().)+", RegexOptions.Compiled)
               .Match(link.Trim()).Value;
    }
    public static int GetIndexOfPlayer(ref List<Player.Player> players, ulong current_id) {
        for (int i = 0; i < players.Count; i++) {
            if (players[i].discord_id == current_id)
                return i;
        }

        return -1;
    }
    private void LookupAndTryInsertPlayer(ref DiscordMessage message) {
        Console.ForegroundColor = ConsoleColor.Yellow;
        var auth_id = ulong.Parse(message.author.id);
        var name = message.author.name;
        var nick = message.author.nickname;
        foreach (var player in players) {
            if (player.IsPlayer(auth_id)) {
                if (!player.HasName(ref name)) {
                    player.recorded_names.Add(name);
                    Console.WriteLine("\t[LookupAndTryInsertPlayer] Registered name " + name + " for id " + auth_id);
                }
                if (!player.HasName(ref nick)) {
                    player.recorded_names.Add(nick);
                    Console.WriteLine("\t[LookupAndTryInsertPlayer] Registered name " + nick + " for id " + auth_id);
                }
                Console.ForegroundColor = ConsoleColor.Green;
            }
        }
        var new_player = new Player.Player(name, auth_id, nick);
        Console.WriteLine("\t[LookupAndTryInsertPlayer] Creating new Player with name = {0}, nick = {1}, id = {2}", name, nick, auth_id);
        players.Add(new_player);
        Console.ForegroundColor = ConsoleColor.Green;
    }
}