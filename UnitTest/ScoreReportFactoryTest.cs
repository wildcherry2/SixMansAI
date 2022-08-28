
using Database.Database.DatabaseCore;
using Database.Database.DatabaseCore.Season.Cleaners;
using Database.Database.DatabaseCore.Season.Queue;

namespace UnitTest; 

[TestClass]
public class ScoreReportFactoryTests {
    private ScoreReportCleaner sr_cleaner { get; set; }
    private ScoreReportFactory sr_factory { get; set; }
    private ChatCleaner        ccleaner   { get; set; }
    private QueueFactory       qfactory   { get; set; }
    private PlayerFactory      pfactory   { get; set; }

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

        var chat_clean_res = ccleaner.ProcessChat();
        var all_players = pfactory.ProcessChat(chat_clean_res);
        //var qfact_res = qfactory.ProcessChat();
    }

    [TestMethod]
    public void TestProcessChat() {
        var clean = sr_cleaner.ProcessChat();
        Assert.IsNotNull(clean);
        var res = sr_factory.ProcessChat(clean);
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