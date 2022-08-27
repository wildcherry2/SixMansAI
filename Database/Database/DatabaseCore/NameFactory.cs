
namespace Database.Database.DatabaseCore; 

public class NameFactory {
    private static NameFactory? singleton { get; set; }

    private NameFactory() {

    }

    public static NameFactory GetSingleton() {
        if (singleton == null) singleton = new NameFactory();
        return singleton;
    }
}