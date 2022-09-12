using Database.Database.DatabaseCore;
using Database.Database.DatabaseCore.Binders;
using Database.Database.DatabaseCore.Cleaners;
using Database.Database.DatabaseCore.Factories;

namespace UnitTest;

[TestClass]
public class PlayerRecordBinderTests {
    [TestMethod, TestInitialize]
    public void Construct() {
        var core = DDatabaseCore.GetSingleton();
        ChatCleaner.GetSingleton().ProcessChat();
        ScoreReportCleaner.GetSingleton().ProcessChat();
        PlayerFactory.GetSingleton().ProcessChat(core.all_discord_chat_messages);
        ScoreReportFactory.GetSingleton().ProcessChat(core.all_score_report_messages);
        QueueFactory.GetSingleton(core.all_discord_chat_messages).ProcessChat();
        QueueReportBinder.GetSingleton().BindReportsToQueues();
        PlayerRecordBinder.GetSingleton();

        Assert.AreNotEqual(false, ChatCleaner.GetSingleton().bIsComplete);
        Assert.AreNotEqual(false, ScoreReportCleaner.GetSingleton().bIsComplete);
        Assert.AreNotEqual(false, PlayerFactory.GetSingleton().bIsComplete);
        Assert.AreNotEqual(false, ScoreReportFactory.GetSingleton().bIsComplete);
        Assert.AreNotEqual(false, QueueFactory.GetSingleton().bIsComplete);
        Assert.AreNotEqual(false, QueueReportBinder.GetSingleton().bIsComplete);

        Assert.IsNotNull(core.all_discord_chat_messages);
        Assert.IsNotNull(core.all_score_report_messages);
        Assert.IsNotNull(core.all_players);
        Assert.IsNotNull(core.all_queues);
        Assert.IsNotNull(core.all_score_reports);
        Assert.IsNotNull(PlayerRecordBinder.GetSingleton());

        Assert.AreNotEqual(0, core.all_discord_chat_messages.messages.Count);
        Assert.AreNotEqual(0, core.all_score_report_messages.messages.Count);
        Assert.AreNotEqual(0, core.all_players.Count);
        Assert.AreNotEqual(0, core.all_queues.Count);
        Assert.AreNotEqual(0, core.all_score_reports.Count);
    }

    [TestMethod]
    public void TestBindRecordsToPlayers() {
        PlayerRecordBinder.GetSingleton().BindRecordsToPlayers();
        Assert.IsTrue(PlayerRecordBinder.GetSingleton().bComplete);

        foreach (var player in DDatabaseCore.GetSingleton().all_players) {
            int wins = 0;
            int losses = 0;

            foreach (var record in player.game_history) {
                if (record == null) break;

                if (record.queue.IsPlayerInTeam(record.team, player.discord_id)) {
                    if (record.bPlayerWon) {
                        Assert.IsTrue(record.queue.winner == record.team);
                        wins++;
                    }
                    else {
                        Assert.IsTrue(record.queue.winner != record.team);
                        losses++;
                    }
                }
            }

            Assert.AreEqual(wins, player.iTotalWins);
            Assert.AreEqual(losses, player.iTotalLosses);
        }
    }
}