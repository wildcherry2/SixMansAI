using System.Text.Json;
using Database.Messages.ScoreReportMessage;

namespace Database;

public partial class Database {
    private  StreamReader             in_reader = null!;
    private  List<QueueBlock>         queue_blocks;
    private  List<Queue>              queues;
    private  List<ScoreReportMessage> sr_messages;

    public Database() {
        queue_blocks = new List<QueueBlock>();
        sr_messages = new List<ScoreReportMessage>();
        queues = new List<Queue>();
        players = new List<Player.Player>();
    }

    public Database(string sr_path_s, string chat_path_s) {
        queue_blocks = new List<QueueBlock>();
        sr_messages = new List<ScoreReportMessage>();
        queues = new List<Queue>();
        players = new List<Player.Player>();
        this.sr_path_s = sr_path_s;
        this.chat_path_s = chat_path_s;
    }

    public List<Player.Player> players     { get; set; }
    public string              sr_path_s   { get; set; } = "";
    public string              chat_path_s { get; set; } = "";

    /*  TODO: A better algorithm for score gathering: get all names from all raw data files, then independently get all join vc messages and lookup by name. Or do this only for error queues to sew what's salvageable  */
    public void BuildDatabase() {
        //CleanupScoreReportFile();
        CleanupChatFile();
        
        RemoveUnusableBlocks();
        RegisterPlayerNames();

        var options = new JsonSerializerOptions { WriteIndented = true };
        Console.WriteLine(JsonSerializer.Serialize(players[0], options));
        //ParseQueueBlocks();
        //SetMatchResults();
        // validate queues have 6 players before scanning
        //SetPlayerRecords();

        //validate final objects are valid
    }
}