using System.Text.RegularExpressions;
using Database.Messages.DiscordMessage;

namespace Database;

public partial class Database {
    /*  TODO: FIX THIS  */
    private void RegisterPlayerNames()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[RegisterPlayerNames] Registering player names...");
        foreach (var block in queue_blocks)
        {
            for (int i = 0; i + 1 < block.messages.Count; i++)
            {
                var message = block.messages[i];
                var next_message = block.messages[i + 1];
                if (message.IsQMessage() && next_message.IsBotResponsePlayerJoinedMessage()) {
                    var found_player = false;
                    var date = message.datetime;
                    var discord_id = message.author.GetDiscordId();
                    var player_name = message.author.name;
                    var player_nick = message.author.nickname;
                    var linkname = GetPlayerNameFromEmbeddedLink(next_message.GetEmbeddedDescription());
                    foreach (var player in players) {
                        if (player.discord_id == discord_id) {
                            string log = "\t[RegisterPlayerNames] Registered new names for {0} ({4}):";
                            bool has_name = false;
                            if (linkname != null && !player.HasName(ref linkname)) {
                                player.recorded_names.Add(linkname);
                                has_name = true;
                                log += "\n\t\t{1} (linkname)\t";
                            }
                            if (!player.HasName(ref player_name)) {
                                player.recorded_names.Add(player_name);
                                has_name = true;
                                log += "\n\t\t{2} (player_name)\t";
                            }

                            if (!player.HasName(ref player_nick)) {
                                player.recorded_names.Add(player_nick);
                                has_name = true;
                                log += "\n\t\t{3} (player_nickname)\t";
                            }

                            if (has_name) {
                                Console.WriteLine(log, player.recorded_names[0], linkname, player_name, player_nick, date.ToString());
                            }
                            found_player = true;
                            break;
                        }
                    }

                    if (!found_player) {
                        var new_player = new Player.Player(player_name, discord_id, player_nick, linkname != null ? linkname : "");
                        players.Add(new_player);

                        Console.WriteLine("\t[RegisterPlayerNames] Registered new player:\n\t\tplayer_name = {0}\n\t\tplayer_nick = {1}\n\t\tlinkname = {2}\n\t\tdiscord_id = {3}", 
                                          player_name, player_nick, linkname != null ? linkname : "Null", discord_id);
                    }
                }
            }
        }
        Console.WriteLine("\t[RegisterPlayerNames] Registered {0} players!", players.Count);
        Console.ForegroundColor = ConsoleColor.White;
    }

    private static Regex name_from_embedded_link_regex = new Regex(@"(?:(?!^\[|\]\().)+", RegexOptions.Compiled);
    private static Regex is_link_regex                 = new Regex(@"^\[.+\]\(https://www\.rl6mans\.com/profile/.+\) has joined.$", RegexOptions.Compiled);
    public static string? GetPlayerNameFromEmbeddedLink(string link) {
        if(name_from_embedded_link_regex.Match(link).Success)
            return name_from_embedded_link_regex.Match(link).Value;
        return null;
    }
    public static bool IsLinkMessage(string test) {
        return is_link_regex.Match(test).Success;
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