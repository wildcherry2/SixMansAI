
using Database.Database.DatabaseCore;
using Database.Database.DatabaseCore.Season.RawMessageDeserializer;
using Database.Enums;

namespace UnitTest;

[TestClass]
public class ChatCleanerTests {
    public ChatCleaner cleaner { get; set; }
    [TestMethod, TestInitialize]
    public void TestIfSingletonIsCreated() {
        cleaner = ChatCleaner.GetSingleton();
        Assert.IsNotNull(cleaner);
    }

    [TestMethod]
    public void TestProcessChat() {
        var result = cleaner.ProcessChat();
        Assert.IsNotNull(result);

        foreach (var chat in result.messages) {
            Assert.IsTrue(chat.type != EMessageType.UNKNOWN);
            if (chat.type == EMessageType.PLAYER_Q || chat.type == EMessageType.PLAYER_LEAVE || chat.type == EMessageType.SCORE_REPORT) {
                Assert.IsTrue(chat.content != null);
                Assert.IsTrue(chat.content.Length > 0);
                Assert.IsTrue(chat.content == "!q" || chat.content == "!leave" || RegularExpressions.score_report_relaxed_regex.Match(chat.content).Success);
            }
            else {
                Assert.IsTrue(chat.embeds != null && (chat.type != EMessageType.BOT_LOBBY_CANCELLED ? chat.embeds.Count > 0 : chat.embeds.Count == 0), chat.type.ToString() + ", " + chat.GetMatchId() + ", " + (chat.embeds != null).ToString() + ", " + chat.embeds.Count.ToString());
            }
        }
    }
}