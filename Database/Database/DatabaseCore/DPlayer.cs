using Database.Structs;

namespace Database.Database.DatabaseCore;

public class DPlayer : IDatabaseComponent {
    public ulong             discord_id     { get; set; } = 0;
    public List<string>      recorded_names { get; set; }
    public List<FGameRecord> game_history   { get; set; }
    public int               iTotalWins     { get; set; } = 0;
    public int               iTotalLosses   { get; set; } = 0;

    public DPlayer(ulong discord_id, string discord_name, string nickname = "", string link_name = "") : base(ConsoleColor.Yellow, 1, "DPlayer"){
        this.discord_id = discord_id;
        game_history = new List<FGameRecord>();
        recorded_names = new List<string>();
        recorded_names.Add(discord_name);
        if(nickname.Length > 0 && nickname != discord_name) recorded_names.Add(nickname);
        if(link_name.Length > 0 && link_name != discord_name && link_name != nickname) recorded_names.Add(link_name);
    }

    public bool HasName(string name) {
        foreach (var player_name in recorded_names) {
            if (name == player_name) return true;
        }

        return false;
    }

    public FGameRecord GetPlayerGameRecord(FSeasonLabel season, int match_id) {
        /*
         * Get season object here
         */

        return new FGameRecord();
    }

    public void TryAddName(string name) {
        bool found = false;
        foreach (var player_name in recorded_names) {
            if (name == player_name) {
                found = true;
                break;
            }
        }

        if(!found) recorded_names.Add(name);
    }

    public void TryAddGameRecord(FGameRecord record) {
        bool found = false;
        foreach (var rec in game_history) {
            if (rec.queue == record.queue) {
                found = true;
                break;
            }
        }

        if(!found) game_history.Add(record);
    }

    public static bool operator ==(DPlayer? lhs, string rhs) {
        if (!lhs) return false;
        foreach (var name in lhs.recorded_names) {
            if(name == rhs) return true;
        }

        return false;
    }

    public static bool operator !=(DPlayer? lhs, string rhs) {
        return !(lhs == rhs);
    }

    #region Inherited Overrides
    protected override bool IsEqual(IDatabaseComponent? rhs) {
    
        var rhs_casted = rhs != null ? (rhs as DPlayer) : null;
        if(!ReferenceEquals(rhs_casted, null)) 
            return discord_id == rhs_casted.discord_id;
        return false;
    }
    protected override bool IsLessThan(IDatabaseComponent? rhs) {
        var rhs_casted = rhs != null ? (rhs as DPlayer) : null;
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