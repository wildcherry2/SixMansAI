using Database.Database.DatabaseCore;
using Database.Database.DatabaseCore.Season.Queue;
using Database.Database.DatabaseCore.Season.RawMessageDeserializer;
using Database.Structs;

namespace UnitTest;

[TestClass]
public class QueueFactoryTests {
    private QueueFactory  qf                { get; set; }
    private ChatCleaner   ccleaner          { get; set; }
    private PlayerFactory pfactory          { get; set; }
    private FMessageList  filtered_messages { get; set; }
    private List<DPlayer> all_players       { get; set; }

    [TestMethod, TestInitialize]
    public void Construct() {
        ccleaner = ChatCleaner.GetSingleton();
        pfactory = PlayerFactory.GetSingleton();

        filtered_messages = ccleaner.ProcessChat();
        Assert.IsTrue(filtered_messages.messages.Count > 0);

        all_players = pfactory.ProcessChat(filtered_messages);
        Assert.IsTrue(all_players.Count > 0);

        qf = QueueFactory.GetSingleton(filtered_messages);
        Assert.IsNotNull(qf);
    }
    [TestMethod]
    public void TestProcessChat() {
        var res = qf.ProcessChat();
        Assert.IsNotNull(res);
        Assert.IsTrue(res.Count > 0);

        int partials = 0;
        int nothing = 0;
        foreach (var queue in res) {
            if (!queue.bError) {
                Assert.IsNotNull(queue);
                Assert.IsNotNull(queue.team_one.player_one);
                Assert.IsNotNull(queue.team_one.player_two);
                Assert.IsNotNull(queue.team_one.player_three);
                Assert.IsNotNull(queue.team_two.player_one);
                Assert.IsNotNull(queue.team_two.player_two);
                Assert.IsNotNull(queue.team_two.player_three);
                Assert.IsTrue(queue.match_id > 0);
            }
            else if(queue.not_matched.Count > 0) {
                string build = "Match {0} is missing player(s): ";
                for (int i = 0; i < queue.not_matched.Count; i++) {
                    build += queue.not_matched[i] + "\t";
                }

                Console.WriteLine(build, queue.match_id);
                partials++;
            }
            else {
                nothing++;
            }
        }

        Console.WriteLine("Partially built queues = {0}, Useless queues = {1}", partials, nothing);

        Assert.AreEqual(0, nothing);
    }
}