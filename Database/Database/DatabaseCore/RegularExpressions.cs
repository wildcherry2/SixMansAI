using System.Text.RegularExpressions;

namespace Database.Database.DatabaseCore {
    public static class RegularExpressions {
        public static Regex lobby_id_regex                               = new(@"[0-9]+", RegexOptions.Compiled);
        public static Regex score_report_strict_regex                    = new("!report [0-9]+ [w,l,W,L]", RegexOptions.Compiled);
        public static Regex score_report_relaxed_regex                   = new(@"!report +[0-9]+[\n,\t, ]* [\n,\t, ]*[w,W,l,L]", RegexOptions.Compiled);
        public static Regex select_multiple_spaces_in_score_report_regex = new("[ ]{2,}", RegexOptions.Compiled);
        public static Regex name_from_embedded_link_regex                = new(@"(?:(?!^\[|\]\().)+", RegexOptions.Compiled);
        public static Regex is_link_regex                                = new(@"^\[.+\]\(https://www\.rl6mans\.com/profile/.+\) has joined.$", RegexOptions.Compiled);
        public static Regex lobby_id_lobby_cancelled_regex               = new(@"(?:(?!(^.+\*\*))+(?!(\*\*Match ID ))+(?!(\*\*\..+$))[0-9]+)", RegexOptions.Compiled);

        // Match 1 = month, Match 2 = year
        public static Regex season_from_file_name_regex = new(@"(?:\D+|\d+)", RegexOptions.Compiled);
    }
}