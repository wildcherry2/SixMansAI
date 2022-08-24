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
    public Database.Team winning_team { get; set; } = Database.Team.NOT_SET;
    public Queue queue { get; set; } = new Queue();
    public Player.Player subbed_in { get; set; } = new();
    public Player.Player subbed_out { get; set; } = new();

    public static implicit operator bool(ScoreReportMessage msg) {
        return msg.match_id > -1 && msg.reporter != 0;
    }

    // Successful construction heavily relies on formatting going well, will crash intentionally if there's an unaccounted edge case
    public ScoreReportMessage(DiscordMessage.DiscordMessage msg, List<Player.Player> players)
    {
        var content = msg.content.Split(' ');

        match_id = int.Parse(content[1]);
        reporter = ulong.Parse(msg.author.id);

        if (content[2].StartsWith('w') || content[2].StartsWith('W')) reported_win = true;

        if (msg.HasSubs())
        {
            int index = Database.GetIndexOfPlayer(ref players, msg.mentions[0].GetDiscordId());
            if(index != -1) subbed_in = players[index];
            //else log error

            index = Database.GetIndexOfPlayer(ref players, msg.mentions[1].GetDiscordId());
            if(index != -1) subbed_out = players[index];
            //else log error
        }
    }

    public bool SetWinner(Queue queue) {
        if (queue.TeamOneHasPlayer(reporter)) {
            if (reported_win) {
                winning_team = Database.Team.TEAM_ONE;
            }
            this.queue = queue;
            winner_set = true;
            return true;
        }
        else if(queue.TeamTwoHasPlayer(reporter)) {
            if(!reported_win) winning_team = Database.Team.TEAM_ONE;
            this.queue = queue;
            winner_set = true;
            return true;
        }

        return false;
    }

    public bool GetPlayerWon(Player.Player player) {
        if(queue.TeamOneHasPlayer(player.discord_id) && winning_team == Database.Team.TEAM_ONE) {

        }
    }
}