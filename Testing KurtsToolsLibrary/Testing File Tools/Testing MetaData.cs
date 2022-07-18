using KurtsToolsLibrary.FileTools;
using NUnit.Framework;

namespace Testing_KurtsToolsLibrary.Testing_File_Tools;

[TestFixture]
public class Testing_MetaData{
    private string _tempDir = null!;
    private const string DataTimeRegex=@"\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{7}\+\d{2}:\d{2}";    // 2022-07-03T17:09:44.7059571+02:00

    [SetUp]
    public void SetUp(){
        _tempDir = FileTools.NewTempDirectory();
    }

    [TearDown]
    public void TearDown(){
        FileTools.DeleteDirectory(_tempDir);
    }

    [Test]
    public void testing_MetaData_On_copy_of_file(){
        string originalFile = _tempDir + "originalFile.txt";
        File.WriteAllText(originalFile, "abc");

        FileTools.MetaData metaDataOriginal =FileTools.GetMetaData(originalFile);

        string copyFile = _tempDir + "copyFile.txt";
        File.Copy(originalFile, copyFile);

        FileTools.MetaData metaDataCopy = FileTools.GetMetaData(copyFile);


        FileTools.MetaDataFilter difFlags = FileTools.GetMetaDataDifferences(metaDataOriginal, metaDataCopy,
            FileTools.MetaDataFilter.All & ~FileTools.MetaDataFilter.Path & ~FileTools.MetaDataFilter.CreationTime & ~FileTools.MetaDataFilter.LastAccessTime);

        Assert.That(difFlags, Is.EqualTo(FileTools.MetaDataFilter.None),
            $"{metaDataOriginal.ToString(difFlags)}\n{metaDataCopy.ToString(difFlags)}");
    }

    [Test]
    public void testing_ToString_Attributes(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        FileInfo fi = new(path);
        fi.Attributes |= FileAttributes.ReadOnly;

        FileTools.MetaData md = FileTools.GetMetaData(path);

        string s = md.ToString(FileTools.MetaDataFilter.Attributes);
        Assert.That(s, Is.EqualTo("Attributes={ReadOnly, Archive}"));

    }

    [Test]
    public void testing_ToString_Path(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        FileTools.MetaData md = FileTools.GetMetaData(path);

        string s = md.ToString(FileTools.MetaDataFilter.Path);
        Assert.That(s, Is.EqualTo($"Path=<{path}>"));
    }

    [Test]
    public void testing_ToString_Exists(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        FileTools.MetaData md = FileTools.GetMetaData(path);

        string s = md.ToString(FileTools.MetaDataFilter.Exists);
        Assert.That(s, Is.EqualTo($"Exists=<{true}>"));
    }

    [Test]
    public void testing_ToString_Hash(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        FileTools.MetaData md = FileTools.GetMetaData(path, true);

        string s = md.ToString(FileTools.MetaDataFilter.Hash);
        Assert.That(s, Does.Match(@"Hash=\{\d{1,3}(,\s(\d{1,3}))*\}"));
    }

    [Test]
    public void testing_ToString_Hash_null(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        FileTools.MetaData md = FileTools.GetMetaData(path);

        string s = md.ToString(FileTools.MetaDataFilter.Hash);
        Assert.That(s, Is.EqualTo("Hash=<null>"));
    }

    [Test]
    public void testing_ToString_Length(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        FileTools.MetaData md = FileTools.GetMetaData(path);

        string s = md.ToString(FileTools.MetaDataFilter.Length);
        Assert.That(s, Does.Match(@"Length=<\d+>"));
    }
    
    [Test]
    public void testing_ToString_CreationTime(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        FileTools.MetaData md = FileTools.GetMetaData(path);

        string s = md.ToString(FileTools.MetaDataFilter.CreationTime);
        Assert.That(s, Does.Match("CreationTime=<"+DataTimeRegex+">")); // >
    }

    [Test]
    public void testing_ToString_LastAccessTime(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        FileTools.MetaData md = FileTools.GetMetaData(path);

        string s = md.ToString(FileTools.MetaDataFilter.LastAccessTime);
        Assert.That(s, Does.Match("LastAccessTime=<"+DataTimeRegex+">")); // >
    }

    [Test]
    public void testing_ToString_LastWriteTime(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        FileTools.MetaData md = FileTools.GetMetaData(path);

        string s = md.ToString(FileTools.MetaDataFilter.LastWriteTime);
        Assert.That(s, Does.Match("LastWriteTime=<"+DataTimeRegex+">")); // >
    }
    
    [Test]
    public void testing_Hash_is_equal(){
        string file1 = _tempDir + "file1.txt";
        File.WriteAllText(file1, "abc");

        string file2 = _tempDir + "file2.txt";
        File.WriteAllText(file2, "abc");

        FileTools.MetaData md1 = FileTools.GetMetaData(file1, true);
        FileTools.MetaData md2 = FileTools.GetMetaData(file2, true);

        Assume.That(md1.Hash, Is.Not.Null);
        Assume.That(md2.Hash, Is.Not.Null);

        Assert.That(md1.Hash, Is.EquivalentTo(md2.Hash));
    }

    [Test]
    public void testing_Hash_is_null(){
        string file = _tempDir + "file1.txt";
        File.WriteAllText(file, "abc");

        FileTools.MetaData mdWithFileContent = FileTools.GetMetaData(file, true);

        Assert.That(mdWithFileContent.Hash, Is.Not.Null);

        FileTools.MetaData md2WithoutFileContent = FileTools.GetMetaData(file);

        Assert.That(md2WithoutFileContent.Hash, Is.Null);
    }
}