// Join, leave, !q, and !leave messages between the first queue and the next voting complete message

using Database.Messages.DiscordMessage;

public class QueueBlock {
    public List<DiscordMessage> messages                = new();
    public List<DiscordMessage> teams_decided_messages  = new List<DiscordMessage>(); // may not necessarily belong to this q's team
    public DiscordMessage       voting_complete_message = new DiscordMessage();

    public int counter = 0;
    public QueueBlock() { }
}