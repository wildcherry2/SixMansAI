
using Database.Database.DatabaseCore.Season;
using Database.Database.DatabaseCore.Season.Cleaners;
using Database.Structs;

namespace Database.Database.DatabaseCore; 

public class ScoreReportFactory {
    private static ScoreReportFactory? singleton   { get; set; }
    public         bool                bIsComplete { get; set; } = false;
    private ScoreReportFactory() {}

    public static ScoreReportFactory GetSingleton() {
        if (singleton == null) singleton = new ScoreReportFactory();
        return singleton;
    }

    // Precondition: ScoreReportCleaner.ProcessChat has already been called with messages
    // Postcondition: DDatabaseCore's singleton's all_score_reports field is initialized with data
    public void ProcessChat(FMessageList messages) {
        if (!ScoreReportCleaner.GetSingleton().bIsComplete) return;
        List<FScoreReport> ret = new List<FScoreReport>();

        for(var i = 0; i < messages.messages.Count; i++) {
            var message = messages.messages[i];
            FScoreReport report = new FScoreReport();
            report.iMatchId = message.GetMatchId();
            report.bReportedWin = GetReportedWin(ref message);
            report.reporter = GetReporter(ref message);
            bool subs = message.HasSubstitutes();
            if (subs) {
                SetSubstitutes(ref report, ref message);
            }


            if (report.iMatchId == -1 || ReferenceEquals(report.reporter, null) || (subs && (ReferenceEquals(report.subbed_in, null) || ReferenceEquals(report.subbed_out, null)))) 
                report.bError = true;

            ret.Add(report);
        }

        if (ret.Count > 0) bIsComplete = true;
        DDatabaseCore.GetSingleton().all_score_reports = ret;
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