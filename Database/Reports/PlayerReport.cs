using Database.Database.DatabaseCore;
using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Interfaces;
using Database.Reports;

public class PlayerReport : IReport {
    private ILogger logger {get; set;}
    public PlayerReport() {
        logger = this as ILogger;
    }
    public void GenerateReport(in string path) {
        var players   = DDatabaseCore.GetSingleton().all_players;
        if (players == null) {
            logger.Log("all_players null! Need to build database!");
            return;
        }

        try {

            var time   = DateTime.Now;
            var writer = new StreamWriter(path + time.ToString() + ".csv");
            foreach (var player in players) {
                if (player == null) {
                    logger.Log("Null player in all_players!");
                    continue;
                }


            }
        }
        catch (Exception ex) {
            logger.Log($"Caught exception: {ex.Message}");
        }
    }

    public string ParseComponent<ComponentType>(ComponentType? abstract_player) where ComponentType : IDatabaseComponent {
        var    player = abstract_player as DPlayer;
        string line = "";
        line += player.discord_id + ",";
        foreach (var name in player.recorded_names) {
            line += name + " ";
        }
        line.Trim();
        line += player.iTotalWins + ",";
        line += player.iTotalLosses + ",";
        foreach (var match in player.game_history) {

        }
        return line;
    }
}