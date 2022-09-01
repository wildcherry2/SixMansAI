using Database.Database.Interfaces;

namespace Database.Database.DatabaseCore.Cleaners; 

public class CleanerBase : ILogger {
    protected ILogger logger { get; init; }

    protected CleanerBase() {
        logger = this as ILogger;
    }
}