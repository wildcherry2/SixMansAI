using Database.Database.DatabaseCore;

namespace UnitTest;

[TestClass]
public class DDatabaseCoreTests {
    [TestMethod]
    public void TestInitialize() {
        var single = DDatabaseCore.GetSingleton();
        Assert.IsNotNull(single);
        single.BuildDatabase();

        Assert.IsNotNull(single.all_discord_chat_messages);
        Assert.IsNotNull(single.all_score_report_messages);
        Assert.IsNotNull(single.all_players);
        Assert.IsNotNull(single.all_queues);
        Assert.IsNotNull(single.all_score_reports);

    }
}