using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Structs;

namespace Database.Database.DatabaseCore.Factories;

public class SeasonFactory
{
    private static SeasonFactory? singleton { get; set; }
    private FMessageList? chat_messages { get; set; }

    private SeasonFactory(FMessageList? chat = null)
    {
        if (chat == null)
            chat_messages = DDatabaseCore.GetSingleton().LoadAndGetAllDiscordChatMessages(DDatabaseCore.chat_path);
        else
            chat_messages = chat;
    }

    public SeasonFactory GetSingleton(FMessageList? chat = null)
    {
        if (singleton == null) singleton = new SeasonFactory(chat_messages);
        else if (chat != null) singleton.chat_messages = chat;
        return singleton;
    }

    public List<DSeason> ProcessChat()
    {
        var ret = new List<DSeason>();

        //if (chat_messages != null) {
        //    var previous_id = 0;
        //    var previous_id_index = -1;
        //    var season_begin_index = 0;
        //    for (int i = 0; i < chat_messages.messages.Count; i++) {
        //        var message = chat_messages.messages[i];
        //        if (message.type == EMessageType.TEAMS_PICKED) {
        //            var current_id = message.GetMatchId();
        //            if (current_id < previous_id) {
        //                var season_end_index = previous_id_index;

        //                var sub_list = new FMessageList { messages = chat_messages.messages.GetRange(season_begin_index, season_end_index - season_begin_index + 1) };
        //                var qf = QueueFactory.GetSingleton(sub_list);
        //                ret.Add(new DSeason(new FSeasonLabel(chat_messages.messages[previous_id_index].timestamp), qf.ProcessChat()));

        //                season_begin_index = i;
        //                previous_id_index = -1;
        //                previous_id = 0;
        //            }
        //            else {
        //                previous_id = current_id;
        //                previous_id_index = i;
        //            }
        //        }
        //    }
        //}

        return ret;
    }
}