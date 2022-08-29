using Database.Database.DatabaseCore.MainComponents;

namespace Database.Structs;

public class FTeam {
    public DPlayer? player_one   { get; set; }
    public DPlayer? player_two   { get; set; }
    public DPlayer? player_three { set; get; }
}