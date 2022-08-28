using Database.Database.DatabaseCore;

namespace Database.Structs;

public class FScoreReport {
    public DPlayer? reporter     { get; set; }
    public bool    bReportedWin { get; set; } = false;
    public int     iMatchId     { get; set; } = -1;
    public DPlayer? subbed_in { get; set; }
    public DPlayer? subbed_out { get; set; }
}