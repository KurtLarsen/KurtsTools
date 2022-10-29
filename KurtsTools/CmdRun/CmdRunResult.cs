namespace NSKurtsTools;

public static partial class KurtsTools{
    
    // record vs struct: https://stackoverflow.com/questions/64816714/when-to-use-record-vs-class-vs-struct
    public record CmdRunResult(string Command, string? Arguments, int ExitCode, string StdOut, string ErrOut){
        public const int ExitCodeTimeOut = 123456;
        public const int ExitCodeFatalError = 123457;

        // https://stackoverflow.com/questions/1119799/method-chaining-in-c-sharp
        // ReSharper disable once UnusedMember.Global
        public CmdRunResult OutputToConsole(){
            const ConsoleColor commandColor = ConsoleColor.Gray;
            const ConsoleColor exitCodeColor = ConsoleColor.Blue;
            const ConsoleColor errColor = ConsoleColor.Red;
            const ConsoleColor outColor = ConsoleColor.Green;
        
            ColorWriteLine(commandColor, Command + " " + Arguments);
            ColorWriteLine(exitCodeColor, $"Exit code: {ExitCode}");
            if (!string.IsNullOrEmpty(ErrOut))
                ColorWriteLine(ExitCode == 0 ? outColor : errColor, ErrOut);
            if (!string.IsNullOrEmpty(StdOut)) ColorWriteLine(outColor, StdOut);

            return this;
        }

        private static void ColorWriteLine(ConsoleColor color, string? txt){
            Console.ForegroundColor = color;
            Console.WriteLine(txt);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    
        public string ExitCodeAsString{
            get{
                return ExitCode switch{
                    ExitCodeTimeOut => "TimeOut",
                    ExitCodeFatalError => "FatalError",
                    _ => ExitCode.ToString(),
                };
            }
        }
    
    }

    
}

