using Database.Messages.ScoreReportMessage;

namespace Database.Player.GameHistory;


public class GameRecord {
    public GameRecord(Player player, ScoreReportMessage sr, DateTime timestamp, bool has_won) {
        record_owner = player;
        report = sr;
        match_timestamp = timestamp;
        won_queue = has_won;
    }
    public Player record_owner { get; set; }
    public    ScoreReportMessage report { get; set;}
    public DateTime match_timestamp { get; set; }
    public bool won_queue { get; set; } = false;
    public Database.Team team { get; set; }

    public List<Player> GetTeammates(Queue queue) {
        List<Player> teammates = new List<Player>();

        if (queue.TeamOneHasPlayer(record_owner.discord_id)){
            team = Database.Team.TEAM_ONE;
            foreach (var player in queue.team_one) {
                if (player.discord_id != record_owner.discord_id) {
                    teammates.Add(player);
                }
            }
        }

        else if (queue.TeamTwoHasPlayer(record_owner.discord_id)) {
            team = Database.Team.TEAM_TWO;
            foreach (var player in queue.team_two) {
                if (player.discord_id != record_owner.discord_id) {
                    teammates.Add(player);
                }
            }
        }

        return teammates;
    }
    
    public List<Player> GetOpponents(Queue queue) {
        return team == Database.Team.TEAM_ONE ? queue.team_two : queue.team_one;
    }
}