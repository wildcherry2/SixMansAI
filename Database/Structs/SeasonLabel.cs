namespace Database.Structs;

public class FSeasonLabel {
    public string month = "-1";
    public int    year { get; set; }

    public FSeasonLabel(DateTime? dt) {
        month = dt.Value.ToString("MMMM");
        year = dt.Value.Year;
    }
}