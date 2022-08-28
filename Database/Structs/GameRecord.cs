using Database.Database.DatabaseCore.Season.Queue;
using Database.Enums;

namespace Database.Structs;

public class FGameRecord {
    public DQueue     queue { get; set; } = new DQueue();
    public ETeamLabel team  { get; set; } = ETeamLabel.NOT_SET;
    public bool   bPlayerWon { get; set; }
    public bool   bError     { get; set; } = false;
}