namespace Database.Database.Interfaces; 

public interface IPrimaryKey {
    public ulong TryGetOrCreatePrimaryKey();
}