using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Enums;
using Database.Database.Interfaces;
using Database.Database.Structs;

namespace Database.Database.DatabaseCore.Cleaners; 

public class ScoreReportCleaner : ILogger {
    private ScoreReportCleaner(in bool bUsingDirectory = false) : base(ConsoleColor.Yellow, 1, "ScoreReportCleaner") { this.bUsingDirectory = bUsingDirectory; }
    private static ScoreReportCleaner? singleton       { get; set; }
    private        FMessageList?       messages        { get; set; }
    public         bool                bIsComplete     { get; set; }
    public         bool                bUsingDirectory { get; set; }

    public static ScoreReportCleaner GetSingleton(in bool bUsingDirectory = false) {
        if (singleton == null) { singleton = new ScoreReportCleaner(bUsingDirectory); }

        return singleton;
    }

    public void ProcessChat() {
        messages = LoadMessages(DDatabaseCore.sr_path);
        if (messages == null) {
            Log("Preconditions not met! Score report chat data was not set!");

            return;
        }

        var result     = new FMessageList();
        var unverified = 0;
        var notlong    = 0;
        for (var i = 0; i < messages.messages.Count; i++) {
            var message = messages.messages[i];
            if (message.type == EMessageType.SCORE_REPORT) {
                if (IsValidLength(ref message)) {
                    if (IsMatchVerified(ref message)) {
                        CleanContentString(ref message);
                        result.messages.Add(message);
                    }
                    else { unverified++; }
                }
                else { notlong++; }
            }
        }

        Log("Unverified score reports = {0}, score reports with invalid length = {1}", unverified.ToString(), notlong.ToString());
        if (result.messages.Count > 0) {
            Log("Filtered {0} reports from {1} messages!", result.messages.Count.ToString(), messages.messages.Count.ToString());
            bIsComplete = true;
        }
        else { Log("Error cleaning score reports! No messages were filtered!"); }

        DDatabaseCore.GetSingleton().all_score_report_messages = result;
    }

    private bool IsValidLength(ref DDiscordMessage message) { return message.content != null && message.content.Length > 0; }

    private bool IsMatchVerified(ref DDiscordMessage message) {
        if (message.reactions == null || message.reactions.Count == 0) { return false; }

        foreach (var reaction in message.reactions) {
            if (reaction.emoji.name.Contains("✅")) {
                //✅
                return true;
            }
        }

        return false;
    }

    private void CleanContentString(ref DDiscordMessage message) {
        message.content = message.content.Replace("\n", " ");
        message.content = RegularExpressions.select_multiple_spaces_in_score_report_regex.Replace(message.content, " ");
    }

    private FMessageList? LoadMessages(in string override_path = "") {
        if (!bUsingDirectory) { return DDatabaseCore.GetSingleton().LoadAndGetAllDiscordChatMessages(override_path); }

        try {
            var list  = new FMessageList();
            var files = Directory.GetFiles(DDatabaseCore.sr_dir);
            if (files.Length == 0) { throw new Exception("Exception: No files found! Terminating process!"); }

            foreach (var file in files) {
                var temp = DDatabaseCore.GetSingleton().LoadAndGetAllDiscordChatMessages(file);
                if (temp == null) { throw new Exception("Exception: File path " + file + " is invalid! Terminating process!"); }

                list.messages.AddRange(temp.messages);
            }

            return list;
        }
        catch (Exception e) {
            Log(e.Message);
            Environment.Exit(1);
        }

        return null;
    }
}