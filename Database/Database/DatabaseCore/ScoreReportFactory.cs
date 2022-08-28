
using Database.Database.DatabaseCore.Season;
using Database.Structs;

namespace Database.Database.DatabaseCore; 

public class ScoreReportFactory {
    private static ScoreReportFactory? singleton { get; set; }

    private ScoreReportFactory() {}

    public static ScoreReportFactory GetSingleton() {
        if (singleton == null) singleton = new ScoreReportFactory();
        return singleton;
    }

    // Precondition: ScoreReportCleaner.ProcessChat has already been called with messages
    public List<FScoreReport> ProcessChat(FMessageList messages) {
        List<FScoreReport> ret = new List<FScoreReport>();

        for(var i = 0; i < messages.messages.Count; i++) {
            var message = messages.messages[i];
            FScoreReport report = new FScoreReport();
            report.iMatchId = message.GetMatchId();
            report.bReportedWin = GetReportedWin(ref message);
            report.reporter = GetReporter(ref message);
            if (message.HasSubstitutes()) {
                SetSubstitutes(ref report, ref message);
            }

            ret.Add(report);
        }

        DDatabaseCore.GetSingleton().all_score_reports = ret;
        return ret;
    }

    private bool GetReportedWin(ref DDiscordMessage message) {
        var split = message.content.Split(' ');
        if (split.Length == 3) {
            if (split[2].StartsWith('w') || split[2].StartsWith('W')) return true;
        }

        return false;
    }

    private DPlayer? GetReporter(ref DDiscordMessage message) {
        var name = message.author.name;
        if (name == null) return null;
        return DDatabaseCore.GetSingleton().GetPlayerIfExists(name);
    }

    private void SetSubstitutes(ref FScoreReport report, ref DDiscordMessage message) {
        if (message.mentions == null || message.mentions.Count != 2 || message.mentions[0].id == null || message.mentions[1].id == null) return;
        report.subbed_in = DDatabaseCore.GetSingleton().GetPlayerIfExists(message.mentions[0].id);
        report.subbed_out = DDatabaseCore.GetSingleton().GetPlayerIfExists(message.mentions[1].id);

        // TODO: log if either subbed_in or subbed_out is null
    }
}