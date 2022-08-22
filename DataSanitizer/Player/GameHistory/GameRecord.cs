namespace Database.Player.GameHistory;

public class GameRecord {
    public GameRecord(Player player, DateTime record_to_this_date) {
        this.player = player;
        this.record_to_this_date = record_to_this_date;

        foreach (var record in player.game_history) {
            if (record.date_of_game > record_to_this_date) continue;
            if (record.won_game)
                wins++;
            else
                losses++;
        }
    }

    private Player   player              { get; }
    private DateTime record_to_this_date { get; }
    public  int      wins                { get; }
    public  int      losses              { get; }

    public double GetWinRateToDate() { return (double)wins / (wins + losses); }
}