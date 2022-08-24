using Database.Messages.DiscordMessage;
using Database.Player;
using Newtonsoft.Json;

namespace Database.Messages.ScoreReportMessage;

public class ScoreReportMessage : DiscordMessage.DiscordMessage
{
    public int match_id { get; set; } = -1;
    public ulong reporter { get; set; } = 0;
    public bool reported_win { get; set; } = false;
    public bool winner_set { get; set; } = false;
    public Author subbed_in { get; set; } = new();
    public Author subbed_out { get; set; } = new();

    public static implicit operator bool(ScoreReportMessage msg) {
        return msg.match_id > -1 && msg.reporter != 0;
    }

    // Successful construction heavily relies on formatting going well, will crash intentionally if there's an unaccounted edge case
    public ScoreReportMessage(DiscordMessage.DiscordMessage msg)
    {
        var content = msg.content.Split(' ');

        match_id = int.Parse(content[1]);
        reporter = ulong.Parse(msg.author.id);

        if (content[2].StartsWith('w') || content[2].StartsWith('W')) reported_win = true;

        if (msg.HasSubs())
        {
            subbed_in = msg.mentions[0];

            if (msg.mentions.Count > 1) subbed_out = msg.mentions[1];
        }
    }

    public bool SetWinner(Queue queue) {
        if (queue.TeamOneHasPlayer(reporter)) {
            if (reported_win) 
                queue.team_one_won = true;
            winner_set = true;
            return true;
        }
        else if(queue.TeamTwoHasPlayer(reporter) && !reported_win) {
            if(!reported_win) queue.team_one_won = true;
            winner_set = true;
            return true;
        }

        return false;
    }

    public bool GetPlayerWon(Player.Player player) {
        if()
    }
}