using System.Xml;
using KurtsToolsLibrary.KurtsFileTools;
using NUnit.Framework;
using KurtsToolsLibrary.XmlTools;

namespace Testing_KurtsToolsLibrary.Testing_File_Tools;

[TestFixture]
public class Testing_CopyDirectory{
    private string _tempFromDirectory = null!;
    private string _tempToDirectory = null!;


    [SetUp]
    public void SetUp(){
        _tempFromDirectory = KurtsFileTools.NewTempDirectory();
        _tempToDirectory = KurtsFileTools.NewTempDirectory();

        XmlDocument xmlFileStructure = XmlTools.StringToXmlDocument("<root>" +
                                                                    "    <directory name='subdir1'>" +
                                                                    "        <file name='file1.txt' />" +
                                                                    "        <file name='file2.txt' />" +
                                                                    "    </directory>" +
                                                                    "    <directory name='subdir2' />" +
                                                                    "    <file name='file3.txt' />" +
                                                                    "    <file name='file4.txt' />" +
                                                                    "</root>");
        KurtsFileTools.BuildDirectoryStructure(_tempFromDirectory, xmlFileStructure.DocumentElement!);
    }


    [TearDown]
    public void TearDown(){
        KurtsFileTools.DeleteDirectory(_tempFromDirectory);
        KurtsFileTools.DeleteDirectory(_tempToDirectory);
    }

    [Test]
    public void CopyDirectory_can_copy_to_existing_directory(){
        KurtsFileTools.CopyDirectory(_tempFromDirectory, _tempToDirectory);

        Assert.That(_tempToDirectory + "subdir1", Does.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file1.txt", Does.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file2.txt", Does.Exist);
        Assert.That(_tempToDirectory + "subdir2", Does.Exist);
        Assert.That(_tempToDirectory + "file3.txt", Does.Exist);
        Assert.That(_tempToDirectory + "file4.txt", Does.Exist);
    }

    [Test]
    public void CopyDirectory_can_copy_to_non_existing_directory(){
        KurtsFileTools.DeleteDirectory(_tempToDirectory);
        Assume.That(_tempToDirectory, Does.Not.Exist);

        KurtsFileTools.CopyDirectory(_tempFromDirectory, _tempToDirectory);

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
        KurtsFileTools.CopyDirectory(_tempFromDirectory, _tempToDirectory, false);

        Assert.That(_tempToDirectory + "subdir1", Does.Not.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file1.txt", Does.Not.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file2.txt", Does.Not.Exist);
        Assert.That(_tempToDirectory + "subdir2", Does.Not.Exist);
        Assert.That(_tempToDirectory + "file3.txt", Does.Exist);
        Assert.That(_tempToDirectory + "file4.txt", Does.Exist);
    }

    [Test]
    public void Non_recursive_CopyDirectory_can_copy_to_non_existing_directory(){
        KurtsFileTools.DeleteDirectory(_tempToDirectory);
        Assume.That(_tempToDirectory, Does.Not.Exist);

        KurtsFileTools.CopyDirectory(_tempFromDirectory, _tempToDirectory, false);

        Assert.That(_tempToDirectory, Does.Exist);
        Assert.That(_tempToDirectory + "subdir1", Does.Not.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file1.txt", Does.Not.Exist);
        Assert.That(_tempToDirectory + "subdir1" + Path.DirectorySeparatorChar + "file2.txt", Does.Not.Exist);
        Assert.That(_tempToDirectory + "subdir2", Does.Not.Exist);
        Assert.That(_tempToDirectory + "file3.txt", Does.Exist);
        Assert.That(_tempToDirectory + "file4.txt", Does.Exist);
    }
}