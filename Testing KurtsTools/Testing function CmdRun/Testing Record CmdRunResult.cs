using System.Runtime.Versioning;
using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_function_CmdRun;

[SupportedOSPlatform("windows")]
[TestFixture]
public class Testing_Record_CmdRunResult{

    [Test]
    public void Property_ExitCodeAsString_returns_fatal(){
        const string nonExistingCommand = @"\dummy.dummy";

        KurtsTools.CmdRunResult cmdRunResult = KurtsTools.CmdRun(nonExistingCommand);
        
        Assume.That(cmdRunResult.ExitCode,Is.EqualTo(KurtsTools.CmdRunResult.ExitCodeFatalError));
        
        Assert.That(cmdRunResult.MeaningOfExifCode,Is.EqualTo("FatalError"));
    }

    [Test]
    public void Testing_property_MeaningOfExitCode_returns_expected_text(){
        const string longRunningCommand = @"cmd.exe";
        const string longRunningArguments = @"/c dir c:\ /s";
        const int timeout = 1;

        CmdRunSetUp cmdRunSetUp = new(){ Command = longRunningCommand, Arguments = longRunningArguments, TimeOutMilliSec = timeout };
        KurtsTools.CmdRunResult cmdRunResult = KurtsTools.CmdRun(cmdRunSetUp);
        
        Assume.That(cmdRunResult.ExitCode,Is.EqualTo(KurtsTools.CmdRunResult.ExitCodeTimeOut));
        
        Assert.That(cmdRunResult.MeaningOfExifCode,Is.EqualTo("TimeOut"));
    }

    [Test]
    public void Testing_Property_MeaningOfExitCode(){
        const string validCommand = @"cmd.exe";
        const string validArguments = @"/c dir ";
        
        KurtsTools.CmdRunResult cmdRunResult = KurtsTools.CmdRun(validCommand,validArguments);
        
        Assume.That(cmdRunResult.ExitCode,Is.Not.EqualTo(KurtsTools.CmdRunResult.ExitCodeFatalError));
        Assume.That(cmdRunResult.ExitCode,Is.Not.EqualTo(KurtsTools.CmdRunResult.ExitCodeTimeOut));

        Assert.That(cmdRunResult.MeaningOfExifCode,Is.EqualTo(string.Empty));
    }

}