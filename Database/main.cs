using Database.Database.DatabaseCore;

namespace Database; 

public class MainClass {
    public static void Main(string[] args) {
        var core = DDatabaseCore.GetSingleton();
        core.BuildDatabase(true);
    }
}