using Database.Database.DatabaseCore.MainComponents;

namespace Database.AISerialization; 

public static class PlayerValidator {
    public static bool IsPlayerValid(in DPlayer player) { return IsDiscordIdValid(player) && HasNames(player) && HasGames(player); }

    private static bool IsDiscordIdValid(in DPlayer player) { return player.discord_id != 0; }

    private static bool HasNames(in DPlayer player) { return player.recorded_names.Count != 0; }

    private static bool HasGames(in DPlayer player) { return player.game_history.Count != 0; }
}