using Database.Database.Interfaces;
using Database.Structs;

namespace Database.Database.DatabaseCore.MainComponents;

public class DSeason : IDatabaseComponent {
    public DSeason(in List<DQueue> queues) {
        var ts = queues[queues.Count / 2].teams_picked_message.timestamp;
        season_label = new FSeasonLabel(ts);
        this.queues  = queues;
        if (!season_label) {
            logger.Log("Could not create season label for DateTime = {0}!", ts != null ? ts.ToString() : "Null DateTime");
            bError = true;
        }
    }
    private FSeasonLabel season_label { get; }
    private List<DQueue> queues       { get; }

    public bool IsSeason(in FSeasonLabel label) { return season_label == label; }

    #region Inherited Overrides

    //public override PrimaryKey TryGetOrCreatePrimaryKey() {
    //    if (bIsPrimaryKeySet) return primary_key;
    //    primary_key = new PrimaryKey(discord_id, EPrimaryKeyType.PLAYER);
    //    default_incremental_primary_key--;
    //    bIsPrimaryKeySet = true;
    //    return primary_key;
    //}

    #endregion
}