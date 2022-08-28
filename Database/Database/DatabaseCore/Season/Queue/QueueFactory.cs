using Database.Enums;
using Database.Structs;

namespace Database.Database.DatabaseCore.Season.Queue;

public class QueueFactory {
    private static QueueFactory? singleton    { get; set; }
    private        FMessageList? _messageList { get; set; }
    private QueueFactory(FMessageList? season_message_list = null) {
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

    public List<DQueue> ProcessChat() {
        var ret = new List<DQueue>();
        if (_messageList != null && _messageList.messages != null) {
            foreach (var message in _messageList.messages) {
                if (message.type == EMessageType.TEAMS_PICKED) {
                    ret.Add(new DQueue(message));
                }
            }
        }

        DDatabaseCore.GetSingleton().all_queues = ret;
        return ret;
    }
}