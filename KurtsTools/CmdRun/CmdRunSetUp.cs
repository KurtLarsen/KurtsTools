namespace NSKurtsTools;

public record CmdRunSetUp{
    public string Command = null!;
    public string? Arguments;
    public string? WorkingDirectory;
    public int TimeOutMilliSec = 1000; // 1 second

   public override string ToString(){
       return $"\x1b[36m{nameof(Command)}\x1b[0m:\n" +
              $"{Command}\n" +
              $"\x1b[36m{nameof(Arguments)}\x1b[0m:\n" +
              $"{Arguments}\n" +
              $"\x1b[36m{nameof(WorkingDirectory)}\x1b[0m:\n" +
              $"{WorkingDirectory}\n" +
              $"\x1b[36m{nameof(TimeOutMilliSec)}\x1b[0m:\n" +
              $"{TimeOutMilliSec}";

   }
}