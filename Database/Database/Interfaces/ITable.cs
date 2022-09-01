using Database.Structs;

namespace Database.Database.Interfaces;

public abstract class ITable<TableElementType, ComponentType> : ILogger where TableElementType : ITable<TableElementType, ComponentType> where ComponentType : IDatabaseComponent, new() {
    private SortedList<PrimaryKey, ComponentType> table_contents;
    protected ITable(in ConsoleColor current_color, in int iTabs, in string class_name) {
        //table_contents = new SortedList<PrimaryKey, ComponentType>(Compare);
    }
    public ComponentType? Search(in PrimaryKey key) {

        return null;
    }

    public ComponentType? Search(in string primary_key_as_string) {
        
        return null;
    }
    
    public ComponentType? Search(in ulong key) {

        return null;
    }

    public void Insert(in PrimaryKey? key, in ComponentType? component) {
        if (key == null || component == null) return; // and log error
        table_contents.Add(key.Value, component);
    }

    private  int Compare( PrimaryKey? lhs,  PrimaryKey? rhs) {
        if (lhs == null) {
            if (rhs == null) return 0;
            return -1;
        }
        else {
            if(rhs == null) return 1;
            else {
                if(lhs == rhs) return 0;
                if(rhs < lhs) return -1;
                return 1;
            }
        }
    }
}