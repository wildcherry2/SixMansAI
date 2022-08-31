
using System.Data;
using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Interfaces;

namespace Database.Structs;

public class FScoreReport : ILogger, IPrimaryKey {
    public  DPlayer?        reporter     { get; set; }
    public  bool            bReportedWin { get; set; } = false;
    public  int             iMatchId     { get; set; } = -1;
    public  bool            bHasSubs     { get; set; }
    public  DPlayer?        subbed_in    { get; set; }
    public  DPlayer?        subbed_out   { get; set; }
    public  DDiscordMessage report_msg   { get; set; }
    private  ulong           primary_key;
    private bool            bIsPrimaryKeySet { get; set; } = false;
    public FScoreReport() : base(ConsoleColor.Cyan, 2, "FScoreReport"){}
    public bool IsValid() {
        if(iMatchId == -1) {Log("Match ID invalid in score report!"); return false; }
        if(ReferenceEquals(reporter, null)) { Log("Null reporter in score report for lobby {0}!", iMatchId.ToString()); return false; }
        if (bHasSubs) {
            if(ReferenceEquals(subbed_in, null)) {Log("Score report for lobby {0} has subs but subbed_in player was null!", iMatchId.ToString()); return false; }
            if(ReferenceEquals(subbed_out, null)) {Log("Score report for lobby {0} has subs but subbed_out player was null!", iMatchId.ToString()); return false; }

        }
        return true;
    }

    public bool bError { get; set; } = false;
    public override string ToString() {
        return "Reporter name = " +
               (reporter != null ? reporter.recorded_names[0] : "null") +
               "\nReported win = " +
               bReportedWin +
               "\nMatch ID = " +
               iMatchId +
               (subbed_in != null ? "\nSubbed in name = " + subbed_in.recorded_names[0] : "") +
               (subbed_in != null ? "\nSubbed out name = " + subbed_out.recorded_names[0] : "");
    }

    public ulong TryGetOrCreatePrimaryKey() {
        if (bIsPrimaryKeySet) return primary_key;
        primary_key = ulong.Parse(report_msg.id);
        bIsPrimaryKeySet = true;
        return primary_key;
    }
}