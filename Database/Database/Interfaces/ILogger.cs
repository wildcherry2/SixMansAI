using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Database.Database.Interfaces;

public class ILogger
{
    protected ConsoleColor current_color { get; set; } = ConsoleColor.White;
    protected int iTabs { get; set; } = 0;
    protected string class_name { get; set; } = "";

    protected ILogger(ConsoleColor current_color, int iTabs, string class_name)
    {
        this.current_color = current_color;
        this.iTabs = iTabs;
        this.class_name = class_name;
    }

    protected virtual void Log(string message, params string[]? subs_strings)
    {
        Console.ForegroundColor = current_color;
        string msg = "";
        var caller = new StackFrame(1).GetMethod();
        string caller_name = "[" + (caller != null ? caller.Name : "") + "] ";
        for (int i = 0; i < iTabs; i++) msg += "\t";
        msg += "[" + class_name + "] " + caller_name + message;

        if (subs_strings != null && subs_strings.Length > 0)
            Console.WriteLine(msg, subs_strings);
        else
            Console.WriteLine(msg);
    }
}