
using Database;

public class DatabaseCore : IDatabaseComponent {
    #region Inherited Overrides

    /*  Equality operators worthless since this is strictly a singleton  */
    protected override bool IsEqual(IDatabaseComponent? rhs) {
        return false;
    }
    protected override bool IsLessThan(IDatabaseComponent? rhs) {
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