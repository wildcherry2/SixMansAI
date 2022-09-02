using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Enums;
using Database.Database.Interfaces;
using Database.Database.Structs;

namespace Database.Database.DatabaseCore.Binders {
    public class PlayerRecordBinder : ILogger {
        private PlayerRecordBinder() : base(ConsoleColor.Yellow, 1, "PlayerRecordBinder") { }
        private static PlayerRecordBinder? singleton { get; set; }
        public         bool                bComplete { get; set; }

        public static PlayerRecordBinder GetSingleton() {
            if (singleton == null) { singleton = new PlayerRecordBinder(); }

            return singleton;
        }

        // Preconditions: all_players is not null, all_queues is not null, and score reports have been linked to their appropriate DQueue object
        public void BindRecordsToPlayers() {
            if (!QueueReportBinder.GetSingleton().bIsComplete || DDatabaseCore.GetSingleton().all_players == null || DDatabaseCore.GetSingleton().all_queues == null) { return; }

            int success = 0;
            int err     = 0;
            foreach (var queue in DDatabaseCore.GetSingleton().all_queues) {
                //var num_assigned = 0;
                //foreach (var player in DDatabaseCore.GetSingleton().all_players) {
                //    if (queue.IsPlayerInTeam(ETeamLabel.TEAM_ONE, player.discord_id)) {
                //        SetRecord(queue, ETeamLabel.TEAM_ONE, player);
                //        num_assigned++;
                //    }
                //    else if (queue.IsPlayerInTeam(ETeamLabel.TEAM_TWO, player.discord_id)) {
                //        SetRecord(queue, ETeamLabel.TEAM_TWO, player);
                //        num_assigned++;
                //    }

                //    if (num_assigned == 6) { break; }
                //}
                if (!queue.bError) {
                    SetRecord(queue, ETeamLabel.TEAM_ONE, queue.team_one.player_one!);
                    SetRecord(queue, ETeamLabel.TEAM_ONE, queue.team_one.player_two!);
                    SetRecord(queue, ETeamLabel.TEAM_ONE, queue.team_one.player_three!);
                    SetRecord(queue, ETeamLabel.TEAM_ONE, queue.team_two.player_one!);
                    SetRecord(queue, ETeamLabel.TEAM_ONE, queue.team_two.player_two!);
                    SetRecord(queue, ETeamLabel.TEAM_ONE, queue.team_two.player_three!);
                    success++;
                }
                else
                    err++;

                

                //if (num_assigned != 6) { Log("Not all players linked to queue {0}! Number of assigned players = {1}", queue.match_id.ToString(), num_assigned.ToString()); }
            }
            Log($"{success} records bound to players, {err} errors!");
            bComplete = true;
        }
        // TODO: definitely need to try to set subbed players again before this
        private void SetRecord(in DQueue queue, in ETeamLabel team_label, in DPlayer player) {
            DPlayer? target = null;
            if (queue.score_report != null && queue.score_report.bHasSubs) { target = queue.score_report.subbed_in; }
            else { target                                                           = player; }

            var rec = new FGameRecord();
            rec.queue = queue;
            if (!ReferenceEquals(target, null)) {
                rec.bPlayerWon = queue.winner == team_label;
                rec.bError     = queue.bError || queue.winner == ETeamLabel.NOT_SET;
                rec.team       = team_label;
                target.game_history.Add(rec);
                if (rec.bPlayerWon) { target.iTotalWins++; }
                else { target.iTotalLosses++; }
            }
            else {
                Log("Failed to get match results for queue with Match ID = {0} and Player Name = {1}", queue.match_id.ToString(), player.recorded_names[0]);
                rec.bError = true;
                player.game_history.Add(rec);
            }
        }
    }
}