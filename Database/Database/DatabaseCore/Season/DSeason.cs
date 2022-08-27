using Database.Database.DatabaseCore.Season.RawMessageDeserializer;
using Database.Structs;

namespace Database.Database.DatabaseCore.Season;

public class DSeason : IDatabaseComponent {
    private DSeason(string chat_data, string score_report_data) : base(ConsoleColor.Yellow, 1, "DSeason") {
        queues = new List<DQueue>();
    }
    public         FSeasonLabel         season_label { get; set; }
    public         List<DQueue>        queues       { get; set; }
    private static DSeason?            singleton;
    private static QueueScanner?        queue_scanner;
    private static RawChatDeserializer? deserializer;

    public static DSeason GetSingleton(string chat_data = "", string score_report_data = "") {
        if (!singleton) singleton = new DSeason(chat_data, score_report_data);
        return singleton;
    }

    public void SetQueues() {

    }

    #region Inherited Overrides

    protected override bool IsEqual(IDatabaseComponent? rhs) {
        
        var rhs_casted = rhs != null ? (rhs as DSeason) : null;
        if(rhs_casted) 
            return season_label.month == rhs_casted.season_label.month && season_label.year == rhs_casted.season_label.year;
        return false;
    }
    protected override bool IsLessThan(IDatabaseComponent? rhs) {
        var rhs_casted = rhs != null ? (rhs as DSeason) : null;
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