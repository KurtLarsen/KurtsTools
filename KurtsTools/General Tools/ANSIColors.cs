// ReSharper disable UnusedMember.Global
namespace NSKurtsTools;

public static class Ansi{
    public const string Reset = "\x1b[0m";
    
    public const string FrontBlack = "\x1b[30m";
    public const string FrontRed = "\x1b[31m";
    public const string FrontGreen = "\x1b[32m";
    public const string FrontYellow = "\x1b[33m";
    public const string FrontBlue = "\x1b[34m";
    public const string FrontMagenta = "\x1b[35m";
    public const string FrontCyan = "\x1b[36m";
    public const string FrontWhite = "\x1b[37m";
    
    public const string BackBlack = "\x1b[40m";
    public const string BackRed = "\x1b[41m";
    public const string BackGreen = "\x1b[42m";
    public const string BackYellow = "\x1b[43m";
    public const string BackBlue = "\x1b[44m";
    public const string BackMagenta = "\x1b[45m";
    public const string BackCyan = "\x1b[46m";
    public const string BackWhite = "\x1b[47m";

    public static string Black(string text){
        return FrontBlack + text + Reset;
    }
    public static string Red(string text){
        return FrontRed + text + Reset;
    }
    public static string Green(string text){
        return FrontGreen + text + Reset;
    }
    public static string Yellow(string text){
        return FrontYellow + text + Reset;
    }

    public static string Magenta(string text){
        return FrontMagenta + text + Reset;
    }
    public static string Cyan(string text){
        return FrontCyan + text + Reset;
    }
    public static string White(string text){
        return FrontWhite + text + Reset;
    }
    public static string Blue(string text){
        return FrontBlue + text + Reset;
    }
}


public static class AnsiConsole{
    public static void WriteLine(string frontBlue, string text){
        throw new NotImplementedException();
    }
}