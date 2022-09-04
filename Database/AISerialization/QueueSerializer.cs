using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Enums;

namespace Database.AISerialization; 

public static class QueueSerializer {
    public static List<MiniQueue> mqueues { get; set; } = new List<MiniQueue>();


    // Format: {Player,Player,...},Winner\n
    // Player = Season_Wins_Up_Until_Now Season_Losses_Up_Until_Now Total_Wins Total_Losses
    // Winner = 0 if winner is team one, 1 if team two
    // Ex: {6 7 16 23,11 22 54 71,0 0 2 2,13 22 14 25,27 33 68 90,7 5 52 48},1
    // NOTE: Players... are in logical order (team one players then team two players, each in order)
    // TODO: change the player serializer to get total wins/losses up until now, instead of total given all of the data
    public static string GetQueueString(in DQueue queue) {
        if (!QueueValidator.IsQueueValid(queue)) { return ""; }

        var ret_val = "";
        ret_val += SerializeTeams(queue) + ",";
        ret_val += SerializeWinner(queue);

        return ret_val;
    }

    private static string SerializeWinner(in DQueue queue) { return queue.winner == ETeamLabel.TEAM_ONE ? "0" : "1"; }

    // TODO: check for subs
    private static string SerializeTeams(in DQueue queue) {
        var       ret_val    = "";
        var       queue_time = queue.teams_picked_message.timestamp;
        MiniQueue mq         = new MiniQueue();
        ret_val += $"{PlayerSerializer.SerializePlayer(queue.team_one.player_one!, queue_time!.Value, EPlayerNumber.ONE, ref mq)},";
        ret_val += $"{PlayerSerializer.SerializePlayer(queue.team_one.player_two!, queue_time!.Value, EPlayerNumber.TWO, ref mq)},";
        ret_val += $"{PlayerSerializer.SerializePlayer(queue.team_one.player_three!, queue_time!.Value, EPlayerNumber.THREE, ref mq)},";
        ret_val += $"{PlayerSerializer.SerializePlayer(queue.team_two.player_one!, queue_time!.Value, EPlayerNumber.FOUR, ref mq)},";
        ret_val += $"{PlayerSerializer.SerializePlayer(queue.team_two.player_two!, queue_time!.Value, EPlayerNumber.FIVE, ref mq)},";
        ret_val += $"{PlayerSerializer.SerializePlayer(queue.team_two.player_three!, queue_time!.Value, EPlayerNumber.SIX, ref mq)}";
        //ret_val += "";
        mqueues.Add(mq);
        return ret_val;
    }
}

public class MiniQueue {
    public int one_wins,   two_wins,   three_wins,   four_wins,   five_wins,   six_wins;
    public int one_losses, two_losses, three_losses, four_losses, five_losses, six_losses;
}