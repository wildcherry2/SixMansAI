using System.Text.RegularExpressions;
using Database.Messages.DiscordMessage;
using Database.Messages.ScoreReportMessage;

namespace Database;

public partial class Database {
     /*  TODO: THIS WILL NOT WORK YET DUE TO REFACTORS  */
    private void CleanupScoreReportFile() {
        try {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[CleanupScoreReportFile] Opening \"" + sr_path_s + "\"...");
            in_reader = File.OpenText(sr_path_s);
            Console.WriteLine("[CleanupScoreReportFile] Reading \"" + sr_path_s + "\" into JSON object...");
            var score_report = DiscordMessage.RawDataToDiscordMessage(ref in_reader);
            Console.WriteLine("[CleanupScoreReportFile] Done reading \"" + sr_path_s + "\" into JSON object...");
            if (score_report != null) {
                var messages = score_report.raw_messages;
                int pre_count = score_report.raw_messages.Count;
                Console.WriteLine("[CleanupScoreReportFile] Parsed " + pre_count + " messages before error filtering");
                Console.WriteLine("[CleanupScoreReportFile] Cleaning and formatting \"" + sr_path_s + "\"...");
                Console.ForegroundColor = ConsoleColor.Yellow;
                FormatScoreReportList(ref messages);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[CleanupScoreReportFile] Removed " + (pre_count - score_report.raw_messages.Count) +
                                  " erroneous score reports, " + score_report.raw_messages.Count + " reports are valid");
                Console.WriteLine("[CleanupScoreReportFile] Serializing {0} reports to ScoreReportMessage objects...",
                                  score_report.raw_messages.Count);
                foreach (var message in score_report.raw_messages) sr_messages.Add(new ScoreReportMessage(message));
                Console.WriteLine("[CleanupScoreReportFile] Done serializing reports...");
            }

            Console.WriteLine("[CleanupScoreReportFile] Closing file \"{0}\"", sr_path_s);
            in_reader.Close();
        }
        catch (Exception ex) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    private void FormatScoreReportList(ref List<DiscordMessage> list) {
        foreach (var message in list) {
            // If the content is empty, remove the message and go next

            #region ContentLengthCheck

            if (message.content.Length == 0) {
                Console.WriteLine("\t[FormatScoreReportList] Removed empty message...");
                list.Remove(message);
                FormatScoreReportList(ref list);
                break;
            }

            #endregion

            // If there isn't a ✅ reaction, the match wasn't recorded in the database, so remove and go next

            #region MatchVerified

            var is_valid = false;
            foreach (var reaction in message.reactions)
                if (reaction.emojis.name.Contains("✅")) {
                    is_valid = true;
                    break;
                }

            if (!is_valid) {
                Console.WriteLine("\t[FormatScoreReportList] Removed unverified score report...");
                list.Remove(message);
                FormatScoreReportList(ref list);
                break;
            }

            #endregion

            // If it passed the previous if statements it's a valid match report, so remove newlines and unnecessary whitespace

            #region CleanupString

            message.content = message.content.Replace("\n", " ");
            var options = RegexOptions.None;
            var regex = new Regex("[ ]{2,}", options);
            message.content = regex.Replace(message.content, " ");

            #endregion
        }
    }
}