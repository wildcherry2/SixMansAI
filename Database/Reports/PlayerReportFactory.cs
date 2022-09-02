using Database.Database.DatabaseCore;
using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Interfaces;
using Database.Reports;

// Generates AI training data for players
public class PlayerReportFactory : IReport {
    private ILogger             logger    {get;  set;}
    private static PlayerReportFactory singleton { get; set; }
    private PlayerReportFactory() {
        logger = this as ILogger;
    }
    public static PlayerReportFactory GetSingleton() {
        if (singleton == null) singleton = new PlayerReportFactory();
        return singleton;
    }
    public void GenerateReport(in string path) {
        var players   = DDatabaseCore.GetSingleton().all_players;
        if (players == null) {
            logger.Log("all_players null! Need to build database!");
            return;
        }

        try {
            var time       = DateTime.Now;
            var final_path = $"{path}\\player.csv";
            //File.Create(final_path);
            var writer     = new StreamWriter(final_path);
            foreach (var player in players) {
                if (player == null) {
                    logger.Log("Null player in all_players!");
                    continue;
                }

                writer.WriteLine(ParseComponent(player));
            }
            writer.Close();
        }
        catch (Exception ex) {
            logger.Log($"Caught exception: {ex.Message}");
        }
    }

    // CSV: discord_id,iTotalWins,iTotalLosses,RECORDS...
    // RECORD = team bPlayerWon queue.primary_key
    // team = 0 for team one, 1 for team two, 2 for error
    public string ParseComponent<ComponentType>(ComponentType? abstract_player) where ComponentType : IDatabaseComponent {
        var    player = abstract_player as DPlayer;
        string line = "";
        line += player.discord_id + ",";
        line += player.iTotalWins + ",";
        line += player.iTotalLosses + ",";
        foreach (var record in player.game_history) {
            if (record == null) {
                logger.Log($"Found null record in history for {player.recorded_names[0]} ({player.discord_id})!");
                continue;
            }

            line += $"{(int)record.team} {record.bPlayerWon} {record.queue.TryGetOrCreatePrimaryKey().key},";
        }

        line = line.Trim(',');
        return line;
    }
}