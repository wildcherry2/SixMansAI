using Database.Enums;
using Database.Structs;

namespace Database.Database.DatabaseCore.Season.Queue;

public class DQueue : IDatabaseComponent
{
    public int match_id { get; set; } = -1;
    public FTeam team_one { get; set; }
    public FTeam team_two { get; set; }
    public FScoreReport score_report { get; set; }

    public List<DDiscordMessage> raw_messages_in_queue;
    public ETeamLabel winner { get; set; } = ETeamLabel.NOT_SET;

    public DQueue(int match_id, ref List<DDiscordMessage> raw_messages_in_queue) : base(ConsoleColor.Cyan, 2, "DQueue")
    {
        this.match_id = match_id;
        this.raw_messages_in_queue = raw_messages_in_queue;
        team_one = new FTeam();
        team_two = new FTeam();
        score_report = new FScoreReport();
    }

    public DQueue(DDiscordMessage teams_picked_message) {
        if (teams_picked_message.type == EMessageType.TEAMS_PICKED) {
            match_id = teams_picked_message.GetMatchId();
            team_one = new FTeam(); //TODO: replace with GetTeamOne once gathering all DPlayers is implemented
            team_two = new FTeam();
            score_report = new FScoreReport(); // maybe parse names and score reports before chat so that we can go ahead and set it and winner now?}
        }
        else
            bError = true; // TODO: add logging for every failed constructor in addition to setting bError
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