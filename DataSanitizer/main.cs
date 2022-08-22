class MainClass {
    static void Main(string[] args) {
        var ds = new Database.Database(@"C:\Users\tyler\Documents\Programming\AI\SixMans\RawData\score-report\July 2022.json",@"C:\Users\tyler\Documents\Programming\AI\SixMans\RawData\rank-b\July2022All.json");
        ds.BuildDatabase();
    }
}