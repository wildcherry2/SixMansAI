using Database.Database.DatabaseCore.Cleaners;
using Database.Database.DatabaseCore.MainComponents;
using Database.Enums;
using Database.Structs;

namespace Database.Database.DatabaseCore.Factories;

public class QueueFactory : FactoryBase {
    private QueueFactory(in FMessageList? season_message_list = null) { _messageList = DDatabaseCore.GetSingleton().all_discord_chat_messages; }
    private static QueueFactory? singleton    { get; set; }
    private        FMessageList? _messageList { get; set; }
    public         bool          bIsComplete  { get; private set; }

    public static QueueFactory GetSingleton(in FMessageList? season_message_list = null) {
        if (singleton == null)
            singleton                                                = new QueueFactory(season_message_list);
        else if (season_message_list != null) singleton._messageList = season_message_list;

        return singleton;
    }

    // Precondition: ChatCleaner.ProcessChat and PlayerFactory.ProcessChat must have already been called
    // Postcondition: DDatabaseCore's singleton's all_queues field is initialized with data
    public void ProcessChat() {
        var ret = new List<DQueue>();
        if (ChatCleaner.GetSingleton().bIsComplete && PlayerFactory.GetSingleton().bIsComplete && _messageList != null && _messageList.messages != null) {
            foreach (var message in _messageList.messages)
                if (message.type == EMessageType.TEAMS_PICKED) {
                    var queue = new DQueue(message);
                    queue.TryGetOrCreatePrimaryKey();
                    ret.Add(queue);
                }

            logger.Log("Created {0} queues!", ret.Count.ToString());
            bIsComplete = true;
        }
        else {
            logger.Log("Preconditions not met! Preconditions status:\nChatCleaner.bIsComplete = {0}\nPlayerFactory.bIsComplete = {1}\nmessage_list = {2}",
                       ChatCleaner.GetSingleton().bIsComplete.ToString(), PlayerFactory.GetSingleton().bIsComplete.ToString(),
                       _messageList != null ? "Not null" : "Null");
        }

        DDatabaseCore.GetSingleton().all_queues = ret;
    }
}