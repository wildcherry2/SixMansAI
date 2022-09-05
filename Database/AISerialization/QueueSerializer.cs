using System.Reflection.Metadata.Ecma335;
using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Enums;
using MathNet.Numerics.Statistics;

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
        ret_val += SerializeTeams(queue);
        if (ret_val.Length > 0) {
            ret_val += "," + SerializeWinner(queue);

            return ret_val;
        }

        return "";
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
        ret_val += $"{PlayerSerializer.SerializePlayer(queue.team_two.player_three!, queue_time!.Value, EPlayerNumber.SIX, ref mq)},";
        //ret_val += "";
        if (IsGoodQueue(mq)) {
            mqueues.Add(mq);
            var begin = PlayerSerializer.seasons[PlayerSerializer.GetSeasonFromTime(queue_time.Value)].after;
            return ret_val + $"{(queue_time.Value - begin).Days}";
        }
        else
            return "";
    }

    public static string PostCalcs(in MiniQueue q) {
        const int query = 1;
        Console.ForegroundColor = ConsoleColor.White;

            if (!IsGoodQueue(q))
                return "";

            var samples = new List<double>();
            samples.Add(q.one_wins - q.one_losses);
            samples.Add(q.two_wins - q.two_losses);
            samples.Add(q.three_wins - q.three_losses);
            samples.Add(q.four_wins - q.four_losses);
            samples.Add(q.five_wins - q.five_losses);
            samples.Add(q.six_wins - q.six_losses);
            //var sd       = samples.StandardDeviation();
            //var mean     = samples.Mean();
            //var z_scores = new List<Int64>();
            //z_scores.Add(CalculateZScore(samples[0], mean, sd));
            //z_scores.Add(CalculateZScore(samples[1], mean, sd));
            //z_scores.Add(CalculateZScore(samples[2], mean, sd));
            //z_scores.Add(CalculateZScore(samples[3], mean, sd));
            //z_scores.Add(CalculateZScore(samples[4], mean, sd));
            //z_scores.Add(CalculateZScore(samples[5], mean, sd));
            //Int64 t1_bias = 0, t2_bias = 0;

            //GetBiases(ref t1_bias, ref t2_bias, ref samples);
            //const double scale = 1;
            //const int    trunc = 15;
            return ($"{samples[0]},{samples[1]},{samples[2]}," +
                    $"{samples[3]},{samples[4]},{samples[5]}");

    }

    //private static Int64 CalculateZScore(in double value, in double sample_mean, in double sd) {
    //    var res = (value - sample_mean) / sd;
    //    return (Int64)((res == 0 ? res + 1e-6 : res) * 1e14);
    //}
    //private static void GetBiases(ref Int64 t1_out, ref Int64 t2_out, ref List<double> z_scores) {
    //    var t1_avg = new List<double>() {
    //        z_scores[0],
    //        z_scores[1],
    //        z_scores[2]
    //    }.Mean();
    //    var t2_avg = new List<double>() {
    //        z_scores[3],
    //        z_scores[4],
    //        z_scores[5]
    //    }.Mean();

    //    var skill_gap = t1_avg - t2_avg;
    //    //Console.WriteLine($"Queue with skill gap = {skill_gap}");
    //    var res = (skill_gap * skill_gap) / 8.0;
    //    // double - int floored version to separate fractals, keep fractals below 1 by using the same method after adding to it
    //    if (skill_gap > 0) {
    //        // Team one is higher rated than team two
    //        t1_out = (Int64)((res == 0 ? res + 1e-6 : res) * 1e14);
    //        t2_out = (Int64)(1e-6 * 1e14);
    //    }

    //    else if (skill_gap < 0) {
    //        // Team two is higher rated than team one
    //        t2_out = (Int64)((res == 0 ? res + 1e-6 : res) * 1e14);
    //        t1_out = (Int64)(1e-6 * 1e14);
    //    }

    //    else {
    //        t1_out = (Int64)(1e-6 * 1e14);
    //        t2_out = (Int64)(1e-6 * 1e14);
    //    }
    //}

    private static bool IsGoodQueue(in MiniQueue q) {
        var zero_count = 0;

        if(q.one_wins + q.one_losses <= 1) zero_count++;
        if(q.two_wins + q.two_losses <= 1) zero_count++;
        if(q.three_wins + q.three_losses <= 1) zero_count++;
        if(q.four_wins + q.four_losses <= 1) zero_count++;
        if(q.five_wins + q.five_losses <= 1) zero_count++;
        if(q.six_wins + q.six_losses <= 1) zero_count++;

        return zero_count <= 1;
    }
}

public class MiniQueue {
    public int one_wins,   two_wins,   three_wins,   four_wins,   five_wins,   six_wins;
    public int one_losses, two_losses, three_losses, four_losses, five_losses, six_losses;
}