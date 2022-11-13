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
    
}