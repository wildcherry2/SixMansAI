using Database.Database.DatabaseCore;
using Database.Database.DatabaseCore.Season.Cleaners;
using Database.Enums;

namespace UnitTest;

[TestClass]
public class ScoreReportCleanerTests {
    private ScoreReportCleaner sr_cleaner { get; set; }

    [TestMethod, TestInitialize]
    public void Construct() {
        sr_cleaner = ScoreReportCleaner.GetSingleton();
        Assert.IsNotNull(sr_cleaner);
    }

    [TestMethod]
    public void TestProcessChat() {
        sr_cleaner.ProcessChat();
        var ret = DDatabaseCore.GetSingleton().all_score_report_messages;
        Assert.IsNotNull(ret);
        Assert.IsFalse(ret.messages.Count == 0);

        foreach (var chat in ret.messages) {
            Assert.IsNotNull(chat);
            Assert.IsNotNull(chat.content);
            Assert.IsTrue(chat.type == EMessageType.SCORE_REPORT);
            Assert.IsTrue(RegularExpressions.score_report_strict_regex.Match(chat.content).Success);
        }
    }
}