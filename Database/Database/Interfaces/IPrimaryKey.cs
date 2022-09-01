using Database.Structs;

namespace Database.Database.Interfaces; 

public interface IPrimaryKey {
    public PrimaryKey TryGetOrCreatePrimaryKey();
}