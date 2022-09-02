using Database.Database.DatabaseCore.MainComponents;
using Database.Database.Enums;

namespace Database.AISerialization {
    public static class QueueValidator {
        public static bool IsQueueValid(in DQueue queue) { return IsWinnerSet(queue) && HasDate(queue) && ArePlayersSet(queue) && ArePlayersValid(queue); }

        private static bool ArePlayersSet(in DQueue queue) {
            return !ReferenceEquals(queue.team_one.player_one, null) &&
                   !ReferenceEquals(queue.team_one.player_two, null) &&
                   !ReferenceEquals(queue.team_one.player_three, null) &&
                   !ReferenceEquals(queue.team_two.player_one, null) &&
                   !ReferenceEquals(queue.team_two.player_two, null) &&
                   !ReferenceEquals(queue.team_two.player_three, null);
        }

        private static bool ArePlayersValid(in DQueue queue) {
            return PlayerValidator.IsPlayerValid(queue.team_one.player_one) &&
                   PlayerValidator.IsPlayerValid(queue.team_one.player_two) &&
                   PlayerValidator.IsPlayerValid(queue.team_one.player_three) &&
                   PlayerValidator.IsPlayerValid(queue.team_two.player_one) &&
                   PlayerValidator.IsPlayerValid(queue.team_two.player_two) &&
                   PlayerValidator.IsPlayerValid(queue.team_two.player_three);
        }

        private static bool IsWinnerSet(in DQueue queue) { return queue.winner != ETeamLabel.NOT_SET; }

        private static bool HasDate(in DQueue queue) { return queue.teams_picked_message.timestamp != null; }
    }
}