using Database.Database.DatabaseCore;

namespace Database.Structs;

public struct FScoreReport {
    public DPlayer reporter;
    public bool     bReportedWin;
    public int      iMatchId;
}