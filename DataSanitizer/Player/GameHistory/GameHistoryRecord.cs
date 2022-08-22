namespace Database.Player.GameHistory;
public class GameHistoryRecord
{
    public GameHistoryRecord(DateTime date_of_game, bool won_game)
    {
        this.date_of_game = date_of_game;
        this.won_game = won_game;
    }

    public DateTime date_of_game { get; }
    public bool won_game { get; }
}