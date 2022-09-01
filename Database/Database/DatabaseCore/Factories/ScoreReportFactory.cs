using Database.Database.DatabaseCore.Cleaners;
using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Interfaces;
using Database.Structs;

namespace Database.Database.DatabaseCore.Factories {
    public class ScoreReportFactory : FactoryBase {
        private ScoreReportFactory() {}
        private static ScoreReportFactory? singleton   { get; set; }
        public         bool                bIsComplete { get; set; }

        public static ScoreReportFactory GetSingleton() {
            if (singleton == null) { singleton = new ScoreReportFactory(); }

            return singleton;
        }

        // Precondition: ScoreReportCleaner.ProcessChat has already been called with messages
        // Postcondition: DDatabaseCore's singleton's all_score_reports field is initialized with data
        public void ProcessChat(in FMessageList messages) {
            logger.Log("Deserializing {0} messages into FScoreReports...", messages.messages.Count.ToString());
            if (!ScoreReportCleaner.GetSingleton().bIsComplete) { return; }

            var ret = new List<FScoreReport>();
            var err_count = 0;
            for (var i = 0; i < messages.messages.Count; i++) {
                var message = messages.messages[i];
                var report = new FScoreReport();
                report.iMatchId = message.GetMatchId();
                report.bReportedWin = GetReportedWin(ref message);
                report.reporter = GetReporter(ref message);
                report.report_msg = message;
                var subs = message.HasSubstitutes();
                if (subs) { SetSubstitutes(ref report, ref message); }


                if (report.iMatchId == -1 || ReferenceEquals(report.reporter, null) || (subs && (ReferenceEquals(report.subbed_in, null) || ReferenceEquals(report.subbed_out, null)))) {
                    report.bError = true;
                    logger.Log("Error building an FScoreReport! Missing data, fields = \nMatch ID = {0}, \nReporter = {1}, \nHasSubs = {2}, \nSubbed in = {3}, \nSubbed out = {4}",
                        report.iMatchId.ToString(), !ReferenceEquals(report.reporter, null) ? report.reporter.recorded_names[0] : "Null Reporter!",
                        subs.ToString(), subs ? !ReferenceEquals(report.subbed_in, null) ? report.subbed_in.recorded_names[0] : "Null subbed in player!" : "",
                        subs ? !ReferenceEquals(report.subbed_out, null) ? report.subbed_out.recorded_names[0] : "Null subbed out player!" : "");
                    err_count++;
                }

                report.TryGetOrCreatePrimaryKey();
                ret.Add(report);
            }

            if (ret.Count == 0) { logger.Log("Error creating score report list! No reports were added to the list!"); }
            else {
                logger.Log("{0} score reports generated with {1} errors! Valid score reports = {2}", ret.Count.ToString(), err_count.ToString(), (ret.Count - err_count).ToString());
                bIsComplete = true;
                DDatabaseCore.GetSingleton().all_score_reports = ret;
            }
        }

        private bool GetReportedWin(ref DDiscordMessage message) {
            var split = message.content.Split(' ');
            if (split.Length == 3) {
                if (split[2].StartsWith('w') || split[2].StartsWith('W')) { return true; }
            }

            return false;
        }

        private DPlayer? GetReporter(ref DDiscordMessage message) {
            var id = message.author.id;
            if (id == null) { return null; }

            return DDatabaseCore.GetSingleton().GetPlayerIfExists(ulong.Parse(id));
        }

        private void SetSubstitutes(ref FScoreReport report, ref DDiscordMessage message) {
            if (message.mentions == null || message.mentions.Count != 2 || message.mentions[0].id == null || message.mentions[1].id == null) { return; }

            report.subbed_in = DDatabaseCore.GetSingleton().GetPlayerIfExists(message.mentions[0].id);
            report.subbed_out = DDatabaseCore.GetSingleton().GetPlayerIfExists(message.mentions[1].id);
            report.bHasSubs = true;
        }
    }
}