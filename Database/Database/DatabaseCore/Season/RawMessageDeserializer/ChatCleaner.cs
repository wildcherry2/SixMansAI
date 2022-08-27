
using Database.Enums;
using Database.Structs;

namespace Database.Database.DatabaseCore.Season.RawMessageDeserializer; 

public class ChatCleaner {
    private static ChatCleaner?  singleton         { get; set; }
    //private        FMessageList? all_messages      { get; set; }
    //public         FMessageList  filtered_messages { get; set; }
    private ChatCleaner() {
        //all_messages = DDatabaseCore.GetSingleton().LoadAndGetAllDiscordChatMessages(DDatabaseCore.chat_path);
        //filtered_messages = new FMessageList();
    }

    public static ChatCleaner GetSingleton() {
        if(singleton == null) singleton = new ChatCleaner();
        return singleton;
    }

    public FMessageList ProcessChat(string override_path = "") {
        var core = DDatabaseCore.GetSingleton();
        var all_messages = core.LoadAndGetAllDiscordChatMessages(override_path == "" ? DDatabaseCore.chat_path : override_path);
        var filtered_messages = new FMessageList();
        if (all_messages != null) {
            foreach (var message in all_messages.messages) {
                if (IsRelevantMessage(message)) 
                    filtered_messages.messages.Add(message);
            }
        }

        return filtered_messages;
    }

    private bool IsRelevantMessage(DDiscordMessage message) {
        return message.type != EMessageType.UNKNOWN;
    }
}