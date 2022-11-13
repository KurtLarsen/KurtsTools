namespace NSKurtsTools;

public static partial class KurtsTools{
    // record vs struct: https://stackoverflow.com/questions/64816714/when-to-use-record-vs-class-vs-struct
    public record CmdRunResult(CmdRunSetUp CmdRunSetUp, int ExitCode, string StdOut, string ErrOut){
        public const int ExitCodeTimeOut = 123456;
        public const int ExitCodeFatalError = 123457;

        // https://stackoverflow.com/questions/1119799/method-chaining-in-c-sharp
        // ReSharper disable once UnusedMember.Global
        public CmdRunResult OutputToConsole(){
            const ConsoleColor commandColor = ConsoleColor.Gray;
            const ConsoleColor exitCodeColor = ConsoleColor.Blue;
            const ConsoleColor errColor = ConsoleColor.Red;
            const ConsoleColor outColor = ConsoleColor.Green;

            ColorWriteLine(commandColor, CmdRunSetUp.Command + " " + CmdRunSetUp.Arguments);
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

        public string MeaningOfExifCode{
            get{
                return ExitCode switch{
                    ExitCodeTimeOut => "TimeOut",
                    ExitCodeFatalError => "FatalError",
                    _ => "",
                };
            }
        }

        public string Commandline{
            get{
                string s = CmdRunSetUp.Command;
                if (CmdRunSetUp.Arguments != string.Empty) s += " " + CmdRunSetUp.Arguments;
                return s.Trim();
            }
        }

        // ReSharper disable once ArrangeAccessorOwnerBody
        public string WorkingDirectory{
            get{ return CmdRunSetUp.WorkingDirectory!; }
        }


        public override string ToString(){

            return $"\x1b[36m.{nameof(ExitCode)}\x1b[0m was <{ExitCode}> when running command line:\n" +
                   $"{Commandline}\n" +
                   $"\x1b[36m.{nameof(WorkingDirectory)}\x1b[0m:\n" +
                   $"{WorkingDirectory}\n" +
                   $"\x1b[36m.{nameof(ErrOut)}\x1b[0m:\n" +
                   $"{ErrOut.Trim()}\n" +
                   $"\x1b[36m.{nameof(StdOut)}\x1b[0m:\n" +
                   $"{StdOut.Trim()}";
        }
    }
}