using KurtsToolsLibrary.KurtsFileTools;
using NUnit.Framework;

namespace Testing_KurtsToolsLibrary.Testing_File_Tools.Testing_MoveStackPhotosToSubDirectories;

[TestFixture]
public class Testing_MoveStackPhotosToSubDirectories{
    private const string PathToExifToolExe = @"C:\Program Files\exiftool\exiftool.exe";
    private string _tempDir = null!;
    private const string Fn = $"{nameof(KurtsFileTools)}.{nameof(KurtsFileTools.MoveStackPhotosToSubDirectories)}()";

    [SetUp]
    public void SetUp(){
        string testDataDirectory=TestContext.CurrentContext.TestDirectory +
                                 @"\Testing File Tools\Testing MoveStackPhotosToSubDirectories\TestData for Testing MoveStackPhotosToSubDirectories";
        Assume.That(testDataDirectory,Does.Exist);
        
        _tempDir = KurtsFileTools.NewTempDirectory();

        KurtsFileTools.CopyDirectory(testDataDirectory,_tempDir);
    }

    [TearDown]
    public void TearDown(){
        KurtsFileTools.DeleteDirectory(_tempDir);
    }

    [Test]
    public void test_of_MoveStackPhotosToSubDirectories(){
        DirectoryInfo di = new(_tempDir);
        
        Assume.That(di.GetFiles().Length,Is.EqualTo(7),$"Number of files in {nameof(_tempDir)} BEFORE {Fn}");
        
        Assume.That(di.GetDirectories().Length,Is.EqualTo(0),$"Number of subDirectories in {nameof(_tempDir)} BEFORE {Fn}");
        
        KurtsFileTools.MoveStackPhotosToSubDirectories(_tempDir, PathToExifToolExe);
        
        di.Refresh();
        
        Assert.That(di.GetFiles().Length,Is.EqualTo(1),$"Number of files in {nameof(_tempDir)} AFTER {Fn}");

        DirectoryInfo[] subDirectories = di.GetDirectories();
        Assert.That(subDirectories.Length,Is.EqualTo(2),$"Number of subDirectories in {nameof(_tempDir)} AFTER {Fn}");

        for (int n = 0; n < subDirectories.Length; n++){
            Assert.That(subDirectories[n].Name,Is.EqualTo("group_"+(n+1)));
        }
        
        
    }
}