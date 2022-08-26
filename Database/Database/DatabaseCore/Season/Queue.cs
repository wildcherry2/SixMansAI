using Database.Enums;
using Database.Structs;

namespace Database.Database.DatabaseCore.Season;

public class DBQueue : IDatabaseComponent {
    public int          match_id     { get; set; } = -1;
    public FTeam        team_one     { get; set; }
    public FTeam        team_two     { get; set; }
    public FScoreReport score_report { get; set; }

    //public List<DiscordMessage> raw_messages_in_queue
    public ETeamLabel winner;

    public DBQueue(int match_id /* List<DiscordMessages> maybe?? */) : base(ConsoleColor.Cyan, 2, "DBQueue"){
        this.match_id = match_id;
        // raw_messages_in_queue = new
        // init structs?
    }



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