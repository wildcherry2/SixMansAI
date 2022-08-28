using Database.Database.DatabaseCore;

namespace Database.Structs;

public class FScoreReport {
    public DPlayer? reporter     { get; set; }
    public bool     bReportedWin { get; set; } = false;
    public int      iMatchId     { get; set; } = -1;
    public DPlayer? subbed_in    { get; set; }
    public DPlayer? subbed_out   { get; set; }

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
}