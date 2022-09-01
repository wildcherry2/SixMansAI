using Database.Database.Interfaces;

public class FactoryBase : ILogger{
    protected ILogger logger { get; init; }

    protected FactoryBase() {
        logger = this as ILogger;
    }
}