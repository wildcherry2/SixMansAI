﻿using Database.Database.DatabaseCore;

namespace Database.Structs;

public struct FScoreReport {
    public DBPlayer reporter;
    public bool     bReportedWin;
    public int      iMatchId;
}