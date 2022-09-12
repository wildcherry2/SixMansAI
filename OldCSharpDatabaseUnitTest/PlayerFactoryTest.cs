using Database.Database.DatabaseCore;
using Database.Database.DatabaseCore.Factories;

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
        factory.ProcessChat(DDatabaseCore.GetSingleton().all_discord_chat_messages);
        var res = DDatabaseCore.GetSingleton().all_players;
        foreach (var player in res) {
            Assert.IsNotNull(player);
            Assert.IsTrue(player.recorded_names.Count > 0);
            Assert.IsTrue(player.discord_id != 0);
        }
    }
}