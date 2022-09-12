using Database.Database.DatabaseCore;
using Database.Database.DatabaseCore.Binders;
using Database.Database.DatabaseCore.Cleaners;
using Database.Database.DatabaseCore.Factories;

namespace UnitTest;

[TestClass]
public class QueueReportBinderTests {
    [TestMethod, TestInitialize]
    public void Construct() {
        var core = DDatabaseCore.GetSingleton();
        ChatCleaner.GetSingleton().ProcessChat();
        ScoreReportCleaner.GetSingleton().ProcessChat();
        PlayerFactory.GetSingleton().ProcessChat(core.all_discord_chat_messages);
        //ScoreReportFactory.GetSingleton().ProcessChat(core.all_score_report_messages);
        QueueFactory.GetSingleton(core.all_discord_chat_messages).ProcessChat();
        ScoreReportFactory.GetSingleton().ProcessChat(core.all_score_report_messages); //putting this after player factory and queue factory could give the score report factory more names to work with
        QueueReportBinder.GetSingleton();
        
        Assert.AreNotEqual(false, ChatCleaner.GetSingleton().bIsComplete);
        Assert.AreNotEqual(false, ScoreReportCleaner.GetSingleton().bIsComplete);
        Assert.AreNotEqual(false, PlayerFactory.GetSingleton().bIsComplete);
        Assert.AreNotEqual(false, ScoreReportFactory.GetSingleton().bIsComplete);
        Assert.AreNotEqual(false, QueueFactory.GetSingleton().bIsComplete);

        Assert.IsNotNull(core.all_discord_chat_messages);
        Assert.IsNotNull(core.all_score_report_messages);
        Assert.IsNotNull(core.all_players);
        Assert.IsNotNull(core.all_queues);
        Assert.IsNotNull(core.all_score_reports);
        Assert.IsNotNull(QueueReportBinder.GetSingleton());

        Assert.AreNotEqual(0, core.all_discord_chat_messages.messages.Count);
        Assert.AreNotEqual(0, core.all_score_report_messages.messages.Count);
        Assert.AreNotEqual(0, core.all_players.Count);
        Assert.AreNotEqual(0, core.all_queues.Count);
        Assert.AreNotEqual(0, core.all_score_reports.Count);
    }

    [TestMethod]
    public void TestBindReportsToQueues() {
        QueueReportBinder.GetSingleton().BindReportsToQueues();

        Assert.AreNotEqual(false, QueueFactory.GetSingleton().bIsComplete);
        Assert.IsNotNull(DDatabaseCore.GetSingleton().all_queues);
        Assert.AreNotEqual(0, DDatabaseCore.GetSingleton().all_queues.Count);

        foreach (var queue in DDatabaseCore.GetSingleton().all_queues) {
            Assert.IsNotNull(queue);
            Assert.IsTrue(queue.bError || queue.score_report != null);

            Assert.IsTrue(queue.bError || queue.match_id > 0);
            Assert.IsTrue(queue.score_report.bError || queue.score_report.iMatchId > 0);
            if(queue) Console.WriteLine(queue.ToString());
        }
        
    }
}