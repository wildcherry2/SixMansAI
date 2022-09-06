using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Structs;

namespace Database.AISerialization; 

public static class PlayerSerializer {
    public static Dictionary<ESeason, SeasonTimes> seasons = new() {
        { ESeason.AUGUST22, new SeasonTimes(DateTime.Parse(@"2022-08-31T15:30:00-04:00"), DateTime.Parse(@"2022-07-31T18:30:00-04:00")) },
        { ESeason.JULY22, new SeasonTimes(DateTime.Parse(@"2022-07-31T18:30:00-04:00"), DateTime.Parse(@"2022-06-30T15:30:00-04:00")) },
        { ESeason.JUNE22, new SeasonTimes(DateTime.Parse(@"2022-06-30T15:29:00-04:00"), DateTime.Parse(@"2022-05-31T15:30:00-04:00")) },
        { ESeason.MAY22, new SeasonTimes(DateTime.Parse(@"2022-05-31T15:30:00-04:00"), DateTime.Parse(@"2022-04-30T15:30:00-04:00")) },
        { ESeason.APRIL22, new SeasonTimes(DateTime.Parse(@"2022-04-30T15:30:00-04:00"), DateTime.Parse(@"2022-03-31T15:30:00-04:00")) },
        { ESeason.MARCH22, new SeasonTimes(DateTime.Parse(@"2022-03-31T15:30:00-04:00"), DateTime.Parse(@"2022-02-28T15:30:00-05:00")) },
        { ESeason.FEBRUARY22, new SeasonTimes(DateTime.Parse(@"2022-02-28T15:30:00-05:00"), DateTime.Parse(@"2022-01-31T15:30:00-05:00")) },
        { ESeason.JANUARY22, new SeasonTimes(DateTime.Parse(@"2022-01-31T15:30:00-05:00"), DateTime.Parse(@"2022-01-02T15:30:00-05:00")) }
    };

    public static string SerializePlayer(in DPlayer player, in DateTime queue_time) {
        var season_wins   = 0;
        var season_losses = 0;
        foreach (var record in player.game_history) {
            if (IsGameInSeasonSoFar(record, GetSeasonFromTime(queue_time), queue_time)) { SerializeRecord(record, ref season_wins, ref season_losses); }
        }

        return $"{season_wins - season_losses},{player.iTotalWins - player.iTotalLosses}";
    }

    public static string SerializePlayer(in DPlayer player, in DateTime queue_time, in EPlayerNumber number, ref MiniQueue meta) {
        var season_wins   = 0;
        var season_losses = 0;
        foreach (var record in player.game_history) {
            if (IsGameInSeasonSoFar(record, GetSeasonFromTime(queue_time), queue_time)) { SerializeRecord(record, ref season_wins, ref season_losses); }
        }

        switch (number) {
            case EPlayerNumber.ONE:
                meta.one_wins   = season_wins;
                meta.one_losses = season_losses;
                break;
            case EPlayerNumber.TWO:
                meta.two_wins   = season_wins;
                meta.two_losses = season_losses;
                break;
            case EPlayerNumber.THREE:
                meta.three_wins   = season_wins;
                meta.three_losses = season_losses;
                break;
            case EPlayerNumber.FOUR:
                meta.four_wins   = season_wins;
                meta.four_losses = season_losses;
                break;
            case EPlayerNumber.FIVE:
                meta.five_wins = season_wins;
                meta.five_losses = season_losses;
                break;
            default:
                meta.six_wins   = season_wins;
                meta.six_losses = season_losses;
                break;
        }

        return $"{season_wins - season_losses},{season_wins + season_losses},{player.iTotalWins - player.iTotalLosses},{player.iTotalWins + player.iTotalLosses}";
    }

    private static void SerializeRecord(in FGameRecord? record, ref int wins, ref int losses) {
        if (record == null) { return; }

        if (record.bPlayerWon) { wins++; }
        else { losses++; }
    }

    private static bool IsGameInSeasonSoFar(in FGameRecord? record, in ESeason season, in DateTime limit) {
        if (record == null) { return false; }

        var date = record.queue.teams_picked_message.timestamp;
        if (date == null) { return false; }

        return date < limit && date > seasons[season].after;
    }

    public static ESeason GetSeasonFromTime(in DateTime time) {
        if (time > seasons[ESeason.AUGUST22].after && time < seasons[ESeason.AUGUST22].before) { return ESeason.AUGUST22; }

        if (time > seasons[ESeason.JULY22].after && time < seasons[ESeason.JULY22].before) { return ESeason.JULY22; }

        if (time > seasons[ESeason.JUNE22].after && time < seasons[ESeason.JUNE22].before) { return ESeason.JUNE22; }

        if (time > seasons[ESeason.MAY22].after && time < seasons[ESeason.MAY22].before) { return ESeason.MAY22; }

        if (time > seasons[ESeason.APRIL22].after && time < seasons[ESeason.APRIL22].before) { return ESeason.APRIL22; }

        if (time > seasons[ESeason.MARCH22].after && time < seasons[ESeason.MARCH22].before) { return ESeason.MARCH22; }

        if (time > seasons[ESeason.FEBRUARY22].after && time < seasons[ESeason.FEBRUARY22].before) { return ESeason.FEBRUARY22; }

        if (time > seasons[ESeason.JANUARY22].after && time < seasons[ESeason.JANUARY22].before) { return ESeason.JANUARY22; }

        return ESeason.UNKNOWN;
    }
}

public class SeasonTimes {
    public DateTime after;
    public DateTime before;
    public SeasonTimes(in DateTime before, in DateTime after) {
        this.before = before;
        this.after  = after;
    }
}

public enum ESeason {
    AUGUST22,
    JULY22,
    JUNE22,
    MAY22,
    APRIL22,
    MARCH22,
    FEBRUARY22,
    JANUARY22,
    UNKNOWN
}

public enum EPlayerNumber {
    ONE,
    TWO,
    THREE,
    FOUR,
    FIVE,
    SIX
}