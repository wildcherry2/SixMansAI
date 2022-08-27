using Database.Enums;
using Database.Structs;

namespace Database.Database.DatabaseCore.Season.Queue;

public class QueueFactory {
    private static QueueFactory? singleton    { get; set; }
    private        FMessageList? _messageList { get; set; }
    private QueueFactory() {
        _messageList = DDatabaseCore.GetSingleton().LoadAndGetAllDiscordChatMessages(DDatabaseCore.chat_path);
    }

    public QueueFactory GetSingleton() {
        if (singleton == null) singleton = new QueueFactory();
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


        return ret;
    }
}