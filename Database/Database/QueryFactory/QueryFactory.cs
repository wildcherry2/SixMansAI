using Database.Database.DatabaseCore;
using Database.Database.DatabaseCore.MainComponents;
using Database.Structs;

namespace Database.Database.QueryFactory; 

public static class QueryFactory {
    public static DPlayer? QueryPlayers(in string name) {
        var players = DDatabaseCore.GetSingleton().all_players;

        if (ReferenceEquals(players, null)) return null;

        foreach (var player in players) {
            foreach (var player_name in player.recorded_names) {
                if(player_name == name) return player;
            }
        }

        return null;
    }
    public static DPlayer? QueryPlayers(in PrimaryKey primary_key) {
        var players = DDatabaseCore.GetSingleton().all_players;

        if (ReferenceEquals(players, null)) return null;

        foreach (var player in players) {
            if(player.TryGetOrCreatePrimaryKey() == primary_key) return player;
        }

        return null;
    }
    public static DQueue? QueryQueues(in DSeason season, in int match_id) {
        return null;
    }
    public static DQueue? QueryQueues(in PrimaryKey primary_key) {
        var queues = DDatabaseCore.GetSingleton().all_queues;

        if (ReferenceEquals(queues, null)) return null;

        foreach (var queue in queues) {
            if(queue.TryGetOrCreatePrimaryKey() == primary_key) return queue;
        }

        return null;
    }
    public static FScoreReport? QueryScoreReports(in PrimaryKey primary_key) {
        var reports = DDatabaseCore.GetSingleton().all_score_reports;

        if (ReferenceEquals(reports, null)) return null;

        foreach (var report in reports) {
            if(report.TryGetOrCreatePrimaryKey() == primary_key) return report;
        }

        return null;
    }
}