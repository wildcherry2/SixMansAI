using Database.Database.Interfaces;

namespace Database.Reports; 

public interface IReport : ILogger {
    public void GenerateReport(in string path);
    public string ParseComponent<ComponentType>(ComponentType component) where ComponentType : IDatabaseComponent;
}