using Database.Database.DatabaseCore;
using Database.Database.DatabaseCore.Season;
using Database.Database.Interfaces;
using Database.Enums;

namespace Database.Structs;

//make common base class for structs so T can be a struct as well?
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