using Database.AISerialization;
using Database.Database.DatabaseCore;
using MathNet.Numerics.Statistics;

namespace Database; 

public class MainClass {
    public static void Main(string[] args) {
        var core = DDatabaseCore.GetSingleton();
        core.BuildDatabase(true);
        AISerializerCore.GetSingleton().Serialize(@"C:\Users\tyler\Documents\Programming\AI\SixMans\Reports");
        PostCalcs();
        

    }

    public static void PostCalcs() {
        var       post  = QueueSerializer.mqueues;
        var       count = 0;
        const int query = 1;
        Console.ForegroundColor = ConsoleColor.White;
        foreach (var q in post) {
            if (q.one_wins + q.one_losses < query ||
                q.two_wins + q.two_losses < query ||
                q.three_wins + q.three_losses < query ||
                q.four_wins + q.four_losses < query ||
                q.five_wins + q.five_losses < query ||
                q.six_wins + q.six_losses < query)
                continue;

            count++;
            var samples = new List<double>();
            samples.Add(q.one_wins - q.one_losses);
            samples.Add(q.two_wins - q.two_losses);
            samples.Add(q.three_wins - q.three_losses);
            samples.Add(q.four_wins - q.four_losses);
            samples.Add(q.five_wins - q.five_losses);
            samples.Add(q.six_wins - q.six_losses);
            var sd       = samples.StandardDeviation();
            var mean     = samples.Mean();
            var z_scores = new List<double>();
            z_scores.Add(CalculateZScore(samples[0], mean, sd));
            z_scores.Add(CalculateZScore(samples[1], mean, sd));
            z_scores.Add(CalculateZScore(samples[2], mean, sd));
            z_scores.Add(CalculateZScore(samples[3], mean, sd));
            z_scores.Add(CalculateZScore(samples[4], mean, sd));
            z_scores.Add(CalculateZScore(samples[5], mean, sd));
            double t1_bias = 0.0, t2_bias = 0.0;

            GetBiases(ref t1_bias, ref t2_bias, ref z_scores);
            //Console.WriteLine($"One: {z_scores[0]} ({samples[0]}), Two: {z_scores[1]} ({samples[1]}), Three: {z_scores[2]} ({samples[2]})," +
            //                  $"\nFour: {z_scores[3]} ({samples[3]}), Five: {z_scores[4]} ({samples[4]}), Six: {z_scores[5]} ({samples[5]})\n");
        }

        //decimal sum                     = 0;
        //foreach (var val in avgsk) { Console.WriteLine($"sum = {sum}"); sum += (decimal)val; }

        //Console.WriteLine($"Average skill gap (in deviations) = {sum}");
    }

    private static double CalculateZScore(in double value, in double sample_mean, in double sd) {
        return (value - sample_mean) / sd;
    }

    public static List<decimal> avgsk = new List<decimal>();
    private static void GetBiases(ref double t1_out, ref double t2_out, ref List<double> z_scores) {
        var t1_avg = new List<double>() {
            z_scores[0],
            z_scores[1],
            z_scores[2]
        }.Mean();
        var t2_avg = new List<double>() {
            z_scores[3],
            z_scores[4],
            z_scores[5]
        }.Mean();

        var skill_gap = t1_avg - t2_avg;
        Console.WriteLine($"Queue with skill gap = {skill_gap}");
        
        // double - int floored version to separate fractals, keep fractals below 1 by using the same method after adding to it
        if (skill_gap > 0) {
            // Team one is higher rated than team two
        }

        else if (skill_gap < 0) {
            // Team two is higher rated than team one
        }

        else {
            t1_out = 0.0;
            t2_out = 0.0;
        }
    }
}