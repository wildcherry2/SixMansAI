using Database.Database.DatabaseCore;
using Database.Database.DatabaseCore.Cleaners;
using Database.Database.DatabaseCore.Factories;
using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Enums;
using Database.Database.Structs;

namespace UnitTest;

[TestClass]
public class DQueueTests {
    private ChatCleaner      ccleaner            { get; set; }
    private PlayerFactory    pfactory            { get; set; }
    private List<DPlayer>    all_players         { get; set; }
    private FMessageList     filtered_messages   { get; set; }
    private DDiscordMessage? teams_picked_message { get; set; }
    public DDiscordMessage? GetTeamsPickedMessage() {
        foreach (var msg in filtered_messages.messages) {
            if (msg.embeds != null && msg.embeds.Count >= 1 && msg.GetEmbeddedDescription() == "You may now join the team channels") {
                return msg;
            }
        }

        return null;
    }

    [TestMethod, TestInitialize]
    public void Construct() {
        ccleaner = ChatCleaner.GetSingleton();
        pfactory = PlayerFactory.GetSingleton();

        ccleaner.ProcessChat();
        filtered_messages = DDatabaseCore.GetSingleton().all_discord_chat_messages;
        Assert.IsTrue(filtered_messages.messages.Count > 0);

        pfactory.ProcessChat(filtered_messages);
        all_players = DDatabaseCore.GetSingleton().all_players;
        Assert.IsTrue(all_players.Count > 0);

        teams_picked_message = GetTeamsPickedMessage();
        Assert.IsTrue(teams_picked_message != null && teams_picked_message.type == EMessageType.TEAMS_PICKED);
    }

    [TestMethod]
    public void TestConstructor() {
        var test_queue = new DQueue(teams_picked_message);
        Assert.IsNotNull(test_queue);
        Assert.IsNotNull(test_queue.team_one.player_one, "Team one, player one was null!");
        Assert.IsNotNull(test_queue.team_one.player_two, "Team one, player two was null!");
        Assert.IsNotNull(test_queue.team_one.player_three, "Team one, player three was null!");
        Assert.IsNotNull(test_queue.team_two.player_one, "Team two, player one was null!");
        Assert.IsNotNull(test_queue.team_two.player_two, "Team two, player two was null!");
        Assert.IsNotNull(test_queue.team_two.player_three, "Team two, player three was null!");
        Assert.IsTrue(test_queue.match_id != -1, "Match id not set!");

        Console.WriteLine(test_queue.ToString());
    }
}