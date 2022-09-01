using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Database.Database.Interfaces;

//public class ILogger
//{
//    protected ConsoleColor current_color { get; init; } = ConsoleColor.White;
//    protected int iTabs { get; init; } = 0;
//    protected string class_name { get; init; } = "";

//    protected ILogger(in ConsoleColor current_color, in int iTabs, in string class_name)
//    {
//        this.current_color = current_color;
//        this.iTabs = iTabs;
//        this.class_name = class_name;
//    }

//    protected virtual void logger.Login string message, params string[]? subs_strings)
//    {
//        Console.ForegroundColor = current_color;
//        string msg = "";
//        var caller = new StackFrame(1).GetMethod();
//        string caller_name = "[" + (caller != null ? caller.Name : "") + "] ";
//        for (int i = 0; i < iTabs; i++) msg += "\t";
//        msg += "[" + class_name + "] " + caller_name + message;

//        if (subs_strings != null && subs_strings.Length > 0)
//            Console.WriteLine(msg, subs_strings);
//        else
//            Console.WriteLine(msg);
//    }
//}

public interface ILogger {
    void Log<T>(in string message, params T[]? subs) where T : IDatabaseComponent {
        var header = GetLogLineHeader();
        Console.WriteLine(header + message, subs);
    }
    void Log(in string message, params string[]? substrings) {
        var header = GetLogLineHeader();
        Console.WriteLine(header + message, substrings);
    }
    static Dictionary<int, ConsoleColor> FrameCountToColor { get; set; } = new Dictionary<int, ConsoleColor>() {
        { 0, ConsoleColor.White },
        { 1, ConsoleColor.Yellow },
        { 2, ConsoleColor.Green },
        { 3, ConsoleColor.Blue },
        { 4, ConsoleColor.Red },
        { 5, ConsoleColor.Magenta },
        { 6, ConsoleColor.Cyan},
        { 7, ConsoleColor.DarkBlue }
    };

    string GetLogLineHeader() {
        var calling_frame_method = new StackFrame(1).GetMethod();
        if (calling_frame_method != null) {
            var calling_frame_class = calling_frame_method.DeclaringType;
            if (calling_frame_class != null) {
                var tabs = GetLogLineAttributes();
                return $"{tabs}[{calling_frame_class.Name}] [{calling_frame_method.Name}] ";
            }

            return $"[UnknownClass] [{calling_frame_method.Name}] ";
        }

        return "[Unknown Caller] ";
    }

    string GetLogLineAttributes() {
        var st = new StackTrace(1);
        Console.ForegroundColor = FrameCountToColor[st.FrameCount];
        string tabs = "";
        for(int i = 0; i < st.FrameCount; i++)
            tabs += '\t';

        return tabs;
    }
}