using Database.Database.Interfaces;

namespace Database.Database.DatabaseCore.Factories; 

public class FactoryBase : ILogger{
    protected ILogger logger { get; init; }

    protected FactoryBase() {
        logger = this as ILogger;
    }
}