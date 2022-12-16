using System.Runtime.Versioning;
using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_function_CmdRun;

[SupportedOSPlatform("windows")]
[TestFixture]
public class TestingStaticFunctionCmdRun{
    [Test]
    public void testing_function_CmdRun_using_DIR(){
        const string command = "cmd.exe";
        const string arguments = "/c dir";

        KurtsTools.CmdRunResult result = KurtsTools.CmdRun(command, arguments);

        Assert.That(result.CmdRunSetUp.Command, Is.EqualTo(command));
        Assert.That(result.CmdRunSetUp.Arguments, Is.EqualTo(arguments));
        Assert.That(result.ExitCode, Is.EqualTo(0),
            $"Unexpected ExitCode in record {nameof(result)}\n\n{nameof(result)} =\n{result}\n");
        Assert.That(result.ErrOut, Is.EqualTo(""));
        Assert.That(result.StdOut, Does.StartWith(" Volume in drive "));
    }

    [Test]
    public void testing_function_CmdRun_using_IPCONFIG(){
        const string command = "ipconfig";

        KurtsTools.CmdRunResult result = KurtsTools.CmdRun(command);

        Assert.That(result.CmdRunSetUp.Command, Is.EqualTo(command));
        Assert.That(result.CmdRunSetUp.Arguments, Is.EqualTo("").Or.EqualTo(null));
        Assert.That(result.ExitCode, Is.EqualTo(0),
            $"Unexpected ExitCode in record {nameof(result)}\n\n{nameof(result)} =\n{result}\n");
        Assert.That(result.ErrOut, Is.EqualTo(""));
        Assert.That(result.StdOut.Trim(), Does.StartWith("Windows IP Configuration"));
    }


    [Test]
    public void testing_timeout_in_function_CmdRun_using_SC_QUERY(){
        // SC.exe is a command line program used for communicating with the Service Control Manager and services.

        // string command = @$"{Environment.GetEnvironmentVariable("SystemRoot")}\System32\sc.exe";
        // const string arguments = @"query";
        const string command = "cmd.exe";
        const string arguments = @"/c dir c:\ /s";
        const int timeoutMilliSec = 1;

        CmdRunSetUp cmdRunSetUp = new(){ Command = command, Arguments = arguments, TimeOutMilliSec = timeoutMilliSec };
        KurtsTools.CmdRunResult result = KurtsTools.CmdRun(cmdRunSetUp);

        Assert.That(result.ExitCode, Is.EqualTo(KurtsTools.CmdRunResult.ExitCodeTimeOut),
            $"Unexpected ExitCode in record {nameof(result)}\n\n{nameof(result)} =\n{result}\n");
    }
    
    [Test]
    public void testing_function_CmdRun_using_SC_QUERY(){
        // SC.exe is a command line program used for communicating with the Service Control Manager and services.

        string command = @$"{Environment.GetEnvironmentVariable("SystemRoot")}\System32\sc.exe";
        const string arguments = @"query";

        KurtsTools.CmdRunResult result = KurtsTools.CmdRun(command, arguments);

        Assert.That(result.ExitCode, Is.EqualTo(0),
            $"Unexpected ExitCode in record {nameof(result)}\n\n{nameof(result)} =\n{result}\n");
        Assert.That(result.ErrOut,Is.EqualTo(""));
        Assert.That(result.StdOut.Trim(),Does.StartWith("SERVICE_NAME:"));
    }

    [Test]
    public void testing_invalid_command_to_function_CmdRun(){
        const string command = @"\dummy.dummy";

        KurtsTools.CmdRunResult result = KurtsTools.CmdRun(command);

        Assert.That(result.ExitCode, Is.EqualTo(KurtsTools.CmdRunResult.ExitCodeFatalError));
        Assert.That(result.ErrOut!="");

    }
    
    [Test]
    public void testing_result_Commandline(){
        const string command = @"\dummy.dummy";

        KurtsTools.CmdRunResult result = KurtsTools.CmdRun(command);

        Assert.That(result.Commandline,Is.EqualTo(command));

    }

}