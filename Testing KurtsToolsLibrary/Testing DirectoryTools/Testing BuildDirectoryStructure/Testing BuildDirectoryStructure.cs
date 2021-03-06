using System.Xml;
using KurtsToolsLibrary.DirectoryTools;
using KurtsToolsLibrary.XmlTools;
using NUnit.Framework;

namespace Testing_KurtsToolsLibrary.Testing_DirectoryTools.Testing_BuildDirectoryStructure;

[TestFixture]
public class Testing_BuildDirectoryStructure{
    private static string _pathToTestDataForTestingMoveStackPhotosToSubDirectories = null!;
    private string _tempDir = "";

    private static XmlDocument _xmlDocumentWithFileStructure = null!;

    private const string ContentGivenAsTextNode = "This is content of file4.txt, given as #text node";
    private const string ContentGivenAsAttribute = "This is content of file5.txt, given as attribute value";

    private static string _pathToContentGivenAsSource = null!;



    [OneTimeSetUp]
    public static void OneTimeSetUp(){
        _pathToTestDataForTestingMoveStackPhotosToSubDirectories = TestContext.CurrentContext.TestDirectory +
                                                                   @"/Testing DirectoryTools/Testing BuildDirectoryStructure/TestData for Testing BuildDirectoryStructure";
        Assume.That(_pathToTestDataForTestingMoveStackPhotosToSubDirectories, Does.Exist);

       _pathToContentGivenAsSource = _pathToTestDataForTestingMoveStackPhotosToSubDirectories + "/fileWithContent.txt";
       Assume.That(_pathToContentGivenAsSource, Does.Exist);

       string fileStructureAsXmlString = "<root>" +
            "    <directory name='subdir1'>" +
            "        <file name='file1.txt' />" +
            "        <file name='file2.txt' />" +
            "    </directory>" +
            "    <directory name='subdir2' />" +
            "    <file name='file3.txt' />" +
            $"    <file name='file4.txt'>{ContentGivenAsTextNode}</file>" +
            $"    <file name='file5.txt' content='{ContentGivenAsAttribute}' />" +
            $"    <file name='file6.txt' source='{_pathToContentGivenAsSource}' />" +
            "</root>";
        _xmlDocumentWithFileStructure = XmlTools.StringToXmlDocument(fileStructureAsXmlString);
    }

    [SetUp]
    public void SetUp(){
        _tempDir = DirectoryTools.NewTempDirectory();
    }

    [TearDown]
    public void TearDown(){
        DirectoryTools.DeleteDirectory(_tempDir);
    }


    [Test]
    public void Testing_Content_In_File(){
        DirectoryTools.BuildDirectoryStructure(_tempDir, _xmlDocumentWithFileStructure);

        DirectoryInfo di = new(_tempDir);

        FileInfo[] files = di.GetFiles();
        Assert.That(files.Length, Is.EqualTo(4));
        Assert.That(files[0].Name, Is.EqualTo("file3.txt"));
        Assert.That(files[1].Name, Is.EqualTo("file4.txt"));
        Assert.That(files[2].Name, Is.EqualTo("file5.txt"));
        Assert.That(files[3].Name, Is.EqualTo("file6.txt"));

        string s = File.ReadAllText(files[1].FullName);
        Assert.That(s, Is.EqualTo(ContentGivenAsTextNode));

        s = File.ReadAllText(files[2].FullName);
        Assert.That(s, Is.EqualTo(ContentGivenAsAttribute));

        s = File.ReadAllText(files[3].FullName);
        Assert.That(s, Is.EqualTo(File.ReadAllText(_pathToContentGivenAsSource)));

        DirectoryInfo[] subdirs = di.GetDirectories();

        Assert.That(subdirs.Length, Is.EqualTo(2));
        Assert.That(subdirs[0].Name, Is.EqualTo("subdir1"));
        Assert.That(subdirs[1].Name, Is.EqualTo("subdir2"));

        FileInfo[] filesInSubdir1 = subdirs[0].GetFiles();

        Assert.That(filesInSubdir1.Length, Is.EqualTo(2));
        Assert.That(filesInSubdir1[0].Name, Is.EqualTo("file1.txt"));
        Assert.That(filesInSubdir1[1].Name, Is.EqualTo("file2.txt"));

        FileInfo[] filesInSubdir2 = subdirs[1].GetFiles();

        Assert.That(filesInSubdir2.Length, Is.EqualTo(0));
    }
}