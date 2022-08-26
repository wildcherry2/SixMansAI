using Database.Structs;

namespace Database.Database.DatabaseCore;

public class DBPlayer : IDatabaseComponent {
    public ulong             discord_id     { get; set; } = 0;
    public List<string>      recorded_names { get; set; }
    public List<FGameRecord> game_history   { get; set; }
    public int               iTotalWins     { get; set; } = 0;
    public int               iTotalLosses   { get; set; } = 0;

    public DBPlayer(ulong discord_id, string discord_name, string nickname = "", string link_name = "") : base(ConsoleColor.Yellow, 1, "DBPlayer"){
        this.discord_id = discord_id;
        game_history = new List<FGameRecord>();
        recorded_names = new List<string>();
        recorded_names.Add(discord_name);
        if(nickname.Length > 0) recorded_names.Add(nickname);
        if(link_name.Length > 0) recorded_names.Add(link_name);
    }



    #region Inherited Overrides
    protected override bool IsEqual(IDatabaseComponent? rhs) {
    
        var rhs_casted = rhs != null ? (rhs as DBPlayer) : null;
        if(rhs_casted) 
            return discord_id == rhs_casted.discord_id;
        return false;
    }
    protected override bool IsLessThan(IDatabaseComponent? rhs) {
        var rhs_casted = rhs != null ? (rhs as DBPlayer) : null;
        if (rhs_casted) {
            return discord_id < rhs_casted.discord_id;
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