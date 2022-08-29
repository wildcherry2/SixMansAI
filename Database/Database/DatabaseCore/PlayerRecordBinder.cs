﻿using Database.Database.DatabaseCore.Season.Queue;
using Database.Enums;
using Database.Structs;

namespace Database.Database.DatabaseCore; 

public class PlayerRecordBinder : ILogger {
    private static PlayerRecordBinder? singleton { get; set; } = null;
    public         bool                bComplete { get; set; } = false; 
    private PlayerRecordBinder() : base(ConsoleColor.Yellow, 1, "PlayerRecordBinder") {}
    
    public static PlayerRecordBinder GetSingleton() {
        if(singleton == null) singleton = new PlayerRecordBinder();
        return singleton;
    }

    // Preconditions: all_players is not null, all_queues is not null, and score reports have been linked to their appropriate DQueue object
    public void BindRecordsToPlayers() {
        if (!QueueReportBinder.GetSingleton().bIsComplete || DDatabaseCore.GetSingleton().all_players == null || DDatabaseCore.GetSingleton().all_queues == null) return;
        foreach (var queue in DDatabaseCore.GetSingleton().all_queues) {
            int num_assigned = 0;
            foreach (var player in DDatabaseCore.GetSingleton().all_players) {
                if (queue.IsPlayerInTeam(ETeamLabel.TEAM_ONE, player.discord_id)) {
                    SetRecord(queue, ETeamLabel.TEAM_ONE, player);
                    num_assigned++;
                }
                else if (queue.IsPlayerInTeam(ETeamLabel.TEAM_TWO, player.discord_id)) {
                    SetRecord(queue, ETeamLabel.TEAM_TWO, player);
                    num_assigned++;
                }
                if (num_assigned == 6) break;
            }

            if (num_assigned != 6) {
                Log("Not all players linked to queue {0}! Number of assigned players = {1}", queue.match_id.ToString(), num_assigned.ToString());
            }
        }

        bComplete = true;
    }

    private void SetRecord(DQueue queue, ETeamLabel team_label, DPlayer player) {
        DPlayer? target = null;
        if (queue.score_report != null && queue.score_report.bHasSubs) {
            target = queue.score_report.subbed_in;
        }
        else
            target = player;

        var rec = new FGameRecord();
        rec.queue = queue;
        if (!ReferenceEquals(target, null)) {
            rec.bPlayerWon = queue.winner == team_label;
            rec.bError = queue.bError || queue.winner == ETeamLabel.NOT_SET;
            rec.team = team_label;

            target.game_history.Add(rec);

            if (rec.bPlayerWon)
                target.iTotalWins++;
            else
                target.iTotalLosses++;
        }
        else {
            Log("Failed to get match results for queue with Match ID = {0} and Player Name = {1}", queue.match_id.ToString(), player.recorded_names[0]);
            rec.bError = true;
            player.game_history.Add(rec);
        }
    }
}