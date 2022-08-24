using Database.Messages.ScoreReportMessage;
using Database.Player;
using Database.Messages.DiscordMessage;

public class Queue {
    public Queue() {
        players_in_queue = new List<Player>();
        team_one = new List<Player>();
        team_two = new List<Player>();
    }

    //public Queue(ref QueueBlock block, List<Player> players) {
    //    players_in_queue = new List<Player>();
    //    team_one = new List<Player>();
    //    team_two = new List<Player>();

    //    // Vote complete message contains the lobby id
    //    match_id = block.voting_complete_message.GetLobbyId();

    //    if (!this) {
    //        Console.WriteLine("\t[BuildQueue] Failed to get a match id! Message content = {0},\n title = {1}, type = {2}",
    //                          block.voting_complete_message.content, block.voting_complete_message.GetEmbeddedTitle(),
    //                          block.voting_complete_message.IsVotingCompleteMessage() ? "VotingCompleteMessage" : "Unknown");
    //        error_count++;
    //    }

    //    var i2 = 1;
    //    for (var i = 0; i < block.messages.Count - 1; i++) {
    //        var current_msg = block.messages[i];
    //        var next_msg = block.messages[i2];
    //        var current_id = current_msg.author.GetDiscordId();
    //        var index = Database.Database.GetIndexOfPlayer(ref players, current_id);

    //        if (current_msg.IsQMessage()) {
    //            if (next_msg.IsBotResponsePlayerJoinedMessage()) { players_in_queue.Add(players[index]); }
    //            else {
    //                Console.WriteLine("\t[BuildQueue] Player-bot message pattern broken for players_in_queue.Add with match id = {4}, current message = {0} (Index = {1}), and next message = {2} (Index = {3})",
    //                                  current_msg.content, i, next_msg.content, i2, match_id);
    //                error = true;
    //                break;
    //            }
    //        }
    //        else if (current_msg.IsLeaveMessage()) {
    //            if (next_msg.IsBotResponsePlayerLeftMessage()) { players_in_queue.Remove(players[index]); }
    //            else {
    //                Console.WriteLine("\t[BuildQueue] Player-bot message pattern broken for players_in_queue.Remove with match id = {4}, current message = {0} (Index = {1}), and next message = {2} (Index = {3})",
    //                                  current_msg.content, i, next_msg.content, i2, match_id);
    //                error = true;
    //                break;
    //            }
    //        }

    //        i2++;
    //    }
    //}

    public Queue(DiscordMessage join_msg, List<Player> players){
        var str_t1 = join_msg.GetTeamOneNames();
        var str_t2 = join_msg.GetTeamTwoNames();

        team_one = new List<Player>();
        team_two = new List<Player>();
        players_in_queue = new List<Player>();

        // Vote complete message contains the lobby id
        match_id = join_msg.GetLobbyId();

        if (str_t1 != null && str_t2 != null) {
            foreach (var p_name in str_t1) {
                foreach (Player p in players) {
                    bool found_name = false;
                    foreach (var name in p.recorded_names) {
                        if (name == p_name) {
                            team_one.Add(p);
                            players_in_queue.Add(p);
                            found_name = true;
                            break;
                        }
                    }

                    if (found_name) break;
                }
            }

            foreach (var p_name in str_t2) {
                foreach (Player p in players) {
                    bool found_name = false;
                    foreach (var name in p.recorded_names) {
                        if (name == p_name) {
                            team_two.Add(p);
                            players_in_queue.Add(p);
                            found_name = true;
                            break;
                        }
                    }

                    if (found_name) break;
                }
            }
        }

        if (players_in_queue.Count == 6) return;

        error = true;
        error_count++;

        if (team_one.Count < 3) {
            foreach (var n in str_t1) {
                bool player_found = false;
                foreach (var p in team_one) {
                    var temp = n;
                    if (p.HasName(ref temp)) {
                        player_found = true;
                        break;
                    }
                }

                if (!player_found) {
                    Console.WriteLine("[QueueConstructor] Could not find player record for {0}", n);
                }
            }
        }

        if (team_two.Count < 3) {
            foreach (var n in str_t2) {
                bool player_found = false;
                foreach (var p in team_two) {
                    var temp = n;
                    if (p.HasName(ref temp)) {
                        player_found = true;
                        break;
                    }
                }

                if (!player_found) {
                    Console.WriteLine("[QueueConstructor] Could not find player record for {0}", n);
                }
            }
        }
    }

    public List<Player> players_in_queue { get; set; }
    public List<Player> team_one         { get; set; }
    public List<Player> team_two         { get; set; }
    public int          match_id         { get; set; } = -1;

    public ScoreReportMessage result { get; set; }

    public bool team_one_won { get; set; } = false; // NOTE: team label not zero-indexed, team one is the first team and team two is the second
    public bool error        { get; set; }
    public static int  error_count  { get; set; } = 0;

    public static implicit operator bool(Queue queue) { return !queue.error && queue.match_id >= 0; }

    public static bool operator ==(Queue queue, int match_id) { return queue.match_id == match_id; }

    public static bool operator !=(Queue queue, int match_id) { return !(queue == match_id); }
    public bool TeamOneHasPlayer(ulong discord_id) {
        foreach (var player in team_one) {
            if (player.discord_id == discord_id) {
                return true;
            }
        }

        return false;
    }

    public bool TeamOneHasPlayer(string name) {
        foreach (var player in team_one) {
            if (player.HasName(ref name)) {
                return true;
            }
        }

        return false;
    }

    public bool TeamTwoHasPlayer(ulong discord_id) {
        foreach (var player in team_two) {
            if (player.discord_id == discord_id) {
                return true;
            }
        }

        return false;
    }

    public bool TeamTwoHasPlayer(string name) {
        foreach (var player in team_two) {
            if (player.HasName(ref name)) {
                return true;
            }
        }

        return false;
    }

    public void UpdatePlayerRecords() {
        if (team_one == null || team_two == null || team_one.Count < 3 || team_two.Count < 3 || !result.winner_set) {
            //log error
            return;
        }

        foreach (var player in team_one) { 
            
        }
    }
}