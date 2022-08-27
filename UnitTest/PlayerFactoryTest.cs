using Database.Database.DatabaseCore;
using Database.Database.DatabaseCore.Season.RawMessageDeserializer;

namespace UnitTest;

[TestClass]
public class PlayerFactoryTests {
    public PlayerFactory factory { get; set; }

    [TestMethod, TestInitialize]
    public void TestIfSingletonIsCreated() {
        factory = PlayerFactory.GetSingleton();
        Assert.IsNotNull(factory);
    }

    [TestMethod]
    public void TestProcessChat() {
        var res = factory.ProcessChat(ChatCleaner.GetSingleton().ProcessChat());
        foreach (var player in res) {
            Assert.IsNotNull(player);
            Assert.IsTrue(player.recorded_names.Count > 0);
            Assert.IsTrue(player.discord_id != 0);
        }
    }
}