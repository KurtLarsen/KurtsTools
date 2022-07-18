using System.Xml;
using KurtsToolsLibrary.DirectoryTools;
using KurtsToolsLibrary.XmlTools;
using NUnit.Framework;

namespace Testing_KurtsToolsLibrary.Testing_DirectoryTools;

[TestFixture]
public class Testing_CopyDirectory{
    private string _tempFromDirectory = null!;
    private string _tempToDirectory = null!;


    [SetUp]
    public void SetUp(){
        _tempFromDirectory = DirectoryTools.NewTempDirectory();
        _tempToDirectory = DirectoryTools.NewTempDirectory();

        XmlDocument xmlFileStructure = XmlTools.StringToXmlDocument("<root>" +
                                                                    "    <directory name='subdir1'>" +
                                                                    "        <file name='file1.txt' />" +
                                                                    "        <file name='file2.txt' />" +
                                                                    "    </directory>" +
                                                                    "    <directory name='subdir2' />" +
                                                                    "    <file name='file3.txt' />" +
                                                                    "    <file name='file4.txt' />" +
                                                                    "</root>");
        DirectoryTools.BuildDirectoryStructure(_tempFromDirectory, xmlFileStructure.DocumentElement!);
    }


    [TearDown]
    public void TearDown(){
        DirectoryTools.DeleteDirectory(_tempFromDirectory);
        DirectoryTools.DeleteDirectory(_tempToDirectory);
    }

    [Test]
    public void CopyDirectory_can_copy_to_existing_directory(){
        DirectoryTools.CopyDirectory(_tempFromDirectory, _tempToDirectory);

        Assert.That(_tempToDirectory + "subdir1", Does.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file1.txt", Does.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file2.txt", Does.Exist);
        Assert.That(_tempToDirectory + "subdir2", Does.Exist);
        Assert.That(_tempToDirectory + "file3.txt", Does.Exist);
        Assert.That(_tempToDirectory + "file4.txt", Does.Exist);
    }

    [Test]
    public void CopyDirectory_can_copy_to_non_existing_directory(){
        DirectoryTools.DeleteDirectory(_tempToDirectory);
        Assume.That(_tempToDirectory, Does.Not.Exist);

        DirectoryTools.CopyDirectory(_tempFromDirectory, _tempToDirectory);

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
        DirectoryTools.CopyDirectory(_tempFromDirectory, _tempToDirectory, false);

        Assert.That(_tempToDirectory + "subdir1", Does.Not.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file1.txt", Does.Not.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file2.txt", Does.Not.Exist);
        Assert.That(_tempToDirectory + "subdir2", Does.Not.Exist);
        Assert.That(_tempToDirectory + "file3.txt", Does.Exist);
        Assert.That(_tempToDirectory + "file4.txt", Does.Exist);
    }

    [Test]
    public void Non_recursive_CopyDirectory_can_copy_to_non_existing_directory(){
        DirectoryTools.DeleteDirectory(_tempToDirectory);
        Assume.That(_tempToDirectory, Does.Not.Exist);

        DirectoryTools.CopyDirectory(_tempFromDirectory, _tempToDirectory, false);

        Assert.That(_tempToDirectory, Does.Exist);
        Assert.That(_tempToDirectory + "subdir1", Does.Not.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file1.txt", Does.Not.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file2.txt", Does.Not.Exist);
        Assert.That(_tempToDirectory + "subdir2", Does.Not.Exist);
        Assert.That(_tempToDirectory + "file3.txt", Does.Exist);
        Assert.That(_tempToDirectory + "file4.txt", Does.Exist);
    }
}