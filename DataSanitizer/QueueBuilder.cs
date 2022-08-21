namespace DataSanitizer;

public struct Queue {
    public List<Player> players_in_queue { get; set; } = new List<Player>();
    public List<Player> team_one         { get; set; } = new List<Player>();
    public List<Player> team_two         { get; set; } = new List<Player>();
    public int          match_id         { get; set; } = -1;

    public bool team_one_won { get; set; } = false; // NOTE: team label not zero-indexed, team one is the first team and team two is the second
    public bool error { get; set; } = false;

    public Queue() {}

    public static implicit operator bool(Queue queue) {
        return !queue.error && queue.match_id >= 0;
    }
}

public class QueueBuilder {
    public static Queue BuildQueue(ref Database.QueueBlock block, ref List<Player> players) {
        Queue ret_queue = new Queue();

        // Last message is always the vote complete message, which contains the lobby id
        ret_queue.match_id = block.messages[block.messages.Count - 1].GetLobbyId();

        if(!ret_queue) {
            Console.WriteLine("[BuildQueue] Failed to get a match id!");
            return ret_queue;
        }
        
        var i2 = 1;
        for (var i = 0; i < block.messages.Count - 1; i++) {
            var current_msg = block.messages[i];
            var next_msg = block.messages[i2];
            var current_id = ulong.Parse(current_msg.author.id);
            var index = Database.GetIndexOfPlayer(ref players, current_id);

            if (current_msg.IsQMessage()) {
                if(next_msg.IsBotResponsePlayerJoinedMessage()) 
                    ret_queue.players_in_queue.Add(players[index]);
                else {
                    Console.WriteLine("[BuildQueue] Player-bot message pattern broken for players_in_queue.Add with match id = {4}, current message = {0} (Index = {1}), and next message = {2} (Index = {3})",
                                      current_msg.content, i, next_msg.content, i2, ret_queue.match_id);
                    ret_queue.error = true;
                    break;
                }                
            }
            else if (current_msg.IsLeaveMessage()) {
                if(next_msg.IsBotResponsePlayerLeftMessage()) 
                    ret_queue.players_in_queue.Remove(players[index]);
                else {
                    Console.WriteLine("[BuildQueue] Player-bot message pattern broken for players_in_queue.Remove with match id = {4}, current message = {0} (Index = {1}), and next message = {2} (Index = {3})",
                                      current_msg.content, i, next_msg.content, i2, ret_queue.match_id);
                    ret_queue.error = true;
                    break;
                }
            }
            else if (current_msg.IsLobbyMessage()) {
                // send message to function that searches for the queue by match id and sets the teams
            }
            // else if lobby canceled message?


            i2++;
        }

        

        return ret_queue;
    }
}