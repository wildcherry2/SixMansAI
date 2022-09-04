using Database.AISerialization;
using Database.Database.DatabaseCore;

namespace Database {
    public class MainClass {
        public static void Main(string[] args) {
            var core = DDatabaseCore.GetSingleton();
            core.BuildDatabase(true);
            AISerializerCore.GetSingleton().Serialize(@"C:\Users\tyler\Documents\Programming\AI\SixMans\Reports");

        }
    }
}