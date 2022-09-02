using Database.Database.DatabaseCore;
using Database.Database.DatabaseCore.Factories;
using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Interfaces;
using Database.Structs;

namespace Database.Database;

public class QueryFactory : FactoryBase {
    private static QueryFactory? singleton { get; set; }
    private QueryFactory() { }
    public static QueryFactory GetSingleton() {
        if (singleton == null) singleton = new QueryFactory();

        return singleton;
    }
    public IDatabaseComponent? Query(in PrimaryKey key) {
        switch (key.key_type) {
            case EPrimaryKeyType.PLAYER:
                return QueryPlayers(key);
            case EPrimaryKeyType.QUEUE:
                return QueryQueues(key);
            case EPrimaryKeyType.SCORE_REPORT:
                return QueryScoreReports(key);
            default: 
                return null;
        }
    }

    public ComponentType? Query<ComponentType>(in PrimaryKey key) where ComponentType : IDatabaseComponent {
        IDatabaseComponent? result = null;
        switch (key.key_type) {
            case EPrimaryKeyType.PLAYER:
                result = QueryPlayers(key);
                return (ComponentType?)result;
            case EPrimaryKeyType.QUEUE:
                result = QueryQueues(key);
                return (ComponentType?)result;
            case EPrimaryKeyType.SCORE_REPORT:
                result = QueryScoreReports(key);
                return (ComponentType?)result;
            default: 
                return null;
        }
    }

    // Specifically for getting the return value of a List.BinarySearch, unlike the Query methods
    public int Search(in IDatabaseComponent? component) {
        if(component == null) return int.MinValue;
        var key   = component.TryGetOrCreatePrimaryKey();
        var dm    = DataManager.GetSingleton();
        var error = int.MinValue;
        switch (key.key_type) {
            case EPrimaryKeyType.PLAYER:
                if (dm.all_players == null) return error;
                //if (dm.all_players.Count == 0) return 0;
                return dm.all_players.BinarySearch(component as DPlayer);
            case EPrimaryKeyType.QUEUE:
                if (dm.all_queues == null) return error;
                //if (dm.all_queues.Count == 0) return 0;
                return dm.all_queues.BinarySearch(component as DQueue);
            case EPrimaryKeyType.SCORE_REPORT:
                if (dm.all_score_reports == null) return error;
                //if (dm.all_score_reports.Count == 0) return 0;
                return dm.all_score_reports.BinarySearch(component as ScoreReport);
            default:
                return error;
        }
    }

    public int Search(in PrimaryKey key) {
        switch (key.key_type) {
            case EPrimaryKeyType.PLAYER:
                var dummyp = new DPlayer(key);
                return Search(dummyp);
            case EPrimaryKeyType.QUEUE:
                var dummyq = new DQueue(key);
                return Search(dummyq);
            case EPrimaryKeyType.SCORE_REPORT:
                var dummyr = new ScoreReport(key);
                return Search(dummyr);
            default:
                return int.MinValue;
        }
    }

    public DPlayer? QueryPlayers(in string name) {
        var players = DataManager.GetSingleton().all_players;

        if (ReferenceEquals(players, null)) return null;

        foreach (var player in players) {
            foreach (var player_name in player.recorded_names) {
                if (player_name == name) return player;
            }
        }

        return null;
    }
    public DPlayer? QueryPlayers(in PrimaryKey primary_key) {
        var players = DataManager.GetSingleton().all_players;

        if (ReferenceEquals(players, null)) return null;

        var search = Search(primary_key);
        if (search >= 0) {
            return players[search];
        }

        return null;
    }
    public DQueue? QueryQueues(in DSeason season, in int match_id) { return null; }
    public DQueue? QueryQueues(in PrimaryKey primary_key) {
        var queues = DataManager.GetSingleton().all_queues;

        if (ReferenceEquals(queues, null)) return null;

        var search = Search(primary_key);
        if (search >= 0) {
            return queues[search];
        }

        return null;
    }
    public ScoreReport? QueryScoreReports(in PrimaryKey primary_key) {
        var reports = DataManager.GetSingleton().all_score_reports;

        if (ReferenceEquals(reports, null)) return null;

        var search = Search(primary_key);
        if (search >= 0) {
            return reports[search];
        }

        return null;
    }
}
