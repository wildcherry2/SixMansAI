using Database.Messages.DiscordMessage;
using static System.Reflection.Metadata.BlobBuilder;

namespace Database;

public partial class Database {
    // Removes queue blocks with erroneous !q commands, as indicated by the counter
    private void RemoveInvalidBlocks(ref List<QueueBlock> blocks) {
        foreach (var block in blocks)
            if (block.counter != 6) {
                Console.WriteLine("\t\t[RemoveInvalidBlocks] Removed queue block with counter = {0}", block.counter);
                blocks.Remove(block);
                RemoveInvalidBlocks(ref blocks);
                break;
            }
    }

    // Removes queue blocks with bot errors (doesn't register !q or !leave or responds to multiple commands consecutively)
    private void RemoveNonConsecutiveQueues(ref List<QueueBlock> blocks) {
        for (var b = 0; b < blocks.Count; b++) {
            var queue_block = blocks[b];
            var i2 = 1;
            if (queue_block.messages.Count > 1)
                for (var i = 0; i < queue_block.messages.Count; i++) {
                    //var queue_block = block;
                    if (i2 > queue_block.messages.Count - 1) break;

                    var current_msg = queue_block.messages[i];

                    //if(current_msg.IsJoinGameMessage() || current_msg.IsVotingCompleteMessage()) {
                    //    i2++;
                    //    continue;
                    //}
                    //if (current_msg.IsVotingCompleteMessage()) break;

                    var this_msg = current_msg.author.IsHumanMessage()
                                       ? queue_block.messages[i].content
                                       : current_msg.IsBotResponsePlayerJoinedMessage()
                                           ? "Player joined"
                                           : "Player left";

                    current_msg = queue_block.messages[i2];
                    var next_msg = current_msg.author.IsHumanMessage()
                                       ? queue_block.messages[i2].content
                                       : current_msg.IsBotResponsePlayerJoinedMessage()
                                           ? "Player joined"
                                           : "Player left";

                    if ((this_msg == "!q" && next_msg != "Player joined") ||            // If this message is a !q command and the next message isn't a player joined response OR
                        (this_msg == "!leave" && next_msg != "Player left") ||          // If this message is a !leave command and the next message isn't a player left response OR
                        ((this_msg == "Player joined" || this_msg == "Player left") &&  // If this message is a bot message and the next message is also a bot message
                         (next_msg == "Player joined" || next_msg == "Player left"))) { // Remove this queue block because the bot missed some things and we won't be able to correlate link names to discord names
                      
                        Console.WriteLine("\t\t[RemoveNonConsecutiveQueues] Removed queue block with out of order messages at index {0}", b);
                        blocks.RemoveAt(b);

                        // Since the size of the list has shrunk by 1 and the old blocks[b] is gone and b now refers to the next element, keep b the same
                        b--;

                        break;
                    }

                    i2++;
                }
        }
    }

    private void RemoveUnusableBlocks() {
        int total_removed = 0;
        int pre_count = queue_blocks.Count;

        Console.WriteLine("\t[GetQueueBlocks] Removing invalid blocks...");

        Console.ForegroundColor = ConsoleColor.Magenta;
        RemoveInvalidBlocks(ref queue_blocks);
        total_removed = (pre_count - queue_blocks.Count);
        pre_count = queue_blocks.Count;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\t[GetQueueBlocks] Removed " + total_removed + " invalid blocks");

        Console.ForegroundColor = ConsoleColor.Magenta;
        RemoveNonConsecutiveQueues(ref queue_blocks);
        total_removed += (pre_count - queue_blocks.Count);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\t[GetQueueBlocks] Removed " + (pre_count - queue_blocks.Count) + " non-consecutive queue blocks");

        Console.WriteLine("\t[GetQueueBlocks] {0} blocks validated, {1} blocks removed", queue_blocks.Count, total_removed);
        Console.ForegroundColor = ConsoleColor.Green;
    }

    private List<QueueBlock> GetQueueBlocks(ref List<DiscordMessage> list) {
        var blocks = new List<QueueBlock>();
        var current_queue_block = new QueueBlock();

        // Control variable to make loop skip ahead to beginning of a new block (data likely starts with a partial block)
        var bFoundStart = false;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\t[GetQueueBlocks] Getting queue blocks for current file...");

        foreach (var message in list) {
            // After a voting complete message, players are free to start a new queue, so the current QueueBlock should be added to the list and a new QueueBlock should be instantiated
            if (message.IsVotingCompleteMessage()) {
                // If this message comes after a full queue block, add it to the output list
                if (bFoundStart) {
                    current_queue_block.voting_complete_message = message;
                    blocks.Add(current_queue_block);
                    current_queue_block = new QueueBlock();
                }

                // Else set control variable to true to indicate the beginning of the first block has been reached
                else {
                    bFoundStart = true;
                }
            }

            // If the message isn't part of a partial queue block and it isn't a voting complete message, add it to the current QueueBlock
            else if (bFoundStart) {
                // QueueBlock.counter represents the amount of !q commands in relation to other messages; increment on !q, decrement on !leave
                // The result is that queue blocks with more/less than 6 responses to !q commands can be checked and invalidated, since the bot didn't get the data correctly
                if (message.IsQMessage()) {
                    current_queue_block.counter++;
                    current_queue_block.messages.Add(message);
                }
                else if (message.IsLeaveMessage()) {
                    current_queue_block.counter--;
                    current_queue_block.messages.Add(message);
                }
                else if (message.IsJoinGameMessage()) {
                    current_queue_block.teams_decided_messages.Add(message);
                }
                else if (message.IsBotResponsePlayerJoinedMessage() ||
                         message.IsBotResponsePlayerLeftMessage()) {
                    current_queue_block.messages.Add(message);
                }
            }
        }

        Console.WriteLine("\t[GetQueueBlocks] Parsed " + blocks.Count + " queue blocks before error filtering");
        return blocks;
    }

    // Transforms QueueBlocks (which are essentially collections of relevant chat messages) into Queue structs that organize the queue data
    private void ParseQueueBlocks() {
        Console.ForegroundColor = ConsoleColor.Red;
        int qcount = 0;
        for (int i = 0; i < queue_blocks.Count; i++) {
            var current_block = queue_blocks[i];
            var current_queue = new global::Queue(ref current_block, players);

            if (current_queue) {
                Console.WriteLine("[ParseQueueBlocks] Queue constructed with match id = {6} and players \n{0},\n{1},\n{2},\n{3},\n{4},\n{5}",
                                  current_queue.players_in_queue[0].recorded_names[0], current_queue.players_in_queue[1].recorded_names[0], current_queue.players_in_queue[2].recorded_names[0],
                                  current_queue.players_in_queue[3].recorded_names[0] ,current_queue.players_in_queue[4].recorded_names[0], current_queue.players_in_queue[5].recorded_names[0], 
                                  current_queue.match_id);
                queues.Add(current_queue);
                qcount++;
            }
            //UpdateTeamDecisions(ref current_block);
        }
        UpdateTeamDecisions();
        Console.WriteLine("[ParseQueueBlocks] Assigned {0} teams to queues, {1} queues are unassigned!", qcount, queue_blocks.Count - qcount);
        Console.ForegroundColor = ConsoleColor.White;
    }

    /*  TODO: THIS WILL NOT WORK YET DUE TO REFACTORS  */
    private void CleanupChatFile() {
        try {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[CleanupChat] Opening \"" + chat_path_s + "\"...");
            in_reader = File.OpenText(chat_path_s);

            Console.WriteLine("[CleanupChat] Reading \"" + chat_path_s + "\" into JSON object...");
            var chat = DiscordMessage.RawDataToDiscordMessage(ref in_reader);
            var chat_messages = chat.raw_messages;
            Console.WriteLine("[CleanupChat] Done reading \"" + chat_path_s + "\" into JSON object...");
            queue_blocks = new List<QueueBlock>();
            if (chat != null)
                queue_blocks = GetQueueBlocks(ref chat_messages);

            Console.WriteLine("[CleanupChat] Valid queues in file: " + queue_blocks.Count);
            Console.WriteLine("[CleanupChat] Closing file \"" + chat_path_s + "\"...");
            in_reader.Close();
            Console.ForegroundColor = ConsoleColor.White;
        }
        catch (Exception ex) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}