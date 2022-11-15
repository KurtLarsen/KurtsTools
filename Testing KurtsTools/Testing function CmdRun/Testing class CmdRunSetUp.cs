using System.Runtime.Versioning;
using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_function_CmdRun;

[TestFixture,SupportedOSPlatform("windows")]
public class Testing_Class_CmdRunSetUp{
    private string _tempDir = null!;
    
    [SetUp]
    public void SetUp(){
        _tempDir = KurtsTools.NewTempDirectory();
    }

    [TearDown]
    public void TearDown(){
        KurtsTools.DeleteDirectory(_tempDir);
    }
    
    [Test]
    public void Property_WorkingDirectory_sets_the_directory_where_the_command_is_executed(){
        CmdRunSetUp x = new(){Command = "cmd.exe",Arguments = "/c dir",WorkingDirectory = _tempDir};
        KurtsTools.CmdRunResult result=KurtsTools.CmdRun(x);
        
        Assume.That(result.ExitCode,Is.Zero);
        Assume.That(result.ErrOut,Is.Empty);

        string expectedText = $"Directory of {_tempDir.TrimEnd('\\')}";
        
        Assert.That(result.StdOut,Does.Contain(expectedText),result.StdOut);
        
    }

    [Test]
    public void Testing_fn_ToString(){
        const string theCommand = "The Command";
        const string theArguments = "The Arguments";
        const string theWorkingDirectory = "The WorkingDirectory";
        const int theTimeOutValue = 123;
        

        CmdRunSetUp x = new(){
            Command = theCommand, 
            Arguments = theArguments, 
            WorkingDirectory = theWorkingDirectory,
            TimeOutMilliSec = theTimeOutValue,
        };

        string[] a = x.ToString().Split('\n');
        
        Assert.That(a.Length,Is.EqualTo(8));

        Assert.That(a[0],Does.Match(@"^\x1b\[\d+mCommand\x1b\[\d+m:$"));
        Assert.That(a[1],Is.EqualTo(theCommand));
        Assert.That(a[2],Does.Match(@"^\x1b\[\d+mArguments\x1b\[\d+m:$"));
        Assert.That(a[3],Is.EqualTo(theArguments));
        Assert.That(a[4],Does.Match(@"^\x1b\[\d+mWorkingDirectory\x1b\[\d+m:$"));
        Assert.That(a[5],Is.EqualTo(theWorkingDirectory));
        Assert.That(a[6],Does.Match(@"^\x1b\[\d+mTimeOutMilliSec\x1b\[\d+m:$"));
        Assert.That(a[7],Is.EqualTo(theTimeOutValue.ToString()));
    }
    
}