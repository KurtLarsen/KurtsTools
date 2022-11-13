// See https://aka.ms/new-console-template for more information

using NSKurtsTools;
using static NSKurtsTools.KurtsTools;

CmdRunSetUp x = new(){Command = "cmd.exe",Arguments = "/c dir",WorkingDirectory = @"c:\"};
CmdRunResult result=CmdRun(x);
Console.WriteLine($"result.ExitCode: <{result.ExitCode}>");
Console.WriteLine($"result.ErrOut:\n{result.ErrOut}");
Console.WriteLine($"result.StdOut:\n{result.StdOut}");