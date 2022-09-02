using Database.Database.Interfaces;

namespace Database.Reports; 

// Specifically for AI training, nothing to do with the database
public interface IReport : ILogger {
    public void GenerateReport(in string path);
    public string ParseComponent<ComponentType>(ComponentType component) where ComponentType : IDatabaseComponent;
}