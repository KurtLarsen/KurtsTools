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

        MetaData metaDataOriginal = new(originalFile);

        string copyFile = _tempDir + "copyFile.txt";
        File.Copy(originalFile, copyFile);

        MetaData metaDataCopy = new(copyFile);


        MetaDataFilter difFlags = FileTools.GetMetaDataDifFlags(metaDataOriginal, metaDataCopy,
            MetaDataFilter.All & ~MetaDataFilter.Path & ~MetaDataFilter.CreationTime & ~MetaDataFilter.LastAccessTime);

        Assert.That(difFlags, Is.EqualTo(MetaDataFilter.None),
            $"{metaDataOriginal.ToString(difFlags)}\n{metaDataCopy.ToString(difFlags)}");
    }

    [Test]
    public void testing_ToString_Attributes(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        FileInfo fi = new(path);
        fi.Attributes |= FileAttributes.ReadOnly;

        MetaData md = new(path);

        string s = md.ToString(MetaDataFilter.Attributes);
        Assert.That(s, Is.EqualTo("Attributes={ReadOnly, Archive}"));

    }

    [Test]
    public void testing_ToString_Path(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        MetaData md = new(path);

        string s = md.ToString(MetaDataFilter.Path);
        Assert.That(s, Is.EqualTo($"Path=<{path}>"));
    }

    [Test]
    public void testing_ToString_Exists(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        MetaData md = new(path);

        string s = md.ToString(MetaDataFilter.Exists);
        Assert.That(s, Is.EqualTo($"Exists=<{true}>"));
    }

    [Test]
    public void testing_ToString_Hash(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        MetaData md = new(path, true);

        string s = md.ToString(MetaDataFilter.Hash);
        Assert.That(s, Does.Match(@"Hash=\{\d{1,3}(,\s(\d{1,3}))*\}"));
    }

    [Test]
    public void testing_ToString_Hash_null(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        MetaData md = new(path);

        string s = md.ToString(MetaDataFilter.Hash);
        Assert.That(s, Is.EqualTo("Hash=<null>"));
    }

    [Test]
    public void testing_ToString_Length(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        MetaData md = new(path);

        string s = md.ToString(MetaDataFilter.Length);
        Assert.That(s, Does.Match(@"Length=<\d+>"));
    }
    
    [Test]
    public void testing_ToString_CreationTime(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        MetaData md = new(path);

        string s = md.ToString(MetaDataFilter.CreationTime);
        Assert.That(s, Does.Match("CreationTime=<"+DataTimeRegex+">")); // >
    }

    [Test]
    public void testing_ToString_LastAccessTime(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        MetaData md = new(path);

        string s = md.ToString(MetaDataFilter.LastAccessTime);
        Assert.That(s, Does.Match("LastAccessTime=<"+DataTimeRegex+">")); // >
    }

    [Test]
    public void testing_ToString_LastWriteTime(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        MetaData md = new(path);

        string s = md.ToString(MetaDataFilter.LastWriteTime);
        Assert.That(s, Does.Match("LastWriteTime=<"+DataTimeRegex+">")); // >
    }
    
    [Test]
    public void testing_Hash_is_equal(){
        string file1 = _tempDir + "file1.txt";
        File.WriteAllText(file1, "abc");

        string file2 = _tempDir + "file2.txt";
        File.WriteAllText(file2, "abc");

        MetaData md1 = new(file1, true);
        MetaData md2 = new(file2, true);

        Assume.That(md1.Hash, Is.Not.Null);
        Assume.That(md2.Hash, Is.Not.Null);

        Assert.That(md1.Hash, Is.EquivalentTo(md2.Hash));
    }

    [Test]
    public void testing_Hash_is_null(){
        string file = _tempDir + "file1.txt";
        File.WriteAllText(file, "abc");

        MetaData mdWithFileContent = new(file, true);

        Assert.That(mdWithFileContent.Hash, Is.Not.Null);

        MetaData md2WithoutFileContent = new(file);

        Assert.That(md2WithoutFileContent.Hash, Is.Null);
    }
}