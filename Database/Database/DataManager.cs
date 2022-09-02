using Database.Database.DatabaseCore;
using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Interfaces;
using Database.Structs;

namespace Database.Database;

public partial class DataManager : IDatabaseComponent {
    private DataManager() {}
    public        List<DPlayer>?      all_players               { get; private set; }
    public        List<DSeason>?      all_seasons               { get; private set; }
    public        List<FScoreReport>? all_score_reports         { get; private set; }
    public        List<DQueue>?       all_queues                { get; private set; }
    //public        FMessageList        all_discord_chat_messages { get; private set; }
    //public        FMessageList        all_score_report_messages { get; private set; }
    private static DataManager?        singleton                 { get; set; }
    public static DataManager GetSingleton() {
        if (singleton == null) singleton = new DataManager();

        return singleton;
    }

    public void InitializeData() {
        /*
         * init shit
         */
        all_players = DDatabaseCore.GetSingleton().all_players;
        all_seasons = DDatabaseCore.GetSingleton().all_seasons;
        all_score_reports = DDatabaseCore.GetSingleton().all_score_reports;
        all_queues = DDatabaseCore.GetSingleton().all_queues;
        SortAllTables();
    }
    public void SortAllTables() {
        if (!IsInitialized()) return;
        all_players.Sort(CompareKeysOfComponents);
        all_queues.Sort(CompareKeysOfComponents);
        all_score_reports.Sort(CompareKeysOfComponents);
    }

    public void Insert(in IDatabaseComponent? component) {
        if (component == null) return;
        var search_res = QueryFactory.GetSingleton().Search(component);
        if (search_res != int.MinValue && search_res < 0) {
            switch (component.TryGetOrCreatePrimaryKey().key_type) {
                case EPrimaryKeyType.PLAYER:
                    if(all_players.Count == 0) { all_players.Add(component as DPlayer); break; }
                    all_players.Insert(~search_res, component as DPlayer);
                    break;
                case EPrimaryKeyType.SCORE_REPORT:
                    if(all_score_reports.Count == 0) { all_score_reports.Add(component as FScoreReport); break; }
                    all_score_reports.Insert(~search_res, component as FScoreReport);
                    break;
                case EPrimaryKeyType.QUEUE:
                    if(all_queues.Count == 0) { all_queues.Add(component as DQueue); break; }
                    all_queues.Insert(~search_res, component as DQueue);
                    break;
                default:
                    return;
            }
        }
    }

    private bool IsInitialized() {
        //var core = DDatabaseCore.GetSingleton();
        if (ReferenceEquals(all_players, null)) return false;
        if (ReferenceEquals(all_queues, null)) return false;
        if (ReferenceEquals(all_score_reports, null)) return false;

        return true;
    }

    /*
 *  Less than 0 	x is less than y.
 *  0 	x equals y.
 *  Greater than 0 	x is greater than y. 
 */
    public static int CompareKeysOfComponents<Component>(Component? lhs, Component? rhs) where Component : IDatabaseComponent {
        if (lhs == null) {
            if (rhs == null) return 0;

            return -1;
        }

        if (rhs == null) { return 1; }

        if (lhs == rhs) return 0;
        if (lhs < rhs) return -1;

        return 1;
    }
}