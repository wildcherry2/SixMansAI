using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Enums;
using Database.Database.Interfaces;
using Database.Database.Structs;

namespace Database.Database.DatabaseCore.Cleaners; 

public class ChatCleaner : ILogger {
    private ChatCleaner(in bool bUsingDirectory = false) : base(ConsoleColor.Yellow, 1, "ChatCleaner") { this.bUsingDirectory = bUsingDirectory; }
    private static ChatCleaner? singleton       { get; set; }
    public         bool         bIsComplete     { get; set; }
    public         bool         bUsingDirectory { get; set; }
    public static ChatCleaner GetSingleton(in bool bUsingDirectory = false) {
        if (singleton == null) { singleton = new ChatCleaner(bUsingDirectory); }

        return singleton;
    }
    public void ProcessChat(in string override_path = "") {
        var core              = DDatabaseCore.GetSingleton();
        var all_messages      = LoadMessages(override_path == "" ? DDatabaseCore.chat_path : override_path);
        var filtered_messages = new FMessageList();
        if (all_messages != null) {
            foreach (var message in all_messages.messages) {
                if (IsRelevantMessage(message)) { filtered_messages.messages.Add(message); }
            }
        }
        else { Log("Preconditions not met! Score report chat data was not set!"); }

        if (filtered_messages.messages.Count > 0) { bIsComplete = true; }
        else { Log("Error cleaning chat data! No messages were filtered!"); }

        core.all_discord_chat_messages = filtered_messages;
    }
    private bool IsRelevantMessage(in DDiscordMessage message) { return message.type != EMessageType.UNKNOWN; }

    private FMessageList? LoadMessages(in string override_path = "") {
        if (!bUsingDirectory) { return DDatabaseCore.GetSingleton().LoadAndGetAllDiscordChatMessages(override_path); }

        try {
            var list  = new FMessageList();
            var files = Directory.GetFiles(DDatabaseCore.chat_dir);
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