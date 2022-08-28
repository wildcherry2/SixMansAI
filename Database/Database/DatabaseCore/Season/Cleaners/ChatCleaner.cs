﻿using Database.Enums;
using Database.Structs;

namespace Database.Database.DatabaseCore.Season.Cleaners; 

public class ChatCleaner {
    private static ChatCleaner? singleton   { get; set; }
    public         bool         bIsComplete { get; set; } = false;

    private ChatCleaner() {}

    public static ChatCleaner GetSingleton() {
        if(singleton == null) singleton = new ChatCleaner();
        return singleton;
    }

    public void ProcessChat(string override_path = "") {
        var core = DDatabaseCore.GetSingleton();
        var all_messages = core.LoadAndGetAllDiscordChatMessages(override_path == "" ? DDatabaseCore.chat_path : override_path);
        var filtered_messages = new FMessageList();
        if (all_messages != null) {
            foreach (var message in all_messages.messages) {
                if (IsRelevantMessage(message)) 
                    filtered_messages.messages.Add(message);
            }
        }

        if (filtered_messages.messages.Count > 0) bIsComplete = true;
        core.all_discord_chat_messages = filtered_messages;
    }

    private bool IsRelevantMessage(DDiscordMessage message) {
        return message.type != EMessageType.UNKNOWN;
    }
}