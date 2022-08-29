//change abstract members to virtual where applicable? wouldnt have to reimplement everywhere

using System.Diagnostics;

namespace Database;

public abstract class IDatabaseComponent : ILogger{
    public         bool                bError { get; set; } = false;
    protected      IDatabaseComponent  owner;
    private static IDatabaseComponent? singleton { get; }
    protected      int                 iErrorCount;

    public static implicit operator bool(IDatabaseComponent? component) { return (component != null && !component.bError); }
    public static bool operator ==(IDatabaseComponent? lhs, IDatabaseComponent? rhs) {
        if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

        if (lhs.IsEqual(rhs)) return true;

        return false;
    }
    public static bool operator !=(IDatabaseComponent? lhs, IDatabaseComponent? rhs) { return !(lhs == rhs); }
    public static bool operator <(IDatabaseComponent? lhs, IDatabaseComponent? rhs) {
        if (!lhs || !rhs) return false;
        return lhs.IsLessThan(rhs);
    }
    public static bool operator >(IDatabaseComponent? lhs, IDatabaseComponent? rhs) {
        if (!lhs || !rhs) return false;
        return !(lhs < rhs) && lhs != rhs;
    }
    public static bool operator >=(IDatabaseComponent? lhs, IDatabaseComponent? rhs) {
        if (!lhs || !rhs) return false;
        return !(lhs < rhs) || lhs == rhs;
    }
    public static bool operator <=(IDatabaseComponent? lhs, IDatabaseComponent? rhs) {
        if (!lhs || !rhs) return false;
        return lhs < rhs || lhs == rhs;
    }
    protected abstract bool IsEqual(IDatabaseComponent?    rhs);
    protected abstract bool IsLessThan(IDatabaseComponent? rhs);
    protected IDatabaseComponent(ConsoleColor color = ConsoleColor.White, int tabs = 0, string class_name = "IDatabaseComponent") : base(color, tabs, class_name) {}
    public abstract string ToJson();
    public abstract void   ToJson(string   save_path);
    public abstract void   FromJson(string save_path);
    public static IDatabaseComponent? GetSingleton() { return null; }
}