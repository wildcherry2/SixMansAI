using Database.Database.DatabaseCore;
using Database.Database.DatabaseCore.Season;
using Database.Enums;

namespace Database.Structs;

public struct FQueryResult<T> where T : IDatabaseComponent {
    //private EQueryResultType type;
    private T?               result;
    //private T?        season;
    //private T?         queue;
    //private List<T>    queues;

    T? GetResult() {
        return result;
    }
}