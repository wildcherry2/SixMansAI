
using System.Text.Json;
using Database.Database.DatabaseCore.Binders;
using Database.Database.DatabaseCore.Cleaners;
using Database.Database.DatabaseCore.Factories;
using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Interfaces;
using Database.Structs;

namespace Database.Database.DatabaseCore;

public class DDatabaseCore : IDatabaseComponent {
    public         List<DPlayer>?      all_players               { get; set; }
    public         List<DSeason>?      all_seasons               { get; set; }
    public         List<FScoreReport>? all_score_reports         { get; set; }
    public         List<DQueue>?       all_queues                { get; set; }
    public         FMessageList        all_discord_chat_messages { get; set; }
    public         FMessageList        all_score_report_messages { get; set; }
    private static DDatabaseCore?      singleton                 { get; set; }

    // Will move to CLoader
    public static string chat_path { get; set; } = @"C:\Users\tyler\Documents\Programming\AI\SixMans\RawData\rank-b\July2022.json";
    public static string sr_path   { get; set; } = @"C:\Users\tyler\Documents\Programming\AI\SixMans\RawData\score-report\July2022.json";
    public static string chat_dir  { get; set; } = @"C:\Users\tyler\Documents\Programming\AI\SixMans\RawData\rank-b";
    public static string sr_dir    { get; set; } = @"C:\Users\tyler\Documents\Programming\AI\SixMans\RawData\score-report";

    private DDatabaseCore() : base(ConsoleColor.Green, 0, "DDatabaseCore") {
        all_players = new List<DPlayer>();
        all_seasons = new List<DSeason>();
    }

    public static DDatabaseCore GetSingleton() {
        if(ReferenceEquals(singleton, null)) singleton = new DDatabaseCore();

        return singleton;
    }

    // Look into situations in QueueReportBinder where reporter is true but 
    public void BuildDatabase(in bool bUsingDirectory = false) {
        ChatCleaner.GetSingleton(true).ProcessChat();
        ScoreReportCleaner.GetSingleton(true).ProcessChat();
        PlayerFactory.GetSingleton().ProcessChat(all_discord_chat_messages);
        QueueFactory.GetSingleton(all_discord_chat_messages).ProcessChat();
        ScoreReportFactory.GetSingleton().ProcessChat(all_score_report_messages);
        QueueReportBinder.GetSingleton().BindReportsToQueues();
    }

    // Will later use CLoader
    public FMessageList? LoadAndGetAllDiscordChatMessages(in string path) {
        FMessageList? list = null;
        try {
            list = JsonSerializer.Deserialize<FMessageList>(new StreamReader(path).ReadToEnd());
            if (list == null || list.messages == null) return null;
        }
        catch (Exception e) {
            Log("Deserialization exception for file {0}, message = {1}", path, e.Message);
            return null;
        }

        //all_discord_chat_messages = list;
        return list;
    }

    // TODO: move to cquerier
    public DPlayer? GetPlayerIfExists(in ulong discord_id) {
        if (discord_id == 0 || all_players == null) return null;
        foreach (var player in all_players) {
            if (player.discord_id == discord_id) 
                return player;
        }

        return null;
    }

        // TODO: move to cquerier
    public DPlayer? GetPlayerIfExists(in string name) {
        if (name.Length == 0 || all_players == null) return null;
        foreach (var player in all_players) {
            foreach (var this_name in player.recorded_names) {
                if(this_name == name) 
                    return player;
            }
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