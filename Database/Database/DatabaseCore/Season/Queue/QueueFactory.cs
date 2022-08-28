using Database.Database.DatabaseCore.Season.Cleaners;
using Database.Enums;
using Database.Structs;

namespace Database.Database.DatabaseCore.Season.Queue;

public class QueueFactory : ILogger {
    private static QueueFactory? singleton    { get; set; }
    private        FMessageList? _messageList { get; set; }
    public         bool          bIsComplete  { get; set; } = false;
    private QueueFactory(FMessageList? season_message_list = null) : base(ConsoleColor.Yellow, 1, "QueueFactory"){
        if (season_message_list == null)
            _messageList = DDatabaseCore.GetSingleton().LoadAndGetAllDiscordChatMessages(DDatabaseCore.chat_path);
        else
            _messageList = season_message_list;
    }

    public static QueueFactory GetSingleton(FMessageList? season_message_list = null) {
        if (singleton == null) singleton = new QueueFactory(season_message_list);
        else if(season_message_list != null) singleton._messageList = season_message_list;
        return singleton;
    }

    // Precondition: ChatCleaner.ProcessChat and PlayerFactory.ProcessChat must have already been called
    // Postcondition: DDatabaseCore's singleton's all_queues field is initialized with data
    public void ProcessChat() {
        var ret = new List<DQueue>();
        if (ChatCleaner.GetSingleton().bIsComplete && PlayerFactory.GetSingleton().bIsComplete && _messageList != null && _messageList.messages != null) {
            foreach (var message in _messageList.messages) {
                if (message.type == EMessageType.TEAMS_PICKED) {
                    ret.Add(new DQueue(message));
                }
            }

            bIsComplete = true;
        }
        else {
            Log("Preconditions not met! Preconditions status:\nChatCleaner.bIsComplete = {0}\nPlayerFactory.bIsComplete = {1}\nmessage_list = {2}",
                ChatCleaner.GetSingleton().bIsComplete.ToString(), PlayerFactory.GetSingleton().bIsComplete.ToString(),
                _messageList != null ? "Not null" : "Null");
        }

        DDatabaseCore.GetSingleton().all_queues = ret;
    }
}