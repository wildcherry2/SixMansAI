using Database.Database.Interfaces;

namespace Database.Database.DatabaseCore.Binders; 

public class BinderBase : ILogger {
    protected ILogger logger { get; init; }

    protected BinderBase() {
        logger = this as ILogger;
    }
}