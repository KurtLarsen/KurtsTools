using System.Runtime.Versioning;
using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_static_function_CmdRun;

[SupportedOSPlatform("windows")]
[TestFixture]
public class TestingStaticFunctionCmdRun{
    [Test]
    public void testing_function_CmdRun_using_DIR(){
        const string command = "cmd.exe";
        const string arguments = "/c dir";

        KurtsTools.CmdRunResult result = KurtsTools.CmdRun(command, arguments);

        Assert.That(result.Command, Is.EqualTo(command));
        Assert.That(result.Arguments, Is.EqualTo(arguments));
        Assert.That(result.ExitCode, Is.EqualTo(0),
            $"Unexpected ExitCode in record {nameof(result)}\n\n{nameof(result)} =\n{result}\n");
        Assert.That(result.ErrOut, Is.EqualTo(""));
        Assert.That(result.StdOut, Does.StartWith(" Volume in drive "));
    }

    [SupportedOSPlatform("windows")]
    [Test]
    public void testing_function_CmdRun_using_IPCONFIG(){
        const string command = "ipconfig";

        KurtsTools.CmdRunResult result = KurtsTools.CmdRun(command);

        Assert.That(result.Command, Is.EqualTo(command));
        Assert.That(result.Arguments, Is.EqualTo("").Or.EqualTo(null));
        Assert.That(result.ExitCode, Is.EqualTo(0),
            $"Unexpected ExitCode in record {nameof(result)}\n\n{nameof(result)} =\n{result}\n");
        Assert.That(result.ErrOut, Is.EqualTo(""));
        Assert.That(result.StdOut.Trim(), Does.StartWith("Windows IP Configuration"));
    }


    [SupportedOSPlatform("windows")]
    [Test]
    public void testing_timeout_in_function_CmdRun_using_SC_QUERY(){
        // const string command = @"cmd.exe";
        // const string arguments = @"/k c:\windows\System32\sc.exe query";

        const string command = @"c:\windows\System32\sc.exe";
        const string arguments = @"query";
        const int timeoutMilliSec = 1;

        KurtsTools.CmdRunResult result = KurtsTools.CmdRun(command, arguments,timeoutMilliSec);

        Assert.That(result.ExitCode, Is.EqualTo(KurtsTools.CmdRunResult.ExitCodeTimeOut),
            $"Unexpected ExitCode in record {nameof(result)}\n\n{nameof(result)} =\n{result}\n");
    }
    
    [SupportedOSPlatform("windows")]
    [Test]
    public void testing_function_CmdRun_using_SC_QUERY(){
        // const string command = @"cmd.exe";
        // const string arguments = @"/k c:\windows\System32\sc.exe query";

        const string command = @"c:\windows\System32\sc.exe";
        const string arguments = @"query";

        KurtsTools.CmdRunResult result = KurtsTools.CmdRun(command, arguments);

        Assert.That(result.ExitCode, Is.EqualTo(0),
            $"Unexpected ExitCode in record {nameof(result)}\n\n{nameof(result)} =\n{result}\n");
        Assert.That(result.ErrOut,Is.EqualTo(""));
        Assert.That(result.StdOut.Trim(),Does.StartWith("SERVICE_NAME:"));
    }

    [SupportedOSPlatform("windows")]
    [Test]
    public void testing_invalid_command_to_function_CmdRun(){
        const string command = @"\dummy.dummy";

        KurtsTools.CmdRunResult result = KurtsTools.CmdRun(command);

        Assert.That(result.ExitCode, Is.EqualTo(KurtsTools.CmdRunResult.ExitCodeFatalError));
        Assert.That(result.ErrOut!="");

    }
}