namespace Database.Structs;

public class FSeasonLabel {
    public string month { get; set; } = "-1";
    public string year  { get; set; } = "-1";

    public FSeasonLabel(DateTime? dt) {
        if(dt != null) {
            month = dt.Value.ToString("MMMM");
            year = dt.Value.Year.ToString();
        }
    }

    public static bool operator ==(FSeasonLabel left, FSeasonLabel right) {
        return left.month == right.month && left.year == right.year;
    }

    public static bool operator !=(FSeasonLabel left, FSeasonLabel right) {
        return !(left == right);
    }

    public static implicit operator bool(FSeasonLabel label) {
        return label.month != "-1" && label.year != "-1";
    }
}