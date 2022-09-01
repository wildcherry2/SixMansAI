using Database.Database.Interfaces;
using Database.Structs;

namespace Database.Database.DatabaseCore.MainComponents;

public class DPlayer : IDatabaseComponent {
    public DPlayer(in ulong discord_id, in string discord_name, in string nickname = "", in string link_name = "") : base(ConsoleColor.Yellow, 1, "DPlayer") {
        this.discord_id = discord_id;
        game_history    = new List<FGameRecord?>();
        recorded_names  = new List<string>();
        recorded_names.Add(discord_name);
        if (nickname.Length > 0 && nickname != discord_name) recorded_names.Add(nickname);
        if (link_name.Length > 0 && link_name != discord_name && link_name != nickname) recorded_names.Add(link_name);
    }
    public ulong              discord_id     { get; init; }
    public List<string>       recorded_names { get; set; }
    public List<FGameRecord?> game_history   { get; set; }
    public int                iTotalWins     { get; set; } = 0;
    public int                iTotalLosses   { get; set; } = 0;

    public bool HasName(in string name) {
        foreach (var player_name in recorded_names)
            if (name == player_name)
                return true;

        return false;
    }

    public void TryAddName(in string name) {
        var found = false;
        foreach (var player_name in recorded_names)
            if (name == player_name) {
                found = true;

                break;
            }

        if (!found) recorded_names.Add(name);
    }


    #region Inherited Overrides

    // Primary key is the Discord user ID
    public override PrimaryKey TryGetOrCreatePrimaryKey() {
        if (bIsPrimaryKeySet) return primary_key;
        primary_key = new PrimaryKey(discord_id, EPrimaryKeyType.PLAYER);
        default_incremental_primary_key--;
        bIsPrimaryKeySet = true;
        return primary_key;
    }

    #endregion
}