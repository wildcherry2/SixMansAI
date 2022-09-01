using Database.Database.DatabaseCore;
using Database.Database.Interfaces;
using Database.Structs;

public class DataManager : IDatabaseComponent{
    public void SortAllTables() {
        if (!IsInitialized()) return;
        var core = DDatabaseCore.GetSingleton();
        core.all_players.Sort(CompareKeys);
        core.all_queues.Sort(CompareKeys);
        core.all_score_reports.Sort(CompareKeys);

    }
    private  bool IsInitialized() {
        var core = DDatabaseCore.GetSingleton();
        if (ReferenceEquals(core.all_players, null)) return false;
        if (ReferenceEquals(core.all_queues, null)) return false;
        if (ReferenceEquals(core.all_score_reports, null)) return false;
        return true;
    }

    /*
     *  Less than 0 	x is less than y.
     *  0 	x equals y.
     *  Greater than 0 	x is greater than y. 
     */
    private static int CompareKeys(IDatabaseComponent? lhs, IDatabaseComponent? rhs) {
        if (lhs == null) {
            if (rhs == null) return 0;
            return -1;
        }
        else {
            if(rhs == null) return 1;
            else {
                if(lhs == rhs) return 0;
                if(rhs < lhs) return -1;
                return 1;
            }
        }
    }
}