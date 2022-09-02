using Database.Database.DatabaseCore.Factories;
using Database.Database.DatabaseCore.MainComponents;
using Database.Enums;

namespace Database.Database.DatabaseCore.Binders;

public class QueueReportBinder : BinderBase {
    private QueueReportBinder() { problems = new List<DQueue>(); }
    private static QueueReportBinder? singleton   { get; set; }
    private        List<DQueue>       problems    { get; }
    public         bool               bIsComplete { get; private set; }
    public static QueueReportBinder GetSingleton() {
        if (singleton == null) singleton = new QueueReportBinder();

        return singleton;
    }

    // Precondition: QueueFactory's and ScoreReportFactory's ProcessChat must be successful
    // Postcondition: All queues in DDatabaseCore's singleton's queues field has a valid score report field, if a score report exists 
    public void BindReportsToQueues() {
        var core = DataManager.GetSingleton();

        #region Precondition Checks

        if (!QueueFactory.GetSingleton().bIsComplete || !ScoreReportFactory.GetSingleton().bIsComplete || core.all_queues == null || core.all_score_reports == null) {
            logger.Log("Preconditions not met! Preconditions status:\nQueueFactory.bIsComplete = {0}\nScoreReportFactory.bIsComplete = {1}" +
                       "\nDDatabaseCore.all_queues = {2},\nDDatabaseCore.all_score_reports = {3}", QueueFactory.GetSingleton().bIsComplete.ToString(),
                       ScoreReportFactory.GetSingleton().bIsComplete.ToString(), core.all_queues != null ? "Not null" : "Null",
                       core.all_score_reports != null ? "Not null" : "Null");

            return;
        }

        #endregion

        var err_count = 0;
        foreach (var queue in core.all_queues) {
            var found = false;
            foreach (var report in core.all_score_reports) {
                if (queue.match_id == report.iMatchId) {
                    queue.score_report = report;
                    SetWinnerInQueue(queue);
                    found = true;
                }
            }

            if (!found) {
                logger.Log("Could not match score report to lobby {0}!", queue.match_id.ToString());
                queue.score_report        = new ScoreReport();
                queue.score_report.bError = true;
                err_count++;
                problems.Add(queue);
            }
        }

        logger.Log("{0} rank b queues bound to reports, {1} rank b queues not accounted for!", (core.all_queues.Count - err_count).ToString(), err_count.ToString());
        logger.Log("Printing problem queues...");
        foreach (var queue in problems) logger.Log(queue.ToString());
        bIsComplete = true;
    }
    private void SetWinnerInQueue(in DQueue queue) {
        var report = queue.score_report;
        if (ReferenceEquals(report.reporter, null) || ReferenceEquals(report, null)) {
            logger.Log("Can't set winner!\n\t\tReport = {0}\n\t\tReporter = {1}\n\t\tScore Report message content = {2}\n\t\tScore report message author = {3}",
                       (!ReferenceEquals(report, null)).ToString(),
                       (!ReferenceEquals(report.reporter, null)).ToString(),
                       report.report_msg.content,
                       report.report_msg.author.name);

            return;
        }

        var team_one_not_null = !ReferenceEquals(queue.team_one, null);
        var team_two_not_null = !ReferenceEquals(queue.team_two, null);
        if (team_one_not_null && !ReferenceEquals(queue.team_one.player_one, null) && queue.team_one.player_one.discord_id == report.reporter.discord_id)
            queue.winner = ETeamLabel.TEAM_ONE;
        else if (team_one_not_null && !ReferenceEquals(queue.team_one.player_two, null) && queue.team_one.player_two.discord_id == report.reporter.discord_id)
            queue.winner = ETeamLabel.TEAM_ONE;
        else if (team_one_not_null && !ReferenceEquals(queue.team_one.player_three, null) && queue.team_one.player_three.discord_id == report.reporter.discord_id)
            queue.winner = ETeamLabel.TEAM_ONE;
        else if (team_two_not_null && !ReferenceEquals(queue.team_two.player_one, null) && queue.team_two.player_one.discord_id == report.reporter.discord_id)
            queue.winner = ETeamLabel.TEAM_TWO;
        else if (team_two_not_null && !ReferenceEquals(queue.team_two.player_two, null) && queue.team_two.player_two.discord_id == report.reporter.discord_id)
            queue.winner                                                                                                                                                        = ETeamLabel.TEAM_TWO;
        else if (team_two_not_null && !ReferenceEquals(queue.team_two.player_three, null) && queue.team_two.player_three.discord_id == report.reporter.discord_id) queue.winner = ETeamLabel.TEAM_TWO;

        if (queue.winner == ETeamLabel.NOT_SET) queue.bError = true;
    }
}