
using System.Text.RegularExpressions;

namespace Database.Database.DatabaseCore; 

public static class RegularExpressions {
    public static Regex lobby_id_regex                               = new Regex(@"[0-9]+", RegexOptions.Compiled);
    public static Regex score_report_strict_regex                    = new Regex("!report [0-9]+ [w,l,W,L]", RegexOptions.Compiled);
    public static Regex score_report_relaxed_regex                   = new Regex("^!report +[0-9]+ +[w,W,l,L]", RegexOptions.Compiled);
    public static Regex select_multiple_spaces_in_score_report_regex = new Regex("[ ]{2,}", RegexOptions.Compiled);
    public static Regex name_from_embedded_link_regex                = new Regex(@"(?:(?!^\[|\]\().)+", RegexOptions.Compiled);
    public static Regex is_link_regex                                = new Regex(@"^\[.+\]\(https://www\.rl6mans\.com/profile/.+\) has joined.$", RegexOptions.Compiled);
    public static Regex lobby_id_lobby_cancelled_regex               = new Regex(@"(?:(?!(^.+\*\*))+(?!(\*\*Match ID ))+(?!(\*\*\..+$))[0-9]+)", RegexOptions.Compiled);
}