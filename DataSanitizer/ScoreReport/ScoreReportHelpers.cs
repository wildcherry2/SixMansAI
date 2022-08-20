using Newtonsoft.Json;
using System;

namespace DataSanitizer.ScoreReport;

public static class ScoreReportHelpers {
    public static DiscordMessage RawDataToDiscordMessage(ref StreamReader sr) {
        return JsonConvert.DeserializeObject<DiscordMessage>(sr.ReadToEnd());
    }
}