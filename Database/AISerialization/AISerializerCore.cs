using Database.Database.DatabaseCore;

namespace Database.AISerialization; 

public class AISerializerCore {
    //private        string           path_to_reports { get; set; } = "";
    private AISerializerCore() { }
    private static AISerializerCore singleton { get; set; }
    public static AISerializerCore GetSingleton() {
        if (singleton == null) { singleton = new AISerializerCore(); }

        return singleton;
    }
    public void Serialize(in string path) {
        if (!DDatabaseCore.GetSingleton().built) { return; }

        if (path.Length == 0) { return; }

        var queues = DDatabaseCore.GetSingleton().all_queues;
        if (queues == null) { return; }

        var name = MakeFileName();
        File.Create(path + name).Close();
        var sw = new StreamWriter(path + name);
        foreach (var q in queues) {
            var ret = QueueSerializer.GetQueueString(q);
            if (ret.Length > 0) { sw.WriteLine(ret); }
        }

        sw.Close();
    }

    private string MakeFileName() {
        var ret_val = "";
        var time    = DateTime.Now;
        ret_val += $"data_{time}";
        ret_val =  ret_val.Replace("\\", "_");
        ret_val =  ret_val.Replace("/", "_");
        ret_val =  ret_val.Replace(":", "-");
        ret_val =  ret_val.Insert(0, "\\");
        ret_val += ".csv";

        return ret_val;
    }
}