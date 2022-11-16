
// by Kurt, october 2021

using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_File_Tools;

[TestFixture,SupportedOSPlatform("windows")]
public class Testing_Fn_NewFileName{
    private readonly int _lengthOfGuid = Guid.NewGuid().ToString().Length;
    private readonly Regex _guidRegex=new("[0-9A-Fa-f]{8}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{12}");

    [Test]
    [TestCase(".",TestName = ".")]
    [TestCase(@".\",TestName = @".\")]
    public void Testing_fn_NewFileName_with_string_path(string directory){
        // const string directoryWithoutBackslash = ".";
        Assume.That(directory,Does.Exist);
        
        string result =KurtsTools.NewFileName(".");
        
        string dir = result[..^_lengthOfGuid];
        string name = result[^_lengthOfGuid..];
        
        Assert.That(dir,Does.Exist);
        Assert.That(name,Does.Match(_guidRegex));
    }
    
    [Test]
    [TestCase(".",TestName = ".")]
    [TestCase(@".\",TestName = @".\")]
    public void Testing_fn_NewFileName_with_DirectoryInfo(string directory){
        Assume.That(directory,Does.Exist);
        
        DirectoryInfo di = new(directory);
        
        string result =KurtsTools.NewFileName(di);

        string dir = result[..^_lengthOfGuid];
        string name = result[^_lengthOfGuid..];
        
        Assert.That(dir,Does.Exist);
        Assert.That(name,Does.Match(_guidRegex));
    }

    [Test]
    public void Testing_fn_NewFileName_without_arguments(){
        string result =KurtsTools.NewFileName();

        string dir = result[..^_lengthOfGuid];
        string name = result[^_lengthOfGuid..];
        
        Assert.That(dir,Does.Exist);
        Assert.That(name,Does.Match(_guidRegex));
        
    }

}