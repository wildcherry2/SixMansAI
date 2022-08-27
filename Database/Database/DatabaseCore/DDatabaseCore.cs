
using System.Text.Json;
using Database.Database.DatabaseCore.Season;
using Database.Structs;

namespace Database.Database.DatabaseCore; 

public class DDatabaseCore : IDatabaseComponent {
    public         List<DPlayer>          all_players          { get; set; }
    public         List<DSeason>          all_seasons          { get; set; }
    public         FMessageList all_discord_chat_messages { get; set; }
    private static DDatabaseCore?         singleton;

    // Will move to CLoader
    public static string chat_path = @"";

    private DDatabaseCore() : base(ConsoleColor.Green, 0, "DDatabaseCore") {
        all_players = new List<DPlayer>();
        all_seasons = new List<DSeason>();
    }

    public static DDatabaseCore GetSingleton() {
        if(!singleton)
            singleton = new DDatabaseCore();

        return singleton;
    }

    // Will later use CLoader
    public FMessageList? LoadAndGetAllDiscordChatMessages(string path) {
        FMessageList? list = null;

        try {
            list = JsonSerializer.Deserialize<FMessageList>(path);
            if (list == null || list.messages == null) return null;
        }
        catch (Exception e) {
            Log("Deserialization exception for file {0}, message = {1}", path, e.Message);
            return null;
        }

        all_discord_chat_messages = list;
        return list;
    }

    public DPlayer? GetPlayerIfExists(ulong discord_id) {
        foreach (var player in all_players) {
            if (player.discord_id == discord_id) return player;
        }

        return null;
    }

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