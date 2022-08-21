using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using DataSanitizer.ScoreReport;
using static DiscordMessage;

namespace DataSanitizer;

public class Database {
    private readonly List<ScoreReportMessage> sr_messages;
    private          StreamReader             in_reader = null!;
    private          List<QueueReadyMessage>  qr_messages;
    private          List<QueueBlock>         queue_blocks;

    public List<Player> players { get; set; }

    public string sr_path_s   { get; set; } = "";
    public string chat_path_s { get; set; } = "";

    public Database() {
        queue_blocks = new List<QueueBlock>();
        sr_messages = new List<ScoreReportMessage>();
        qr_messages = new List<QueueReadyMessage>();
        players = new List<Player>();
    }

    public Database(string sr_path_s, string chat_path_s) {
        queue_blocks = new List<QueueBlock>();
        sr_messages = new List<ScoreReportMessage>();
        qr_messages = new List<QueueReadyMessage>();
        players = new List<Player>();
        this.sr_path_s = sr_path_s;
        this.chat_path_s = chat_path_s;
    }

    public static int GetIndexOfPlayer(ref List<Player> players, ulong discord_id) {
        for (int i = 0; i < players.Count; i++) {
            if (players[i].discord_id == discord_id) {
                return i;
            }
        }

        return -1;
    }

    private void FormatScoreReportList(ref List<Message> list) {
        foreach (var message in list) {
            // If the content is empty, remove the message and go next

            #region ContentLengthCheck

            if (message.content.Length == 0) {
                Console.WriteLine("\t[FormatScoreReportList] Removed empty message...");
                list.Remove(message);
                FormatScoreReportList(ref list);
                break;
            }

            #endregion

            // If there isn't a ✅ reaction, the match wasn't recorded in the database, so remove and go next

            #region MatchVerified

            var is_valid = false;
            foreach (var reaction in message.reactions)
                if (reaction.emojis.name.Contains("✅")) {
                    is_valid = true;
                    break;
                }

            if (!is_valid) {
                Console.WriteLine("\t[FormatScoreReportList] Removed unverified score report...");
                list.Remove(message);
                FormatScoreReportList(ref list);
                break;
            }

            #endregion

            // If it passed the previous if statements it's a valid match report, so remove newlines and unnecessary whitespace

            #region CleanupString

            message.content = message.content.Replace("\n", " ");
            var options = RegexOptions.None;
            var regex = new Regex("[ ]{2,}", options);
            message.content = regex.Replace(message.content, " ");

            #endregion
        }
    }

    private void CleanupScoreReportFile() {
        try {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[CleanupScoreReportFile] Opening \"" + sr_path_s + "\"...");
            in_reader = File.OpenText(sr_path_s);
            Console.WriteLine("[CleanupScoreReportFile] Reading \"" + sr_path_s + "\" into JSON object...");
            var score_report = ScoreReportHelpers.RawDataToDiscordMessage(ref in_reader);
            Console.WriteLine("[CleanupScoreReportFile] Done reading \"" + sr_path_s + "\" into JSON object...");
            if (score_report != null) {
                var messages = score_report.messages;
                int pre_count = score_report.messages.Count;
                Console.WriteLine("[CleanupScoreReportFile] Parsed " + pre_count + " messages before error filtering");
                //Console.WriteLine("[CleanupScoreReportFile] Cleaning and formatting \"" + sr_path_s + "\"...");
                Console.ForegroundColor = ConsoleColor.Yellow;
                FormatScoreReportList(ref messages);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[CleanupScoreReportFile] Removed " + (pre_count - score_report.messages.Count) +
                                  " erroneous score reports, " + score_report.messages.Count + " reports are valid");
                Console.WriteLine("[CleanupScoreReportFile] Serializing {0} reports to ScoreReportMessage objects...",
                                  score_report.messages.Count);
                foreach (var message in score_report.messages) sr_messages.Add(new ScoreReportMessage(message));
                Console.WriteLine("[CleanupScoreReportFile] Done serializing reports...");
            }

            Console.WriteLine("[CleanupScoreReportFile] Closing file \"{0}\"", sr_path_s);
            in_reader.Close();
        }
        catch (Exception ex) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    private void CleanupChatFile() {
        try {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[CleanupChat] Opening \"" + chat_path_s + "\"...");
            in_reader = File.OpenText(chat_path_s);

            Console.WriteLine("[CleanupChat] Reading \"" + chat_path_s + "\" into JSON object...");
            var chat = ScoreReportHelpers.RawDataToDiscordMessage(ref in_reader);
            var chat_messages = chat.messages;
            Console.WriteLine("[CleanupChat] Done reading \"" + chat_path_s + "\" into JSON object...");
            //var queues = new List<QueueBlock>();
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
                    var this_msg = !QueueReadyMessage.IsBotMessage(ref current_msg)
                                       ? queue_block.messages[i].content
                                       : QueueReadyMessage.IsBotResponsePlayerJoinedMessage(ref current_msg)
                                           ? "Player joined"
                                           : "Player left";

                    current_msg = queue_block.messages[i2];
                    var next_msg = !QueueReadyMessage.IsBotMessage(ref current_msg)
                                       ? queue_block.messages[i2].content
                                       : QueueReadyMessage.IsBotResponsePlayerJoinedMessage(ref current_msg)
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

    private List<QueueBlock> GetQueueBlocks(ref List<Message> list) {
        var blocks = new List<QueueBlock>();
        var current_queue_block = new QueueBlock();

        // Control variable to make loop skip ahead to beginning of a new block (data likely starts with a partial block)
        var bFoundStart = false;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\t[GetQueueBlocks] Getting queue blocks for current file...");

        //var span = CollectionsMarshal.AsSpan(list);

        for (int i = 0; i < list.Count; i++) {
            //ref Message message = ref span[i];
            var message = list[i];

            // After a voting complete message, players are free to start a new queue, so the current QueueBlock should be added to the list and a new QueueBlock should be instantiated
            if (QueueReadyMessage.IsVotingCompleteMessage(ref message)) {
                // If this message comes after a full queue block, add it to the output list
                if (bFoundStart) {
                    current_queue_block.messages.Add(message);
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
                if (QueueReadyMessage.IsQMessage(ref message)) {
                    current_queue_block.counter++;
                    current_queue_block.messages.Add(message);
                }
                else if (QueueReadyMessage.IsLeaveMessage(ref message)) {
                    current_queue_block.counter--;
                    current_queue_block.messages.Add(message);
                }
                else if (QueueReadyMessage.IsBotResponsePlayerJoinedMessage(ref message) ||
                         QueueReadyMessage.IsBotResponsePlayerLeftMessage(ref message)) { // OR with isLobbyMessage as well??
                    current_queue_block.messages.Add(message);
                }
            }
        }

        int pre_count = blocks.Count;
        int total_removed = 0;

        Console.WriteLine("\t[GetQueueBlocks] Parsed " + pre_count + " queue blocks before error filtering");
        Console.WriteLine("\t[GetQueueBlocks] Removing invalid blocks...");

        Console.ForegroundColor = ConsoleColor.Magenta;
        RemoveInvalidBlocks(ref blocks);
        total_removed = (pre_count - blocks.Count);
        pre_count = blocks.Count;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\t[GetQueueBlocks] Removed " + total_removed + " invalid blocks");

        Console.ForegroundColor = ConsoleColor.Magenta;
        RemoveNonConsecutiveQueues(ref blocks);
        total_removed += (pre_count - blocks.Count);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\t[GetQueueBlocks] Removed " + (pre_count - blocks.Count) + " non-consecutive queue blocks");

        Console.WriteLine("\t[GetQueueBlocks] {0} blocks validated, {1} blocks removed", blocks.Count, total_removed);
        Console.ForegroundColor = ConsoleColor.Green;
        return blocks;
    }

    private Player LookupAndTryInsertPlayer(ref Message message) {
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
                return player;
            }
        }

        var new_player = new Player(name, auth_id, nick);
        Console.WriteLine("\t[LookupAndTryInsertPlayer] Creating new Player with name = {0}, nick = {1}, id = {2}", name, nick, auth_id);
        players.Add(new_player);

        Console.ForegroundColor = ConsoleColor.Green;
        return new_player;
    }

    private void RegisterPlayerNames() {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[RegisterPlayerNames] Registering player names...");
        foreach (var block in queue_blocks) {
            Player current_player = new Player();
            for (int i = 0; i < block.messages.Count; i++) {
                var message = block.messages[i];
                if (QueueReadyMessage.IsQMessage(ref message) || QueueReadyMessage.IsLeaveMessage(ref message)) {
                    current_player = LookupAndTryInsertPlayer(ref message);
                }
                else if(message.IsBot()){
                    string linkname = message.embeds[0].description.Substring(1, (message.embeds[0].description.IndexOf(']')) - 1);
                    if (!current_player.HasName(ref linkname)) {
                        Console.WriteLine("\t[RegisterPlayerNames] Registered linkname " + linkname + " for " + current_player.discord_id);
                        current_player.recorded_names.Add(linkname);
                    }
                }
            }
        }
        Console.ForegroundColor = ConsoleColor.White;
    }

    private void SetPlayerRecords() {

    }

    public void BuildDatabase() {
        CleanupScoreReportFile();
        CleanupChatFile();
        RegisterPlayerNames();
        SetPlayerRecords();
    }

    // Join, leave, !q, and !leave messages between the first queue and the next voting complete message
    public struct QueueBlock {
        public List<Message> messages = new();
        public int           counter  = 0;
        public QueueBlock() { }
    }
}