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

    [Test]
    public void Testing_fn_ToString(){
        const string theCommand = "TheCommand";
        const string theArguments = "TheArguments";
        const string theWorkingDirectory = "TheWorkingDirectory";
        const int theTimeOutValue = 123;
        const int theExitCode = 456;
        const string theStdOut = "The StdOut";
        const string theErrOut = "The ErrOut";
        

        CmdRunSetUp x = new(){
            Command = theCommand, 
            Arguments = theArguments, 
            WorkingDirectory = theWorkingDirectory,
            TimeOutMilliSec = theTimeOutValue,
        };

        string[] a = x.ToString().Split('\n');
        Assume.That(a.Length,Is.EqualTo(8));

        KurtsTools.CmdRunResult y = new(x, theExitCode, theStdOut, theErrOut);

        string[] s = y.ToString().Split('\n');
        
        Assert.That(s.Length,Is.EqualTo(a.Length+6));
        
        Assert.That(s[8],Does.Match(@"^\x1b\[\d+mExitCode\x1b\[\d+m:$"));
        Assert.That(s[9], Is.EqualTo(theExitCode.ToString()));

        Assert.That(s[10],Does.Match(@"^\x1b\[\d+mErrOut\x1b\[\d+m:$"));
        Assert.That(s[11], Is.EqualTo(theErrOut));

        Assert.That(s[12],Does.Match(@"^\x1b\[\d+mStdOut\x1b\[\d+m:$"));
        Assert.That(s[13], Is.EqualTo(theStdOut));
    }

}