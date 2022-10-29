
// by Kurt, october 2021

using System.Runtime.Versioning;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_static_function_CmdRun;

[SupportedOSPlatform("windows")]
[TestFixture]
public class CmdRunKeepWindowOpen{
    [Test,Ignore($"Testing function {nameof(CmdRunKeepWindowOpen)}() is under normal circumstances ignored because it opens a cmd-window, and keeps it open")]
    public void Testing_function_CmdRunKeepWindowOpen(){
        const string command = "cmd.exe";
        const string arguments = "/k dir";

        NSKurtsTools.KurtsTools.CmdRunKeepWindowOpen(command, arguments);

        Assert.Pass();

    }
}