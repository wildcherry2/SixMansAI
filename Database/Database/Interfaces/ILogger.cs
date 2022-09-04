using System.Diagnostics;

namespace Database.Database.Interfaces; 

public class ILogger {
    protected ILogger(in ConsoleColor current_color, in int iTabs, in string class_name) {
        this.current_color = current_color;
        this.iTabs         = iTabs;
        this.class_name    = class_name;
    }
    protected ConsoleColor current_color { get; init; } = ConsoleColor.White;
    protected int          iTabs         { get; init; }
    protected string       class_name    { get; init; } = "";

    protected virtual void Log(in string message, params string[]? subs_strings) {
        Console.ForegroundColor = current_color;
        var msg         = "";
        var caller      = new StackFrame(1).GetMethod();
        var caller_name = "[" + (caller != null ? caller.Name : "") + "] ";
        for (var i = 0; i < iTabs; i++) { msg += "\t"; }

        msg += "[" + class_name + "] " + caller_name + message;
        if (subs_strings != null && subs_strings.Length > 0) { Console.WriteLine(msg, subs_strings); }
        else { Console.WriteLine(msg); }
    }
}