
using Database.Database.DatabaseCore.Season.Queue;
using Database.Enums;
using Database.Structs;

namespace Database.Database.DatabaseCore; 

public class QueueReportBinder : ILogger {
    private static QueueReportBinder? singleton { get; set; }
    private QueueReportBinder() : base(ConsoleColor.Yellow, 1, "ScoreReportFactory") {}
    public static QueueReportBinder GetSingleton() {
        if (singleton == null) singleton = new QueueReportBinder();
        return singleton;
    }

    public bool bIsComplete { get; set; } = false;

    // Precondition: QueueFactory's and ScoreReportFactory's ProcessChat must be successful
    // Postcondition: All queues in DDatabaseCore's singleton's queues field has a valid score report field, if a score report exists 
    public void BindReportsToQueues() {
        var core = DDatabaseCore.GetSingleton();
        if (!QueueFactory.GetSingleton().bIsComplete || !ScoreReportFactory.GetSingleton().bIsComplete || core.all_queues == null || core.all_score_reports == null) {
            Log("Preconditions not met! Preconditions status:\nQueueFactory.bIsComplete = {0}\nScoreReportFactory.bIsComplete = {1}" +
                "\nDDatabaseCore.all_queues = {2},\nDDatabaseCore.all_score_reports = {3}", QueueFactory.GetSingleton().bIsComplete.ToString(),
                ScoreReportFactory.GetSingleton().bIsComplete.ToString(), DDatabaseCore.GetSingleton().all_queues != null ? "Not null" : "Null",
                DDatabaseCore.GetSingleton().all_score_reports != null ? "Not null" : "Null");
            return;
        }

        foreach (var queue in core.all_queues) {
            bool found = false;
            foreach (var report in core.all_score_reports) {
                if (queue.match_id == report.iMatchId) {
                    queue.score_report = report;
                    SetWinnerInQueue(queue);
                    found = true;
                }
            }

            if (!found) {
                queue.score_report = new FScoreReport();
                queue.score_report.bError = true;
            }
        }

        bIsComplete = true;
    }

    private void SetWinnerInQueue(DQueue queue) {
        var report = queue.score_report;
        if (ReferenceEquals(report.reporter, null) || ReferenceEquals(report, null)) {
            Log("Can't set winner! Report = {0}, Reporter = {1}", ReferenceEquals(report, null).ToString(), ReferenceEquals(report.reporter, null).ToString());
            return;
        }

        if (!ReferenceEquals(queue.team_one.player_one, null) && queue.team_one.player_one.discord_id == report.reporter.discord_id) {
            queue.winner = ETeamLabel.TEAM_ONE;
        }

        else if (!ReferenceEquals(queue.team_one.player_two, null) && queue.team_one.player_two.discord_id == report.reporter.discord_id) {
            queue.winner = ETeamLabel.TEAM_ONE;
        }

        else if (!ReferenceEquals(queue.team_one.player_three, null) && queue.team_one.player_three.discord_id == report.reporter.discord_id) {
            queue.winner = ETeamLabel.TEAM_ONE;
        }

        else if (!ReferenceEquals(queue.team_two.player_one, null) && queue.team_two.player_one.discord_id == report.reporter.discord_id) {
            queue.winner = ETeamLabel.TEAM_TWO;
        }

        else if (!ReferenceEquals(queue.team_two.player_two, null) && queue.team_two.player_two.discord_id == report.reporter.discord_id) {
            queue.winner = ETeamLabel.TEAM_TWO;
        }

        else if (!ReferenceEquals(queue.team_two.player_three, null) && queue.team_two.player_three.discord_id == report.reporter.discord_id) {
            queue.winner = ETeamLabel.TEAM_TWO;
        }

        if (queue.winner == ETeamLabel.NOT_SET) queue.bError = true;
    }
}