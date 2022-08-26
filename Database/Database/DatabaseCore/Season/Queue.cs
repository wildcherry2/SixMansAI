namespace Database.Database.DatabaseCore.Season;

public class DBQueue : IDatabaseComponent {
    public int match_id { get; set; } = -1;

    #region Inherited Overrides
    protected override bool IsEqual(IDatabaseComponent? rhs) {
        var rhs_casted = rhs != null ? (rhs as DBQueue) : null;
        if(rhs_casted) 
            return match_id == rhs_casted.match_id;
        return false;
    }
    protected override bool IsLessThan(IDatabaseComponent? rhs) {
        var rhs_casted = rhs != null ? (rhs as DBQueue) : null;
        if (rhs_casted) {
            return match_id < rhs_casted.match_id;
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