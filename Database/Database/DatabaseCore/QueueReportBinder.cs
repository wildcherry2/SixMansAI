
using Database.Database.DatabaseCore.Season.Queue;
using Database.Enums;
using Database.Structs;

namespace Database.Database.DatabaseCore; 

public class QueueReportBinder {
    private static QueueReportBinder? singleton { get; set; }
    private QueueReportBinder(){}
    public static QueueReportBinder GetSingleton() {
        if (singleton == null) singleton = new QueueReportBinder();
        return singleton;
    }

    // Precondition: QueueFactory's and ScoreReportFactory's ProcessChat must be successful
    // Postcondition: All queues in DDatabaseCore's singleton's queues field has a valid score report field, if a score report exists 
    public void BindReportsToQueues() {
        var core = DDatabaseCore.GetSingleton();
        if (!QueueFactory.GetSingleton().bIsComplete || !ScoreReportFactory.GetSingleton().bIsComplete || core.all_queues == null || core.all_score_reports == null) return;

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
    }

    private void SetWinnerInQueue(DQueue queue) {
        var report = queue.score_report;
        if (ReferenceEquals(report.reporter, null)) {
            // log error

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