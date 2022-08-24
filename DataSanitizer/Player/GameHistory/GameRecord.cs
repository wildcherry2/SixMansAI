using Database.Messages.ScoreReportMessage;

namespace Database.Player.GameHistory;


public class GameRecord {
    public GameRecord() {

    }
    public Player record_owner { get; set; }
    public    ScoreReportMessage report { get; set;}
    public DateTime match_timestamp { get; set; }
    public bool won_queue { get; set; } = false;

    public List<Player> GetTeammates(Queue queue) {
        List<Player> teammates = new List<Player>();

        if (queue.TeamOneHasPlayer(record_owner.discord_id)){
            foreach (var player in queue.team_one) {
                if (player.discord_id != record_owner.discord_id) {
                    teammates.Add(player);
                }
            }
        }

        else if (queue.TeamTwoHasPlayer(record_owner.discord_id)) {
            foreach (var player in queue.team_two) {
                if (player.discord_id != record_owner.discord_id) {
                    teammates.Add(player);
                }
            }
        }

        return teammates;
    }
    
}