using System.Text.Json;

namespace DataSanitizer;

public class Player {
    public Player() {
        recorded_names = new List<string>();
        game_history = new List<GameHistoryRecord>();
    }

    public Player(string name, ulong discord_id, string nickname = "") {
        recorded_names = new List<string>();
        if(name.Length > 0) recorded_names.Add(name);
        if(nickname.Length > 0 && nickname != name) recorded_names.Add(nickname);
        game_history = new List<GameHistoryRecord>();
        this.discord_id = discord_id;
    }

    public Player(List<string> recorded_names, ulong discord_id) {
        game_history = new List<GameHistoryRecord>();
        this.recorded_names = recorded_names;
        this.discord_id = discord_id;
    }

    public List<GameHistoryRecord> game_history   { get; }
    public List<string>            recorded_names { get; set; }
    public ulong                   discord_id     { get; }
    public int                     total_wins     { get; set; }
    public int                     total_losses   { get; set; }

    public void AddName(string name) {
        recorded_names.Add(name);
    }

    public void AddGameHistoryRecord(GameHistoryRecord record) {
        game_history.Add(record);
    }

    public double GetTotalWinRateRatio() {
        return (double)total_wins / (total_wins + total_losses);
    }

    public string ToJson() {
        return JsonSerializer.Serialize(this);
    }

    public static Player? FromJson(string json_s) {
        return JsonSerializer.Deserialize<Player>(json_s);
    }

    public static bool operator ==(Player? left, Player? right) {
        if (ReferenceEquals(left, null) && ReferenceEquals(right, null)) return true;
        else if ((ReferenceEquals(left, null) && !ReferenceEquals(right, null)) || (!ReferenceEquals(left, null) && ReferenceEquals(right, null))) return false;
        return left.discord_id == right.discord_id;
    }

    public static bool operator ==(string name, Player? player) {
        if (player == null) return false;
        return player.recorded_names.Any(recorded_name => name == recorded_name);
    }

    public static bool operator !=(Player left, Player? right) {
        return !(left == right);
    }

    public static bool operator !=(string name, Player? player) {
        return !(name == player);
    }

    public override bool Equals(object obj) {
        return this == (Player)obj;
    }

    public override int GetHashCode() {
        return (int)discord_id;
    }

    public bool HasName(ref string name) {
        foreach (var current_name in recorded_names) {
            if (current_name == name) return true;
        }

        return false;
    }

    public bool IsPlayer(ulong id) {
        return (discord_id == id);
    }

    public class GameHistoryRecord {
        public GameHistoryRecord(DateTime date_of_game, bool won_game) {
            this.date_of_game = date_of_game;
            this.won_game = won_game;
        }

        public DateTime date_of_game { get; }
        public bool     won_game     { get; }
    }

    public class GameRecord {
        public GameRecord(Player player, DateTime record_to_this_date) {
            this.player = player;
            this.record_to_this_date = record_to_this_date;

            foreach (var record in player.game_history) {
                if (record.date_of_game > record_to_this_date) continue;
                if (record.won_game) wins++;
                else losses++;
            }
        }

        private Player   player              { get; }
        private DateTime record_to_this_date { get; }
        public  int      wins                { get; }
        public  int      losses              { get; }

        public double GetWinRateToDate() {
            return (double)wins / (wins + losses);
        }
    }
}