using System.Text.Json.Serialization;
using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Enums;
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

    public DDiscordMessage? GetQResponseMessage() {
        foreach (var msg in messages.messages) {
            if (msg.id == "992151004608987217") return msg;
        }

        return null;
    }

    public DDiscordMessage? GetPlayerQMessage() {
        foreach (var msg in messages.messages) {
            if (msg.id == "992151003455565844") return msg;
        }

        return null;
    }

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

    public DDiscordMessage? GetVotingCompleteMessage() {
        foreach (var msg in messages.messages) {
            if (msg.id == "992151879733760170") return msg;
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
        DDiscordMessage? sample = GetPlayerQMessage();
        Console.WriteLine(sample.id);
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
        Assert.AreEqual(sample.author.nickname, "vetta");
        Assert.AreEqual(sample.author.name, "vetta");
        Assert.AreEqual(sample.author.id, "779874051534618676");
        Assert.AreEqual(sample.author.isBot, false);
        Assert.IsNotNull(sample.timestamp);
        Assert.AreEqual(sample.mentions.Count, 0);
        Assert.AreEqual(sample.reactions.Count, 0);
        Assert.AreEqual(sample.embeds.Count, 0);
    }

    [TestMethod]
    public void TestBotResponsePlayerQMessage() {
        DDiscordMessage? sample = GetQResponseMessage();
        Assert.IsNotNull(sample);
        Assert.IsTrue(sample.type == EMessageType.BOT_RESPONSE_TO_PLAYER_Q);

        Assert.IsTrue(sample.IsBotResponse());
        Assert.IsFalse(sample.IsBotNotification());
        Assert.IsFalse(sample.HasSubstitutes());
        Assert.AreEqual(sample.GetMatchId(), -1);
        Assert.IsNotNull(sample.GetEmbeddedTitle());
        Assert.AreEqual(sample.GetEmbeddedTitle(), "1 players are in the queue");
        Assert.IsNotNull(sample.GetEmbeddedDescription());
        Assert.AreEqual(sample.GetEmbeddedDescription(), "[Vetta](https://www.rl6mans.com/profile/Vetta) has joined.");
        Assert.IsNotNull(sample.GetEmbeddedField(0));
        Assert.IsNull(sample.GetTeamOne());
        Assert.IsNull(sample.GetTeamTwo());

        Assert.IsNotNull(sample.id);
        Assert.IsTrue(sample.content == "" || sample.content == "@Rank B NA");
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
        Console.WriteLine(sample.id);
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
        Assert.AreEqual(sample.author.nickname, "CraziiPanduh");
        Assert.AreEqual(sample.author.name, "CraziiPanduh");
        Assert.AreEqual(sample.author.id, "275382835043500032");
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
        Console.WriteLine(sample.id);
        Assert.IsNotNull(sample);
        Assert.IsTrue(sample.type == EMessageType.TEAMS_PICKED);

        Assert.IsFalse(sample.IsBotResponse());
        Assert.IsTrue(sample.IsBotNotification());
        Assert.IsFalse(sample.HasSubstitutes());
        Assert.AreEqual(sample.GetMatchId(), 4);

        Assert.IsNotNull(sample.GetEmbeddedTitle());
        Assert.AreEqual(sample.GetEmbeddedTitle(), "Lobby #4 is ready!");
        Assert.IsNotNull(sample.GetEmbeddedDescription());
        Assert.AreEqual(sample.GetEmbeddedDescription(), "You may now join the team channels");
        Assert.IsNotNull(sample.GetEmbeddedField(0));
        Assert.AreEqual(sample.GetEmbeddedField(0).name, "-Team 1-");
        Assert.AreEqual(sample.GetEmbeddedField(0).value, "[taysech](https://www.rl6mans.com/profile/taysech), [Nava](https://www.rl6mans.com/profile/Nava), [Codyy](https://www.rl6mans.com/profile/Codyy)");
        Assert.IsNotNull(sample.GetEmbeddedField(1));
        Assert.AreEqual(sample.GetEmbeddedField(1).name, "-Team 2-");
        Assert.AreEqual(sample.GetEmbeddedField(1).value, "[Dewy413](https://www.rl6mans.com/profile/Dewy413), [Vetta](https://www.rl6mans.com/profile/Vetta), [Confusion](https://www.rl6mans.com/profile/Confusion)");
        Assert.IsNotNull(sample.GetEmbeddedField(2));
        Assert.AreEqual(sample.GetEmbeddedField(2).name, "Creates the lobby:");
        Assert.AreEqual(sample.GetEmbeddedField(2).value, "@taysech");

        // NOTE: Haven't parsed names at this point since it's out of the scope of this test method
        //Assert.IsNull(sample.GetTeamOne());
        //Assert.IsNull(sample.GetTeamTwo());

        Assert.IsTrue(sample.mentions != null && sample.mentions.Count == 6);

        Assert.AreEqual(sample.mentions[0].nickname, "taysech");
        Assert.AreEqual(sample.mentions[0].name, "taysech");
        Assert.AreEqual(sample.mentions[0].id, "139123904206602240");
        Assert.AreEqual(sample.mentions[0].isBot, false);

        Assert.AreEqual(sample.mentions[1].nickname, "Confusion");
        Assert.AreEqual(sample.mentions[1].name, "Mr Bitch");
        Assert.AreEqual(sample.mentions[1].id, "420719137845673990");
        Assert.AreEqual(sample.mentions[1].isBot, false);

        Assert.AreEqual(sample.mentions[2].nickname, "Nava");
        Assert.AreEqual(sample.mentions[2].name, "JNava8");
        Assert.AreEqual(sample.mentions[2].id, "438868635390443526");
        Assert.AreEqual(sample.mentions[2].isBot, false);

        // Assertions don't like unicode emojis, but it's fine outside of testing
        Assert.AreEqual(sample.mentions[3].nickname, "Dewy413 the Adorable");
        Assert.AreEqual(sample.mentions[3].name, "Dewy413");
        Assert.AreEqual(sample.mentions[3].id, "302910158688747531");
        Assert.AreEqual(sample.mentions[3].isBot, false);

        Assert.AreEqual(sample.mentions[4].nickname, "vetta");
        Assert.AreEqual(sample.mentions[4].name, "vetta");
        Assert.AreEqual(sample.mentions[4].id, "779874051534618676");
        Assert.AreEqual(sample.mentions[4].isBot, false);

        Assert.AreEqual(sample.mentions[5].nickname, "Codyy");
        Assert.AreEqual(sample.mentions[5].name, "Codyy");
        Assert.AreEqual(sample.mentions[5].id, "915656408526127124");
        Assert.AreEqual(sample.mentions[5].isBot, false);

        Assert.IsNotNull(sample.id);
        Assert.AreEqual(sample.content, "@vetta, @Mr Bitch, @Codyy, @taysech, @JNava8, @Dewy413");
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
        DDiscordMessage? sample = GetVotingCompleteMessage();
        Assert.IsNotNull(sample);
        Assert.IsTrue(sample.type == EMessageType.VOTING_COMPLETE);

        Assert.IsFalse(sample.IsBotResponse());
        Assert.IsTrue(sample.IsBotNotification());
        Assert.IsFalse(sample.HasSubstitutes());
        Assert.AreEqual(sample.GetMatchId(), 4);
        Assert.IsNotNull(sample.GetEmbeddedTitle());
        Assert.AreEqual(sample.GetEmbeddedTitle(), "Please join __**Lobby #4**__ in the __**Rank B category**__");
        Assert.IsNotNull(sample.GetEmbeddedDescription());
        Assert.AreEqual(sample.GetEmbeddedDescription(), "All players must join within 7 minutes and then teams will be chosen.\n**Vote result:** Captains");
        Assert.IsNull(sample.GetEmbeddedField(0));
        Assert.IsNull(sample.GetTeamOne());
        Assert.IsNull(sample.GetTeamTwo());

        Assert.IsNotNull(sample.id);
        Assert.AreEqual(sample.content, "@vetta, @Mr Bitch, @Codyy, @taysech, @JNava8, @Dewy413");
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

    // Invariant: There will always be a name between the first pair of [ and ]
    [TestMethod]
    public void TestGetPlayerNameFromEmbeddedLink() {
        string sample0 = @"[SirFinkle](https://www.rl6mans.com/profile/SirFinkle) has joined.";
        string sample1 = @"[[wildcherry]](https://www.rl6mans.com/profile/[[wildcherry]]) has joined.";
        string sample2 = @"[wildcherry]](https://www.rl6mans.com/profile/wildcherry]) has joined.";
        string sample3 = @"[wildcherry+/?\\]](https://www.rl6mans.com/profile/wildcherry]) has joined.";

        var dummy = messages.messages[0];
        Assert.IsNotNull(dummy.GetPlayerNameFromEmbeddedLink(sample0));
        Assert.IsNotNull(dummy.GetPlayerNameFromEmbeddedLink(sample1));
        Assert.IsNotNull(dummy.GetPlayerNameFromEmbeddedLink(sample2));
        Assert.IsNotNull(dummy.GetPlayerNameFromEmbeddedLink(sample3));

        Assert.AreEqual("SirFinkle", dummy.GetPlayerNameFromEmbeddedLink(sample0));
        Assert.AreEqual("[wildcherry]", dummy.GetPlayerNameFromEmbeddedLink(sample1));
        Assert.AreEqual("wildcherry]", dummy.GetPlayerNameFromEmbeddedLink(sample2));
        Assert.AreEqual(@"wildcherry+/?\\]", dummy.GetPlayerNameFromEmbeddedLink(sample3));
    }

    [TestMethod]
    public void TestGetPlayerNamesFromTeamPickedMessage() {
        var msg = GetTeamsPickedMessage();
        Assert.IsNotNull(msg);
        var sample0 = msg.GetPlayerNamesFromTeamPickedMessage();
        Assert.IsNotNull(sample0);
        Assert.IsTrue(sample0.Length == 6);
        Console.WriteLine(sample0);
        Assert.AreEqual("taysech", sample0[0]);
        Assert.AreEqual("Nava", sample0[1]);
        Assert.AreEqual("Codyy", sample0[2]);
    }
}
