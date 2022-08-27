using System.Text.Json.Serialization;
using Database.Database.DatabaseCore.Season;
using Database.Enums;
using Database.Structs;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace UnitTest;
[TestClass]
public class DDiscordMessageTests {
    public DDiscordMessageTests() {
        Construct();
    }
    public class MsgList {
        [JsonProperty("messages")] public List<DDiscordMessage>? messages { get; set; } = new List<DDiscordMessage>();
    }

    public MsgList? messages { get; set; }

    public DDiscordMessage? GetLeaveMessage() {
        foreach (var msg in messages.messages) {
            if (msg.content == "!leave") return msg;
        }

        return null;
    }

    public DDiscordMessage? GetBotResponseLeaveMessage() {
        foreach (var msg in messages.messages) {
            if (msg.embeds != null && msg.embeds.Count >= 1 && msg.GetEmbeddedDescription() == "[Fattyy-_-](https://www.rl6mans.com/profile/Fattyy-_-) has left (using command).") {
                return msg;
            }
        }

        return null;
    }

    public DDiscordMessage? GetTeamsPickedMessage() {
        foreach (var msg in messages.messages) {
            if (msg.embeds != null && msg.embeds.Count >= 1 && msg.GetEmbeddedDescription() == "You may now join the team channels") {
                return msg;
            }
        }

        return null;
    }

    [TestMethod]
    public void Construct() {
        var chat = "C:\\Users\\tyler\\Documents\\Programming\\AI\\SixMans\\RawData\\rank-b\\July2022.json";
        string json = new StreamReader(chat).ReadToEnd();
        messages = JsonSerializer.Deserialize<MsgList>(json);
        Assert.IsNotNull(messages);
    }

    [TestMethod]
    public void TestPlayerQMessage() {
        DDiscordMessage? sample = messages.messages[0];
        Assert.IsNotNull(sample);
        Assert.IsTrue(sample.type == EMessageType.PLAYER_Q);

        Assert.IsFalse(sample.IsBotResponse());
        Assert.IsFalse(sample.IsBotNotification());
        Assert.IsFalse(sample.HasSubstitutes());
        Assert.AreEqual(sample.GetMatchId(), -1);
        Assert.IsNull(sample.GetEmbeddedTitle());
        Assert.IsNull(sample.GetEmbeddedDescription());
        Assert.IsNull(sample.GetEmbeddedField(0));
        Assert.IsNull(sample.GetTeamOne());
        Assert.IsNull(sample.GetTeamTwo());

        Assert.IsNotNull(sample.id);
        Assert.AreEqual(sample.content, "!q");
        Assert.IsNotNull(sample.author);
        Assert.AreEqual(sample.author.nickname, "TopHattMatt");
        Assert.AreEqual(sample.author.name, "TopHattMatt");
        Assert.AreEqual(sample.author.id, "343117878360539146");
        Assert.AreEqual(sample.author.isBot, false);
        Assert.IsNotNull(sample.timestamp);
        Assert.AreEqual(sample.mentions.Count, 0);
        Assert.AreEqual(sample.reactions.Count, 0);
        Assert.AreEqual(sample.embeds.Count, 0);
    }

    [TestMethod]
    public void TestBotResponsePlayerQMessage() {
        DDiscordMessage? sample = messages.messages[1];
        Assert.IsNotNull(sample);
        Assert.IsTrue(sample.type == EMessageType.BOT_RESPONSE_TO_PLAYER_Q);

        Assert.IsTrue(sample.IsBotResponse());
        Assert.IsFalse(sample.IsBotNotification());
        Assert.IsFalse(sample.HasSubstitutes());
        Assert.AreEqual(sample.GetMatchId(), -1);
        Assert.IsNotNull(sample.GetEmbeddedTitle());
        Assert.AreEqual(sample.GetEmbeddedTitle(), "5 players are in the queue");
        Assert.IsNotNull(sample.GetEmbeddedDescription());
        Assert.AreEqual(sample.GetEmbeddedDescription(), "[TopHattMatt](https://www.rl6mans.com/profile/TopHattMatt) has joined.");
        Assert.IsNull(sample.GetEmbeddedField(0));
        Assert.IsNull(sample.GetTeamOne());
        Assert.IsNull(sample.GetTeamTwo());

        Assert.IsNotNull(sample.id);
        Assert.AreEqual(sample.content, "");
        Assert.IsNotNull(sample.author);
        Assert.AreEqual(sample.author.nickname, "Bot 6MansBot");
        Assert.AreEqual(sample.author.name, "6MansBot");
        Assert.AreEqual(sample.author.id, "351735054969470976");
        Assert.AreEqual(sample.author.isBot, true);
        Assert.IsNotNull(sample.timestamp);
        Assert.IsTrue(sample.mentions == null || sample.mentions.Count == 0);
        Assert.IsTrue(sample.reactions == null || sample.reactions.Count == 0);
        
    }

    [TestMethod]
    public void TestPlayerLeaveMessage() {
        DDiscordMessage? sample = GetLeaveMessage();
        Assert.IsNotNull(sample);
        Assert.IsTrue(sample.type == EMessageType.PLAYER_LEAVE);

        Assert.IsFalse(sample.IsBotResponse());
        Assert.IsFalse(sample.IsBotNotification());
        Assert.IsFalse(sample.HasSubstitutes());
        Assert.AreEqual(sample.GetMatchId(), -1);
        Assert.IsNull(sample.GetEmbeddedTitle());
        Assert.IsNull(sample.GetEmbeddedDescription());
        Assert.IsNull(sample.GetEmbeddedField(0));
        Assert.IsNull(sample.GetTeamOne());
        Assert.IsNull(sample.GetTeamTwo());

        Assert.IsNotNull(sample.id);
        Assert.AreEqual(sample.content, "!leave");
        Assert.IsNotNull(sample.author);
        Assert.AreEqual(sample.author.nickname, "Fattyy-_-");
        Assert.AreEqual(sample.author.name, "Fatty");
        Assert.AreEqual(sample.author.id, "183751831405461504");
        Assert.AreEqual(sample.author.isBot, false);
        Assert.IsNotNull(sample.timestamp);
        Assert.IsTrue(sample.mentions == null || sample.mentions.Count == 0);
        Assert.IsTrue(sample.reactions == null || sample.reactions.Count == 0);
        Assert.IsTrue(sample.embeds == null || sample.embeds.Count == 0);
    }

    [TestMethod]
    public void TestBotResponsePlayerLeaveMessage() {
        DDiscordMessage? sample = GetBotResponseLeaveMessage();
        Assert.IsNotNull(sample);
        Assert.IsTrue(sample.type == EMessageType.BOT_RESPONSE_TO_PLAYER_LEAVE);

        Assert.IsTrue(sample.IsBotResponse());
        Assert.IsFalse(sample.IsBotNotification());
        Assert.IsFalse(sample.HasSubstitutes());
        Assert.AreEqual(sample.GetMatchId(), -1);
        Assert.IsNotNull(sample.GetEmbeddedTitle());
        Assert.AreEqual(sample.GetEmbeddedTitle(), "1 players are in the queue");
        Assert.IsNotNull(sample.GetEmbeddedDescription());
        Assert.AreEqual(sample.GetEmbeddedDescription(), "[Fattyy-_-](https://www.rl6mans.com/profile/Fattyy-_-) has left (using command).");
        Assert.IsNull(sample.GetEmbeddedField(0));
        Assert.IsNull(sample.GetTeamOne());
        Assert.IsNull(sample.GetTeamTwo());

        Assert.IsNotNull(sample.id);
        Assert.AreEqual(sample.content, "");
        Assert.IsNotNull(sample.author);
        Assert.AreEqual(sample.author.nickname, "Bot 6MansBot");
        Assert.AreEqual(sample.author.name, "6MansBot");
        Assert.AreEqual(sample.author.id, "351735054969470976");
        Assert.AreEqual(sample.author.isBot, true);
        Assert.IsNotNull(sample.timestamp);
        Assert.IsTrue(sample.mentions == null || sample.mentions.Count == 0);
        Assert.IsTrue(sample.reactions == null || sample.reactions.Count == 0);
    }
    
    [TestMethod]
    // TODO: test after score reports are mined
    public void TestScoreReportMessage() {
        
    }

    [TestMethod]
    public void TestTeamsPickedMessage() {
        DDiscordMessage? sample = GetTeamsPickedMessage();
        Assert.IsNotNull(sample);
        Assert.IsTrue(sample.type == EMessageType.TEAMS_PICKED);

        Assert.IsFalse(sample.IsBotResponse());
        Assert.IsTrue(sample.IsBotNotification());
        Assert.IsFalse(sample.HasSubstitutes());
        Assert.AreEqual(sample.GetMatchId(), 886);

        Assert.IsNotNull(sample.GetEmbeddedTitle());
        Assert.AreEqual(sample.GetEmbeddedTitle(), "Lobby #886 is ready!");
        Assert.IsNotNull(sample.GetEmbeddedDescription());
        Assert.AreEqual(sample.GetEmbeddedDescription(), "You may now join the team channels");
        Assert.IsNotNull(sample.GetEmbeddedField(0));
        Assert.AreEqual(sample.GetEmbeddedField(0).name, "-Team 1-");
        Assert.AreEqual(sample.GetEmbeddedField(0).value, "[Whale-](https://www.rl6mans.com/profile/Whale-), [ohtits](https://www.rl6mans.com/profile/ohtits), [kimo](https://www.rl6mans.com/profile/kimo)");
        Assert.IsNotNull(sample.GetEmbeddedField(1));
        Assert.AreEqual(sample.GetEmbeddedField(1).name, "-Team 2-");
        Assert.AreEqual(sample.GetEmbeddedField(1).value, "[cg](https://www.rl6mans.com/profile/cg), [Bella](https://www.rl6mans.com/profile/Bella), [Evil](https://www.rl6mans.com/profile/Evil)");
        Assert.IsNotNull(sample.GetEmbeddedField(2));
        Assert.AreEqual(sample.GetEmbeddedField(2).name, "Creates the lobby:");
        Assert.AreEqual(sample.GetEmbeddedField(2).value, "@Evil");

        // NOTE: Haven't parsed names at this point since it's out of the scope of this test method
        //Assert.IsNull(sample.GetTeamOne());
        //Assert.IsNull(sample.GetTeamTwo());

        Assert.IsTrue(sample.mentions != null && sample.mentions.Count == 6);

        Assert.AreEqual(sample.mentions[0].nickname, "Bella the Elite");
        Assert.AreEqual(sample.mentions[0].name, "Bella.");
        Assert.AreEqual(sample.mentions[0].id, "213080978111987712");
        Assert.AreEqual(sample.mentions[0].isBot, false);

        Assert.AreEqual(sample.mentions[1].nickname, "ControllerEvil");
        Assert.AreEqual(sample.mentions[1].name, "Evil");
        Assert.AreEqual(sample.mentions[1].id, "236485142728671232");
        Assert.AreEqual(sample.mentions[1].isBot, false);

        Assert.AreEqual(sample.mentions[2].nickname, "Whale-");
        Assert.AreEqual(sample.mentions[2].name, "Whale");
        Assert.AreEqual(sample.mentions[2].id, "430460963293233152");
        Assert.AreEqual(sample.mentions[2].isBot, false);

        // Assertions don't like unicode emojis, but it's fine outside of testing
        Assert.AreEqual(sample.mentions[3].nickname, "cg");
        Assert.AreEqual(sample.mentions[3].name, ".cgXD\u2730");
        Assert.AreEqual(sample.mentions[3].id, "290318442882400256");
        Assert.AreEqual(sample.mentions[3].isBot, false);

        Assert.AreEqual(sample.mentions[4].nickname, "kimo");
        Assert.AreEqual(sample.mentions[4].name, "kimo");
        Assert.AreEqual(sample.mentions[4].id, "403256669792108545");
        Assert.AreEqual(sample.mentions[4].isBot, false);

        Assert.AreEqual(sample.mentions[5].nickname, "otis");
        Assert.AreEqual(sample.mentions[5].name, "otis");
        Assert.AreEqual(sample.mentions[5].id, "582287073524842496");
        Assert.AreEqual(sample.mentions[5].isBot, false);

        Assert.IsNotNull(sample.id);
        Assert.AreEqual(sample.content, "@otis, @.cgXD\u2730, @Bella., @Evil, @Whale, @kimo");
        Assert.IsNotNull(sample.author);
        Assert.AreEqual(sample.author.nickname, "Bot 6MansBot");
        Assert.AreEqual(sample.author.name, "6MansBot");
        Assert.AreEqual(sample.author.id, "351735054969470976");
        Assert.AreEqual(sample.author.isBot, true);
        Assert.IsNotNull(sample.timestamp);
        Assert.IsTrue(sample.reactions == null || sample.reactions.Count == 0);
    }

    [TestMethod]
    public void TestVotingCompleteMessage() {
        DDiscordMessage? sample = messages.messages[11];
        Assert.IsNotNull(sample);
        Assert.IsTrue(sample.type == EMessageType.VOTING_COMPLETE);

        Assert.IsFalse(sample.IsBotResponse());
        Assert.IsTrue(sample.IsBotNotification());
        Assert.IsFalse(sample.HasSubstitutes());
        Assert.AreEqual(sample.GetMatchId(), 884);
        Assert.IsNotNull(sample.GetEmbeddedTitle());
        Assert.AreEqual(sample.GetEmbeddedTitle(), "Please join __**Lobby #884**__ in the __**Rank B category**__");
        Assert.IsNotNull(sample.GetEmbeddedDescription());
        Assert.AreEqual(sample.GetEmbeddedDescription(), "All players must join within 7 minutes and then teams will be chosen.\n**Vote result:** Captains");
        Assert.IsNull(sample.GetEmbeddedField(0));
        Assert.IsNull(sample.GetTeamOne());
        Assert.IsNull(sample.GetTeamTwo());

        Assert.IsNotNull(sample.id);
        Assert.AreEqual(sample.content, "@Charm, @frigglefrackler, @bazzcannon, @volterohh, @TopHattMatt, @Gengar");
        Assert.IsNotNull(sample.author);
        Assert.AreEqual(sample.author.nickname, "Bot 6MansBot");
        Assert.AreEqual(sample.author.name, "6MansBot");
        Assert.AreEqual(sample.author.id, "351735054969470976");
        Assert.AreEqual(sample.author.isBot, true);
        Assert.IsNotNull(sample.timestamp);
        Assert.IsNotNull(sample.mentions);
        Assert.IsTrue(sample.mentions != null && sample.mentions.Count == 6);
        Assert.IsTrue(sample.reactions == null || sample.reactions.Count == 0);
    }

    [TestMethod]
    public void TestGetPlayerNameFromEmbeddedLink() {
        string sample0 = "[SirFinkle](https://www.rl6mans.com/profile/SirFinkle) has joined.";
        string sample1 = "[[wildcherry]](https://www.rl6mans.com/profile/[[wildcherry]]) has joined.";
        string sample2 = "[wildcherry]";
        string sample3 = "";
        string sample4 = "[SirFinkle(https://www.rl6mans.com/profile/SirFinkle) has joined.";
        string sample5 = "SirFinkle(https://www.rl6mans.com/profile/SirFinkle) has joined.";
        string sample6 = "SirFinkle](https://www.rl6mans.com/profile/SirFinkle) has joined.";

        var dummy = messages.messages[0];
        Assert.IsNotNull(dummy.GetPlayerNameFromEmbeddedLink(sample0));
        Assert.IsNotNull(dummy.GetPlayerNameFromEmbeddedLink(sample1));
        Assert.IsNull(dummy.GetPlayerNameFromEmbeddedLink(sample2));
        Assert.IsNull(dummy.GetPlayerNameFromEmbeddedLink(sample3));
        Assert.IsNull(dummy.GetPlayerNameFromEmbeddedLink(sample4));
        Assert.IsNull(dummy.GetPlayerNameFromEmbeddedLink(sample5));
        Assert.IsNull(dummy.GetPlayerNameFromEmbeddedLink(sample6));

        Assert.AreEqual("SirFinkle", dummy.GetPlayerNameFromEmbeddedLink(sample0));
        Assert.AreEqual("[wildcherry]", dummy.GetPlayerNameFromEmbeddedLink(sample1));
    }

    [TestMethod]
    public void TestGetPlayerNamesFromTeamPickedMessage() {
        var msg = GetTeamsPickedMessage();
        Assert.IsNotNull(msg);

        var sample0 = msg.GetPlayerNamesFromTeamPickedMessage();
        Assert.IsNotNull(sample0);
        Assert.IsTrue(sample0.Length == 6);
        Console.WriteLine(sample0);
        Assert.AreEqual("Whale-", sample0[0]);
        Assert.AreEqual("ohtits", sample0[1]);
        Assert.AreEqual("kimo", sample0[2]);
    }
}
