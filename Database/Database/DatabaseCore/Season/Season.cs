using Database.Database.DatabaseCore.Season.RawMessageDeserializer;
using Database.Structs;

namespace Database.Database.DatabaseCore.Season;

public class DBSeason : IDatabaseComponent {
    private DBSeason(string chat_data, string score_report_data) : base(ConsoleColor.Yellow, 1, "DBSeason") {
        queues = new List<DBQueue>();
    }
    public         FSeasonLabel         season_label { get; set; }
    public         List<DBQueue>        queues       { get; set; }
    private static DBSeason?            singleton;
    private static QueueScanner?        queue_scanner;
    private static RawChatDeserializer? deserializer;

    public static DBSeason GetSingleton(string chat_data = "", string score_report_data = "") {
        if (!singleton) singleton = new DBSeason(chat_data, score_report_data);
        return singleton;
    }

    public void SetQueues() {

    }

    #region Inherited Overrides

    protected override bool IsEqual(IDatabaseComponent? rhs) {
        
        var rhs_casted = rhs != null ? (rhs as DBSeason) : null;
        if(rhs_casted) 
            return season_label.month == rhs_casted.season_label.month && season_label.year == rhs_casted.season_label.year;
        return false;
    }
    protected override bool IsLessThan(IDatabaseComponent? rhs) {
        var rhs_casted = rhs != null ? (rhs as DBSeason) : null;
        if (rhs_casted) {
            /*
             *
             *  COMPARE DATES
             *
             */
            return true;
        }

        return false;
    }
    public override string ToJson() {
        /*
         *
         *  CONVERT TO JSON
         *
         */

        return "";
    }
    public override void ToJson(string save_path) {

    }
    public override void FromJson(string save_path) {

    }

    #endregion
}