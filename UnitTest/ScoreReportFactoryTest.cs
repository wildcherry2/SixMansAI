
using Database.Database.DatabaseCore;
using Database.Database.DatabaseCore.Cleaners;
using Database.Database.DatabaseCore.Factories;
using Database.Structs;

namespace UnitTest;

[TestClass]
public class ScoreReportFactoryTests {
    private ScoreReportCleaner sr_cleaner { get; set; }
    private ScoreReportFactory sr_factory { get; set; }
    private ChatCleaner        ccleaner   { get; set; }
    private QueueFactory       qfactory   { get; set; }
    private PlayerFactory      pfactory   { get; set; }
    //private FMessageList       clean      { get; set; }

    [TestMethod, TestInitialize]
    public void Construct() {
        sr_factory = ScoreReportFactory.GetSingleton();
        sr_cleaner = ScoreReportCleaner.GetSingleton();
        ccleaner = ChatCleaner.GetSingleton();
        qfactory = QueueFactory.GetSingleton();
        pfactory = PlayerFactory.GetSingleton();
        Assert.IsNotNull(sr_factory);
        Assert.IsNotNull(sr_cleaner);
        Assert.IsNotNull(ccleaner);
        Assert.IsNotNull(qfactory);
        Assert.IsNotNull(pfactory);

        ccleaner.ProcessChat();
       var clean = DDatabaseCore.GetSingleton().all_discord_chat_messages;
        pfactory.ProcessChat(clean);
        var all_players = DDatabaseCore.GetSingleton().all_players;
        //var qfact_res = qfactory.ProcessChat();
    }

    [TestMethod]
    public void TestProcessChat() {
        sr_cleaner.ProcessChat();
        var clean = DDatabaseCore.GetSingleton().all_score_report_messages;
        Assert.IsNotNull(clean);
        sr_factory.ProcessChat(clean);
        var res = DDatabaseCore.GetSingleton().all_score_reports;
        Assert.IsNotNull(res);
        Assert.IsTrue(res.Count > 0);

        int nullreporters = 0;
        int badids = 0;
        foreach (var report in res) {
            if (ReferenceEquals(report.reporter, null)) nullreporters++;
            if(report.iMatchId == -1) badids++;
        }

        Console.WriteLine("Null reporters = {0}, No match ids = {1}", nullreporters, badids);
    }
}