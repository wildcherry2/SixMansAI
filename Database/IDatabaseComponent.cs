//change abstract members to virtual where applicable? wouldnt have to reimplement everywhere
namespace Database;

public abstract class IDatabaseComponent {
    protected      bool                bError = false;
    protected      ConsoleColor        current_color { get; set; }
    protected      int                 iTabs         { get; set; }
    protected      string              class_name    { get; set; }
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

    // add in System.Reflection.MethodBase.GetCurrentMethod().Name to print with function name?
    protected void Log(string message, params string[]? subs_strings) {
        Console.ForegroundColor = current_color;
        string msg = "";
        for (int i = 0; i < iTabs; i++) msg += "\t";
        msg += "[" + class_name + "] " + message;

        if (subs_strings != null && subs_strings.Length > 0)
            Console.WriteLine(msg, subs_strings);
        else
            Console.WriteLine(msg);
    }

    protected IDatabaseComponent(ConsoleColor color = ConsoleColor.White, int tabs = 0, string class_name = "IDatabaseComponent") {
        current_color = color;
        iTabs = tabs;
        this.class_name = class_name;
    }

    public abstract string ToJson();
    public abstract void   ToJson(string   save_path);
    public abstract void   FromJson(string save_path);

    public static IDatabaseComponent? GetSingleton() { return null; }
}