using Microsoft.VisualBasic.CompilerServices;

namespace DataSanitizer;

public class Queue {
    public List<Player> players_in_queue { get; set; }
    public List<Player> team_one         { get; set; }
    public List<Player> team_two         { get; set; }
    public int          match_id         { get; set; } = -1;

    public bool team_one_won { get; set; } = false; // NOTE: team label not zero-indexed, team one is the first team and team two is the second
    public bool error { get; set; } = false;

    public Queue() {
        players_in_queue = new List<Player>();
        team_one = new List<Player>();
        team_two = new List<Player>();
    }

    public static implicit operator bool(Queue queue) {
        return !queue.error && queue.match_id >= 0;
    }

    public static bool operator ==(Queue queue, int match_id) {
        return queue.match_id == match_id;
    }

    public static bool operator !=(Queue queue, int match_id) {
        return !(queue == match_id);
    }
}

public class QueueBuilder {
    public static int errcount = 0;
    public static Queue BuildQueue(ref Database.QueueBlock block, ref List<Player> players) {
        var ret_queue = new Queue();
        
        // Vote complete message contains the lobby id
        ret_queue.match_id = block.voting_complete_message.GetLobbyId();

        if (!ret_queue) {
            Console.WriteLine("[BuildQueue] Failed to get a match id! Message content = {0},\n title = {1}, type = {2}",
                              block.voting_complete_message.content, block.voting_complete_message.embeds[0].title,
                              block.voting_complete_message.IsVotingCompleteMessage() ? "VotingCompleteMessage" : "Unknown");
            errcount++;
            return ret_queue;
        }

        var i2 = 1;
        for (var i = 0; i < block.messages.Count - 1; i++) {
            var current_msg = block.messages[i];
            var next_msg = block.messages[i2];
            var current_id = ulong.Parse(current_msg.author.id);
            var index = Database.GetIndexOfPlayer(ref players, current_id);

            if (current_msg.IsQMessage()) {
                if (next_msg.IsBotResponsePlayerJoinedMessage())
                    ret_queue.players_in_queue.Add(players[index]);
                else {
                    Console.WriteLine("[BuildQueue] Player-bot message pattern broken for players_in_queue.Add with match id = {4}, current message = {0} (Index = {1}), and next message = {2} (Index = {3})",
                                      current_msg.content, i, next_msg.content, i2, ret_queue.match_id);
                    ret_queue.error = true;
                    break;
                }
            }
            else if (current_msg.IsLeaveMessage()) {
                if (next_msg.IsBotResponsePlayerLeftMessage())
                    ret_queue.players_in_queue.Remove(players[index]);
                else {
                    Console.WriteLine("[BuildQueue] Player-bot message pattern broken for players_in_queue.Remove with match id = {4}, current message = {0} (Index = {1}), and next message = {2} (Index = {3})",
                                      current_msg.content, i, next_msg.content, i2, ret_queue.match_id);
                    ret_queue.error = true;
                    break;
                }
            }
            i2++;
        }
        return ret_queue;
    }
}