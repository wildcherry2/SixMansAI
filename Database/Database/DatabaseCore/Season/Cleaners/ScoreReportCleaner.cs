using Database.Enums;
using Database.Structs;

namespace Database.Database.DatabaseCore.Season.Cleaners; 

public class ScoreReportCleaner : ILogger {
    private static ScoreReportCleaner? singleton   { get; set; }
    private        FMessageList?       messages    { get; set; }
    public         bool                bIsComplete { get; set; } = false;
    private ScoreReportCleaner() : base(ConsoleColor.Yellow, 1, "ScoreReportCleaner"){
        messages = DDatabaseCore.GetSingleton().LoadAndGetAllDiscordChatMessages(DDatabaseCore.sr_path);
    }

    public static ScoreReportCleaner GetSingleton() {
        if(singleton == null) singleton = new ScoreReportCleaner();
        return singleton;
    }

    public void ProcessChat() {
        if (messages == null) {
            Log("Preconditions not met! Score report chat data was not set!");
            return;
        }
        FMessageList result = new FMessageList();

        int unverified = 0;
        int notlong = 0;
        for (int i = 0; i < messages.messages.Count; i++) {
            var message = messages.messages[i];
            if (message.type == EMessageType.SCORE_REPORT) {
                if (IsValidLength(ref message)) {
                    if (IsMatchVerified(ref message)) {
                        CleanContentString(ref message);
                        result.messages.Add(message);
                    }
                    else {
                        unverified++;
                    }
                }
                else {
                    notlong++;
                }
            }
        }

        Log("Unverified score reports = {0}, score reports with invalid length = {1}", unverified.ToString(), notlong.ToString());
        if (result.messages.Count > 0) bIsComplete = true;
        else Log("Error cleaning score reports! No messages were filtered!");
        DDatabaseCore.GetSingleton().all_score_report_messages = result;
    }

    private bool IsValidLength(ref DDiscordMessage message) {
        return message.content != null && message.content.Length > 0;
    }

    private bool IsMatchVerified(ref DDiscordMessage message) {
        if (message.reactions == null || message.reactions.Count == 0) return false;

        foreach (var reaction in message.reactions) {

            if (reaction.emoji.name.Contains("✅")) { //✅
                return true;
            }
            
        }

        return false;
    }

    private void CleanContentString(ref DDiscordMessage message) {
        message.content = message.content.Replace("\n", " ");
        message.content = RegularExpressions.select_multiple_spaces_in_score_report_regex.Replace(message.content, " ");
    }
}