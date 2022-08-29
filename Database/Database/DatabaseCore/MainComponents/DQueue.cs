using Database.Database.DatabaseCore.Factories;
using Database.Database.Interfaces;
using Database.Enums;
using Database.Structs;

namespace Database.Database.DatabaseCore.MainComponents;

public class DQueue : IDatabaseComponent
{
    public int match_id { get; set; } = -1;
    public FTeam team_one { get; set; }
    public FTeam team_two { get; set; }
    public FScoreReport? score_report { get; set; }
    public ETeamLabel winner { get; set; } = ETeamLabel.NOT_SET;
    public DDiscordMessage teams_picked_message { get; set; }
    public List<string> names_not_matched { get; set; }
    public List<DPlayer> matched { get; set; }
    private ETeamLabel unmatched_label { get; set; } = ETeamLabel.NOT_SET;
    private int unmatched_player_index { get; set; } = 0;

    public DQueue() { }

    // Precondition: Expects player names/objects to be deserialized 
    public DQueue(DDiscordMessage teams_picked_message)
    {
        if (!PlayerFactory.GetSingleton().bIsComplete)
        {
            // log precondition fail
        }

        names_not_matched = new List<string>();
        matched = new List<DPlayer>();
        if (teams_picked_message.type == EMessageType.TEAMS_PICKED)
        {
            match_id = teams_picked_message.GetMatchId();

            var names = teams_picked_message.GetPlayerNamesFromTeamPickedMessage();

            if (names != null && names.Length == 6)
            {
                team_one = new FTeam();
                team_two = new FTeam();
                TryAssignToTeam(ref names);

                //score_report = new FScoreReport(); // maybe parse names and score reports before chat so that we can go ahead and set it and winner now?}
                this.teams_picked_message = teams_picked_message;
                if (bError)
                    TryInferMissingPlayer();
            }
            else
            {
                Log("Error getting names from teams picked message with message ID = {0} and match ID = {1}", teams_picked_message.id.ToString(), match_id.ToString());
                bError = true;
            }
        }
        else
        {
            Log("Wrong message type passed into method! Type = {0}", teams_picked_message.type.ToString());
            bError = true;
        }
    }

    [Obsolete("IsQueueCreation isn't needed and isn't guaranteed to produce an accurate result.", true)]
    public bool IsQueueCreationComplete()
    {
        if (match_id == -1) { Log("Match ID is invalid!"); return false; }
        if (!IsTeamValid(ETeamLabel.TEAM_ONE)) return false;
        if (!IsTeamValid(ETeamLabel.TEAM_TWO)) return false;
        if (ReferenceEquals(score_report, null)) { Log("Missing score report for Match {0}!", match_id.ToString()); return false; }
        if (!score_report.IsValid()) return false;
        if (winner == ETeamLabel.NOT_SET) { Log("Winning team not set for Match {0}!", match_id.ToString()); return false; }

        return true;
    }

    private bool IsTeamValid(ETeamLabel team)
    {
        if (team == ETeamLabel.TEAM_ONE)
        {
            if (ReferenceEquals(team_one.player_one, null)) { Log("Match {0}, Team 1, player 1 is null!", match_id.ToString()); return false; }
            if (ReferenceEquals(team_one.player_two, null)) { Log("Match {0}, Team 1, player 2 is null!", match_id.ToString()); return false; }
            if (ReferenceEquals(team_one.player_three, null)) { Log("Match {0}, Team 1, player 3 is null!", match_id.ToString()); return false; }
        }
        else if (team == ETeamLabel.TEAM_TWO)
        {
            if (ReferenceEquals(team_two.player_one, null)) { Log("Match {0}, Team 2, player 1 is null!", match_id.ToString()); return false; }
            if (ReferenceEquals(team_two.player_two, null)) { Log("Match {0}, Team 2, player 2 is null!", match_id.ToString()); return false; }
            if (ReferenceEquals(team_two.player_three, null)) { Log("Match {0}, Team 2, player 3 is null!", match_id.ToString()); return false; }
        }
        else if (team == ETeamLabel.NOT_SET) return false;

        return true;
    }

    // Precondition: names is a valid array with a length of 6, with the first three indices being team one
    private void TryAssignToTeam(ref string[]? names)
    {
        var p1_name = names[0];
        var p2_name = names[1];
        var p3_name = names[2];
        var p4_name = names[3];
        var p5_name = names[4];
        var p6_name = names[5];
        var core = DDatabaseCore.GetSingleton();

        var current = core.GetPlayerIfExists(p1_name);
        if (!ReferenceEquals(current, null)) { team_one.player_one = current; matched.Add(current); }
        else
        {
            names_not_matched.Add(p1_name);
            unmatched_label = ETeamLabel.TEAM_ONE;
            unmatched_player_index = 1;
        }

        current = core.GetPlayerIfExists(p2_name);
        if (!ReferenceEquals(current, null)) { team_one.player_two = current; matched.Add(current); }
        else
        {
            names_not_matched.Add(p2_name);
            unmatched_label = ETeamLabel.TEAM_ONE;
            unmatched_player_index = 2;
        }

        current = core.GetPlayerIfExists(p3_name);
        if (!ReferenceEquals(current, null)) { team_one.player_three = current; matched.Add(current); }
        else
        {
            names_not_matched.Add(p3_name);
            unmatched_label = ETeamLabel.TEAM_ONE;
            unmatched_player_index = 3;
        }

        current = core.GetPlayerIfExists(p4_name);
        if (!ReferenceEquals(current, null)) { team_two.player_one = current; matched.Add(current); }
        else
        {
            names_not_matched.Add(p4_name);
            unmatched_label = ETeamLabel.TEAM_TWO;
            unmatched_player_index = 1;
        }
        current = core.GetPlayerIfExists(p5_name);
        if (!ReferenceEquals(current, null)) { team_two.player_two = current; matched.Add(current); }
        else
        {
            names_not_matched.Add(p5_name);
            unmatched_label = ETeamLabel.TEAM_TWO;
            unmatched_player_index = 2;
        }

        current = core.GetPlayerIfExists(p6_name);
        if (!ReferenceEquals(current, null)) { team_two.player_three = current; matched.Add(current); }
        else
        {
            names_not_matched.Add(p6_name);
            unmatched_label = ETeamLabel.TEAM_TWO;
            unmatched_player_index = 3;
        }

        if (names_not_matched.Count > 0) bError = true;
    }

    public bool IsPlayerInTeam(ETeamLabel team, ulong discord_id)
    {
        switch (team)
        {
            case ETeamLabel.TEAM_ONE:
                if (!ReferenceEquals(team_one.player_one, null) && team_one.player_one.discord_id == discord_id ||
                    !ReferenceEquals(team_one.player_two, null) && team_one.player_two.discord_id == discord_id ||
                    !ReferenceEquals(team_one.player_three, null) && team_one.player_three.discord_id == discord_id)
                    return true;
                return false;
            case ETeamLabel.TEAM_TWO:
                if (!ReferenceEquals(team_two.player_one, null) && team_two.player_one.discord_id == discord_id ||
                    !ReferenceEquals(team_two.player_two, null) && team_two.player_two.discord_id == discord_id ||
                    !ReferenceEquals(team_two.player_three, null) && team_two.player_three.discord_id == discord_id)
                    return true;
                return false;
            default:
                return false;
        }
    }

    // Precondition: names_not_matched == 1, teams_picked_message has 6 mentions, link_name is not empty
    private void TryInferMissingPlayer()
    {
        #region Precondition Checks
        if (names_not_matched.Count != 1)
        {
            Log("Precondition failed: names_not_matched = {0}, should be 1!", names_not_matched.Count.ToString());
            return;
        }

        if (names_not_matched[0].Length == 0)
        {
            Log("Precondition failed: names_not_matched[0].length = 0, should be > 0!");
            return;
        }

        if (teams_picked_message.mentions.Count != 6)
        {
            Log("Precondition failed: teams_picked_message.mentions.Count = {0}, should be 6!", teams_picked_message.mentions.Count.ToString());
            return;
        }
        #endregion

        var link_name = names_not_matched[0];

        foreach (var player in teams_picked_message.mentions)
        {
            bool found = false;
            foreach (var match in matched)
            {
                if (ulong.Parse(player.id) == match.discord_id)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                var lost = DDatabaseCore.GetSingleton().GetPlayerIfExists(ulong.Parse(player.id));
                if (!ReferenceEquals(lost, null))
                {
                    Log("Associated link_name {0} with player {1}!", link_name, lost.recorded_names[0]);
                    lost.recorded_names.Add(link_name);
                    if (!lost.HasName(player.name)) lost.recorded_names.Add(player.name);
                    if (!lost.HasName(player.nickname)) lost.recorded_names.Add(player.nickname);
                }
                else
                {
                    Log("There are no registered players with the link_name {0}!", link_name);
                    // create new player instance with all 3 names in the player field
                    lost = new DPlayer(ulong.Parse(player.id), player.name, player.nickname, link_name);
                    DDatabaseCore.GetSingleton().all_players.Add(lost);

                    Log("Created new player and added to queue! Name = {0}, Nickname = {1}, link_name = {2}, ID = {3}, Match ID = {4}, Team = {5}",
                        player.name, player.nickname, link_name, player.id, match_id.ToString(), unmatched_label == ETeamLabel.TEAM_ONE ? "1" : "2");
                }

                if (unmatched_label == ETeamLabel.TEAM_ONE)
                {
                    if (unmatched_player_index == 1) team_one.player_one = lost;
                    else if (unmatched_player_index == 2) team_one.player_two = lost;
                    else if (unmatched_player_index == 3) team_one.player_three = lost;
                }
                else if (unmatched_label == ETeamLabel.TEAM_TWO)
                {
                    if (unmatched_player_index == 1) team_two.player_one = lost;
                    else if (unmatched_player_index == 2) team_two.player_two = lost;
                    else if (unmatched_player_index == 3) team_two.player_three = lost;
                }

                // Reevaluate score reports after new players registered?

                bError = false;
                break;
            }
        }
    }

    public override string ToString()
    {
        var ret = "";

        ret += "Match ID = " + match_id + "\n";
        ret += "Team One = ";

        ret += !ReferenceEquals(team_one.player_one, null) ? team_one.player_one.recorded_names[0] + " (" + team_one.player_one.discord_id + "),\t" : "null,\t";
        ret += !ReferenceEquals(team_one.player_two, null) ? team_one.player_two.recorded_names[0] + " (" + team_one.player_two.discord_id + "),\t" : "null,\t";
        ret += !ReferenceEquals(team_one.player_three, null) ? team_one.player_three.recorded_names[0] + " (" + team_one.player_three.discord_id + ")\n" : "null\n";

        ret += "Team Two = ";
        ret += !ReferenceEquals(team_two.player_one, null) ? team_two.player_one.recorded_names[0] + " (" + team_two.player_one.discord_id + "),\t" : "null,\t";
        ret += !ReferenceEquals(team_two.player_two, null) ? team_two.player_two.recorded_names[0] + " (" + team_two.player_two.discord_id + "),\t" : "null,\t";
        ret += !ReferenceEquals(team_two.player_three, null) ? team_two.player_three.recorded_names[0] + " (" + team_two.player_three.discord_id + ")\n" : "null\n";

        ret += "Winner = " + winner + "\n";
        ret += "Season = " + teams_picked_message.timestamp.Value.Month + " " + teams_picked_message.timestamp.Value.Year + "\n";

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