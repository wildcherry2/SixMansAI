
using System.Data;
using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Interfaces;

namespace Database.Structs;

public class FScoreReport : IDatabaseComponent {
    public  DPlayer?        reporter     { get; set; }
    public  PrimaryKey      pk_season     { get; set; }
    public  bool            bReportedWin { get; set; } = false;
    public  int             iMatchId     { get; set; } = -1;
    public  bool            bHasSubs     { get; set; }
    public  DPlayer?        subbed_in    { get; set; }
    public  DPlayer?        subbed_out   { get; set; }
    public  DDiscordMessage report_msg   { get; set; }
    //private PrimaryKey      primary_key;
    private bool            bIsPrimaryKeySet { get; set; } = false;
    public FScoreReport(){}
    public FScoreReport(in PrimaryKey key) {
        primary_key      = key;
        bIsPrimaryKeySet = true;
    }
    public bool IsValid() {
        if(iMatchId == -1) {logger.Log("Match ID invalid in score report!"); return false; }
        if(ReferenceEquals(reporter, null)) { logger.Log("Null reporter in score report for lobby {0}!", iMatchId.ToString()); return false; }
        if (bHasSubs) {
            if(ReferenceEquals(subbed_in, null)) {logger.Log("Score report for lobby {0} has subs but subbed_in player was null!", iMatchId.ToString()); return false; }
            if(ReferenceEquals(subbed_out, null)) {logger.Log("Score report for lobby {0} has subs but subbed_out player was null!", iMatchId.ToString()); return false; }

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

    public override PrimaryKey TryGetOrCreatePrimaryKey() {
        if (bIsPrimaryKeySet) return primary_key;
        primary_key      = new PrimaryKey(ulong.Parse(report_msg.id), EPrimaryKeyType.SCORE_REPORT);
        bIsPrimaryKeySet = true;
        return primary_key;
    }
}