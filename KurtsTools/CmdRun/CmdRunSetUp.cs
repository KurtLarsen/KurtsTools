namespace NSKurtsTools;

public record CmdRunSetUp{
    public string Command = null!;
    public string? Arguments;
    public string? WorkingDirectory;
    public int TimeOutMilliSec = 1000; // 1 second
}