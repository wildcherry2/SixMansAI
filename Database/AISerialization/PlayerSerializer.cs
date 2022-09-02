using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Structs;

namespace Database.AISerialization; 

public static class PlayerSerializer {
    public static string SerializePlayer(in DPlayer player, in DateTime queue_time) {
        var season_wins = 0;
        var season_losses = 0;

        foreach (var record in player.game_history) {
            if (IsGameInSeasonSoFar(record, GetSeasonFromTime(queue_time), queue_time))
                SerializeRecord(record, ref season_wins, ref season_losses);
        }

        return $"{season_wins} {season_losses} {player.iTotalWins} {player.iTotalLosses}";
    }

    private static void SerializeRecord(in FGameRecord? record, ref int wins, ref int losses) {
        if (record == null) return;
        if (record.bPlayerWon) wins++;
        else losses++;
    }

    private static bool IsGameInSeasonSoFar(in FGameRecord? record, in ESeason season, in DateTime limit) {
        if (record == null) return false;
        var date = record.queue.teams_picked_message.timestamp;
        if (date == null) return false;

        return date < limit && date > seasons[season].after;
    }

    private static ESeason GetSeasonFromTime(in DateTime time) {
        if (time > seasons[ESeason.JULY22].after && time < seasons[ESeason.JULY22].before) return ESeason.JULY22;
        if (time > seasons[ESeason.JUNE22].after && time < seasons[ESeason.JUNE22].before) return ESeason.JUNE22;
        if (time > seasons[ESeason.MAY22].after && time < seasons[ESeason.MAY22].before) return ESeason.MAY22;
        if (time > seasons[ESeason.APRIL22].after && time < seasons[ESeason.APRIL22].before) return ESeason.APRIL22;
        if (time > seasons[ESeason.MARCH22].after && time < seasons[ESeason.MARCH22].before) return ESeason.MARCH22;

        return ESeason.UNKNOWN;
    } 

    public static Dictionary<ESeason,SeasonTimes> seasons = new() {
        {ESeason.JULY22, new SeasonTimes(DateTime.Parse(@"2022-07-31T18:30:00-04:00"), DateTime.Parse(@"2022-06-30T15:30:00-04:00"))},
        {ESeason.JUNE22, new SeasonTimes(DateTime.Parse(@"2022-06-30T15:29:00-04:00"), DateTime.Parse(@"2022-05-31T15:30:00-04:00"))},
        {ESeason.MAY22, new SeasonTimes(DateTime.Parse(@"2022-05-31T15:30:00-04:00"), DateTime.Parse(@"2022-04-30T15:30:00-04:00"))},
        {ESeason.APRIL22, new SeasonTimes(DateTime.Parse(@"2022-04-30T15:30:00-04:00"), DateTime.Parse(@"2022-03-31T15:30:00-04:00"))},
        {ESeason.MARCH22, new SeasonTimes(DateTime.Parse(@"2022-03-31T15:30:00-04:00"), DateTime.Parse(@"2022-02-28T15:30:00-05:00"))}
    };
}

public class SeasonTimes {
    public DateTime before;
    public DateTime after;
    public SeasonTimes(in DateTime before, in DateTime after) {
        this.before = before;
        this.after  = after;
    }
}

public enum ESeason {
    JULY22,
    JUNE22,
    MAY22,
    APRIL22,
    MARCH22,
    UNKNOWN
}