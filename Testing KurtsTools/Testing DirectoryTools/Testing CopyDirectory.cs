using System.Runtime.Versioning;
using System.Xml;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_DirectoryTools;

[SupportedOSPlatform("windows")]
[TestFixture]
public class Testing_CopyDirectory{
    private string _tempFromDirectory = null!;
    private string _tempToDirectory = null!;


    [SetUp]
    public void SetUp(){
        _tempFromDirectory = NSKurtsTools.KurtsTools.NewTempDirectory();
        _tempToDirectory = NSKurtsTools.KurtsTools.NewTempDirectory();

        XmlDocument xmlFileStructure = NSKurtsTools.KurtsTools.StringToXmlDocument("<root>" +
                                                                      "    <directory name='subdir1'>" +
                                                                      "        <file name='file1.txt' />" +
                                                                      "        <file name='file2.txt' />" +
                                                                      "    </directory>" +
                                                                      "    <directory name='subdir2' />" +
                                                                      "    <file name='file3.txt' />" +
                                                                      "    <file name='file4.txt' />" +
                                                                      "</root>");
        NSKurtsTools.KurtsTools.BuildDirectoryStructure(_tempFromDirectory, xmlFileStructure.DocumentElement!);
    }


    [TearDown]
    public void TearDown(){
        NSKurtsTools.KurtsTools.DeleteDirectory(pathToDirectory: _tempFromDirectory);
        NSKurtsTools.KurtsTools.DeleteDirectory(pathToDirectory: _tempToDirectory);
    }

    [Test]
    public void CopyDirectory_can_copy_to_existing_directory(){
        NSKurtsTools.KurtsTools.CopyDirectory(_tempFromDirectory, _tempToDirectory);

        Assert.That(_tempToDirectory + "subdir1", Does.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file1.txt", Does.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file2.txt", Does.Exist);
        Assert.That(_tempToDirectory + "subdir2", Does.Exist);
        Assert.That(_tempToDirectory + "file3.txt", Does.Exist);
        Assert.That(_tempToDirectory + "file4.txt", Does.Exist);
    }

    [Test]
    public void CopyDirectory_can_copy_to_non_existing_directory(){
        NSKurtsTools.KurtsTools.DeleteDirectory(pathToDirectory: _tempToDirectory);
        Assume.That(_tempToDirectory, Does.Not.Exist);

        NSKurtsTools.KurtsTools.CopyDirectory(_tempFromDirectory, _tempToDirectory);

        Assert.That(_tempToDirectory, Does.Exist);
        Assert.That(_tempToDirectory + "subdir1", Does.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file1.txt", Does.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file2.txt", Does.Exist);
        Assert.That(_tempToDirectory + "subdir2", Does.Exist);
        Assert.That(_tempToDirectory + "file3.txt", Does.Exist);
        Assert.That(_tempToDirectory + "file4.txt", Does.Exist);
    }

    [Test]
    public void Non_recursive_CopyDirectory_can_copy_to_existing_directory(){
        NSKurtsTools.KurtsTools.CopyDirectory(_tempFromDirectory, _tempToDirectory, false);

        Assert.That(_tempToDirectory + "subdir1", Does.Not.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file1.txt", Does.Not.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file2.txt", Does.Not.Exist);
        Assert.That(_tempToDirectory + "subdir2", Does.Not.Exist);
        Assert.That(_tempToDirectory + "file3.txt", Does.Exist);
        Assert.That(_tempToDirectory + "file4.txt", Does.Exist);
    }

    [SupportedOSPlatform("windows")]
    [Test]
    public void Non_recursive_CopyDirectory_can_copy_to_non_existing_directory(){
        NSKurtsTools.KurtsTools.DeleteDirectory(pathToDirectory: _tempToDirectory);
        Assume.That(_tempToDirectory, Does.Not.Exist);

        NSKurtsTools.KurtsTools.CopyDirectory(_tempFromDirectory, _tempToDirectory, false);

        Assert.That(_tempToDirectory, Does.Exist);
        Assert.That(_tempToDirectory + "subdir1", Does.Not.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file1.txt", Does.Not.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file2.txt", Does.Not.Exist);
        Assert.That(_tempToDirectory + "subdir2", Does.Not.Exist);
        Assert.That(_tempToDirectory + "file3.txt", Does.Exist);
        Assert.That(_tempToDirectory + "file4.txt", Does.Exist);
    }
}