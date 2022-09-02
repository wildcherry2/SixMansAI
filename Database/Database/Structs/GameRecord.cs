using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Enums;

namespace Database.Database.Structs;

public class FGameRecord {
    public DQueue     queue { get; set; } = new DQueue();
    public ETeamLabel team  { get; set; } = ETeamLabel.NOT_SET;
    public bool   bPlayerWon { get; set; }
    public bool   bError     { get; set; } = false;
}