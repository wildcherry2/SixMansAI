using Database.Enums;
using Database.Structs;

namespace Database.Database.DatabaseCore.Season.Queue;

public class DQueue : IDatabaseComponent {
    public        int             match_id             { get; set; } = -1;
    public        FTeam           team_one             { get; set; }
    public        FTeam           team_two             { get; set; }
    public        FScoreReport    score_report         { get; set; }
    public        ETeamLabel      winner               { get; set; } = ETeamLabel.NOT_SET;
    public        DDiscordMessage teams_picked_message { get; set; }
    public List<string>   not_matched          { get; set; }

    // Precondition: Expects player names/objects to be deserialized 
    public DQueue(DDiscordMessage teams_picked_message) {
        not_matched = new List<string>();
        if (teams_picked_message.type == EMessageType.TEAMS_PICKED) {
            match_id = teams_picked_message.GetMatchId();

            var names = teams_picked_message.GetPlayerNamesFromTeamPickedMessage();

            if (names != null && names.Length == 6) {
                team_one = new FTeam(); //TODO: replace with GetTeamOne once gathering all DPlayers is implemented
                team_two = new FTeam();
                TryAssignToTeam(ref names);

                score_report = new FScoreReport(); // maybe parse names and score reports before chat so that we can go ahead and set it and winner now?}
                this.teams_picked_message = teams_picked_message;
            }
            else
                bError = true;
        }
        else
            bError = true; // TODO: add logging for every failed constructor in addition to setting bError
    }

    // Precondition: names is a valid array with a length of 6, with the first three indices being team one
    private void TryAssignToTeam(ref string[]? names) {
        var p1_name = names[0];
        var p2_name = names[1];
        var p3_name = names[2];
        var p4_name = names[3];
        var p5_name = names[4];
        var p6_name = names[5];
        var core = DDatabaseCore.GetSingleton();
        
        var current = core.GetPlayerIfExists(p1_name);
        if (!ReferenceEquals(current, null)) team_one.player_one = current;
        else not_matched.Add(p1_name); // TODO: log and add to name not matched list for manual selection

        current = core.GetPlayerIfExists(p2_name);
        if (!ReferenceEquals(current, null)) team_one.player_two = current;
        else not_matched.Add(p2_name); // TODO: log

        current = core.GetPlayerIfExists(p3_name);
        if (!ReferenceEquals(current, null)) team_one.player_three = current;
        else not_matched.Add(p3_name); // TODO: log

        current = core.GetPlayerIfExists(p4_name);
        if (!ReferenceEquals(current, null)) team_two.player_one = current;
        else not_matched.Add(p4_name); // TODO: log

        current = core.GetPlayerIfExists(p5_name);
        if (!ReferenceEquals(current, null)) team_two.player_two = current;
        else not_matched.Add(p5_name); // TODO: log

        current = core.GetPlayerIfExists(p6_name);
        if (!ReferenceEquals(current, null)) team_two.player_three = current;
        else not_matched.Add(p6_name); // TODO: log

        if (not_matched.Count > 0) bError = true;
    }

    public override string ToString() {
        var ret = "";

        ret += "Match ID = " + match_id + "\n";
        ret += "Team One\n";
        ret += team_one.player_one.recorded_names[0] + " (" + team_one.player_one.discord_id + "),\t";
        ret += team_one.player_two.recorded_names[0] + " (" + team_one.player_two.discord_id + "),\t";
        ret += team_one.player_three.recorded_names[0] + " (" + team_one.player_three.discord_id + ")\n\n";

        ret += "Team Two\n";
        ret += team_two.player_one.recorded_names[0] + " (" + team_two.player_one.discord_id + "),\t";
        ret += team_two.player_two.recorded_names[0] + " (" + team_two.player_two.discord_id + "),\t";
        ret += team_two.player_three.recorded_names[0] + " (" + team_two.player_three.discord_id + ")";

        /*
         * TODO: add mined score report stuff later
         */

        return ret;
    }

    #region Inherited Overrides
    protected override bool IsEqual(IDatabaseComponent? rhs)
    {
        var rhs_casted = rhs != null ? rhs as DQueue : null;
        if (rhs_casted)
            return match_id == rhs_casted.match_id;
        return false;
    }
    protected override bool IsLessThan(IDatabaseComponent? rhs)
    {
        var rhs_casted = rhs != null ? rhs as DQueue : null;
        if (rhs_casted)
        {
            return match_id < rhs_casted.match_id;
        }

        return false;
    }
    public override string ToJson()
    {
        /*
         *
         *  CONVERT TO JSON
         *
         */

        return "";
    }
    public override void ToJson(string save_path)
    {

    }
    public override void FromJson(string save_path)
    {

    }
    #endregion
}