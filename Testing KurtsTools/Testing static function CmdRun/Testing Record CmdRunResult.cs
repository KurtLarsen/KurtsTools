using System.Runtime.Versioning;
using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_static_function_CmdRun;

[SupportedOSPlatform("windows")]
[TestFixture]
public class Testing_Record_CmdRunResult{

    [Test]
    public void Property_ExitCodeAsString_returns_fatal(){
        const string nonExistingCommand = @"\dummy.dummy";

        KurtsTools.CmdRunResult cmdRunResult = KurtsTools.CmdRun(nonExistingCommand);
        
        Assume.That(cmdRunResult.ExitCode,Is.EqualTo(KurtsTools.CmdRunResult.ExitCodeFatalError));
        
        Assert.That(cmdRunResult.ExitCodeAsString,Is.EqualTo("FatalError"));
    }

    [SupportedOSPlatform("windows")]
    [Test]
    public void Property_ExitCodeAsString_returns_timeout(){
        const string longRunningCommand = @"cmd.exe";
        const string longRunningArguments = @"/c dir c:\ /s";
        const int timeout = 1;

        KurtsTools.CmdRunResult cmdRunResult = KurtsTools.CmdRun(longRunningCommand,longRunningArguments,timeout);
        
        Assume.That(cmdRunResult.ExitCode,Is.EqualTo(KurtsTools.CmdRunResult.ExitCodeTimeOut));
        
        Assert.That(cmdRunResult.ExitCodeAsString,Is.EqualTo("TimeOut"));
    }

    [SupportedOSPlatform("windows")]
    [Test]
    public void Property_ExitCodeAsString_return_ExitCode_as_integer_string(){
        const string validCommand = @"cmd.exe";
        const string validArguments = @"/c dir ";
        
        KurtsTools.CmdRunResult cmdRunResult = KurtsTools.CmdRun(validCommand,validArguments);
        
        Assume.That(cmdRunResult.ExitCode,Is.Not.EqualTo(KurtsTools.CmdRunResult.ExitCodeFatalError));
        Assume.That(cmdRunResult.ExitCode,Is.Not.EqualTo(KurtsTools.CmdRunResult.ExitCodeTimeOut));

        string expectedValue = cmdRunResult.ExitCode.ToString();
        
        Assert.That(cmdRunResult.ExitCodeAsString,Is.EqualTo(expectedValue));
    }

}