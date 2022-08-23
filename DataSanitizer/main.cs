class MainClass {
    static void Main(string[] args) {
        var chat = Directory.GetCurrentDirectory() + @"\..\..\..\..\..\RawData\rank-b\June2022.json";
        //Console.WriteLine(Directory.Exists(chat));
        var ds = new Database.Database(@"C:\Users\tyler\Documents\Programming\AI\SixMans\RawData\score-report\July 2022.json", chat);
        ds.BuildDatabase();
    }
}