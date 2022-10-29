using System.Runtime.Versioning;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_PhotoTools.Testing_MoveStackPhotosToSubDirectories;

[SupportedOSPlatform("windows")]
[TestFixture]
public class Testing_MoveStackPhotosToSubDirectories{
    private const string PathToExifToolExe = @"C:\Program Files\exiftool\exiftool.exe";
    private static string _pathToTestDataForTestingMoveStackPhotosToSubDirectories = null!;
    private string _tempDir = null!;
    private const string Fn = $"{nameof(NSKurtsTools.KurtsTools)}.{nameof(NSKurtsTools.KurtsTools.MoveStackPhotosToSubDirectories)}()";

    [OneTimeSetUp]
    public void OneTimeSetup(){
        _pathToTestDataForTestingMoveStackPhotosToSubDirectories = TestContext.CurrentContext.TestDirectory +
                                               @"/Testing PhotoTools/Testing MoveStackPhotosToSubDirectories/TestData for Testing MoveStackPhotosToSubDirectories";
        
        Assume.That(_pathToTestDataForTestingMoveStackPhotosToSubDirectories,Does.Exist);
    }

    
    [SetUp]
    public void SetUp(){
        string testDataDirectory = _pathToTestDataForTestingMoveStackPhotosToSubDirectories;
        
        Assume.That(testDataDirectory,Does.Exist);
        
        _tempDir =NSKurtsTools.KurtsTools.NewTempDirectory();

        NSKurtsTools.KurtsTools.CopyDirectory(testDataDirectory,_tempDir);
    }

    [SupportedOSPlatform("windows")]
    [TearDown]
    public void TearDown(){
        NSKurtsTools.KurtsTools.DeleteDirectory(_tempDir);
    }

    [Test]
    public void test_of_MoveStackPhotosToSubDirectories(){
        DirectoryInfo di = new(_tempDir);
        
        Assume.That(di.GetFiles().Length,Is.EqualTo(7),$"Number of files in {nameof(_tempDir)} BEFORE {Fn}");
        
        Assume.That(di.GetDirectories().Length,Is.EqualTo(0),$"Number of subDirectories in {nameof(_tempDir)} BEFORE {Fn}");
        
        NSKurtsTools.KurtsTools.MoveStackPhotosToSubDirectories(_tempDir, PathToExifToolExe);
        
        di.Refresh();
        
        Assert.That(di.GetFiles().Length,Is.EqualTo(1),$"Number of files in {nameof(_tempDir)} AFTER {Fn}");

        DirectoryInfo[] subDirectories = di.GetDirectories();
        Assert.That(subDirectories.Length,Is.EqualTo(2),$"Number of subDirectories in {nameof(_tempDir)} AFTER {Fn}");

        for (int n = 0; n < subDirectories.Length; n++){
            Assert.That(subDirectories[n].Name,Is.EqualTo("group_"+(n+1)));
        }
        
        
    }
}