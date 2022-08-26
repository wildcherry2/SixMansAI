namespace Database.Database.DatabaseCore;

public class DBPlayer : IDatabaseComponent {
    public ulong discord_id { get; set; } = 0;


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