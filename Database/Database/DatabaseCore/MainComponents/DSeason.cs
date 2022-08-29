using Database.Database.Interfaces;
using Database.Structs;

namespace Database.Database.DatabaseCore.MainComponents;

public class DSeason : IDatabaseComponent
{
    public DSeason(in List<DQueue> queues) : base(ConsoleColor.Yellow, 1, "DSeason")
    {
        var ts = queues[queues.Count / 2].teams_picked_message.timestamp;
        season_label = new FSeasonLabel(ts);
        this.queues = queues;

        if (!season_label)
        {
            Log("Could not create season label for DateTime = {0}!", ts != null ? ts.ToString() : "Null DateTime");
            bError = true;
        }
    }
    private FSeasonLabel season_label { get; set; }
    private List<DQueue> queues { get; set; }

    public bool IsSeason(in FSeasonLabel label)
    {
        return season_label == label;
    }

    #region Inherited Overrides

    protected override bool IsEqual(IDatabaseComponent? rhs)
    {

        var rhs_casted = rhs != null ? rhs as DSeason : null;
        if (rhs_casted)
            return season_label.month == rhs_casted.season_label.month && season_label.year == rhs_casted.season_label.year;
        return false;
    }
    protected override bool IsLessThan(IDatabaseComponent? rhs)
    {
        var rhs_casted = rhs != null ? rhs as DSeason : null;
        if (rhs_casted)
        {
            /*
             *
             *  COMPARE DATES
             *
             */
            return true;
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