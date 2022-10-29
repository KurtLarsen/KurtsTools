using System.Runtime.Versioning;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_File_Tools;

[SupportedOSPlatform("windows")]
[TestFixture]
public class Testing_MetaData{
    private string _tempDir = null!;
    private const string DataTimeRegex=@"\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{7}\+\d{2}:\d{2}";    // 2022-07-03T17:09:44.7059571+02:00

    [SupportedOSPlatform("windows")]
    [SetUp]
    public void SetUp(){
        _tempDir = NSKurtsTools.KurtsTools.NewTempDirectory();
    }
    
    [SupportedOSPlatform("windows")]
    [TearDown]
    public void TearDown(){
        NSKurtsTools.KurtsTools.DeleteDirectory(_tempDir);
    }

    [Test]
    public void testing_MetaData_On_copy_of_file(){
        string originalFile = _tempDir + "originalFile.txt";
        File.WriteAllText(originalFile, "abc");

        NSKurtsTools.KurtsTools.MetaData metaDataOriginal =NSKurtsTools.KurtsTools.GetMetaData(originalFile);

        string copyFile = _tempDir + "copyFile.txt";
        File.Copy(originalFile, copyFile);

        NSKurtsTools.KurtsTools.MetaData metaDataCopy = NSKurtsTools.KurtsTools.GetMetaData(copyFile);


        NSKurtsTools.KurtsTools.MetaDataFlags difFlags = NSKurtsTools.KurtsTools.GetMetaDataDifferences(metaDataOriginal, metaDataCopy,
            NSKurtsTools.KurtsTools.MetaDataFlags.All & ~NSKurtsTools.KurtsTools.MetaDataFlags.Path & ~NSKurtsTools.KurtsTools.MetaDataFlags.CreationTime & ~NSKurtsTools.KurtsTools.MetaDataFlags.LastAccessTime);

        Assert.That(difFlags, Is.EqualTo(NSKurtsTools.KurtsTools.MetaDataFlags.None),
            $"{metaDataOriginal.ToString(difFlags)}\n{metaDataCopy.ToString(difFlags)}");
    }

    [SupportedOSPlatform("windows")]
    [Test]
    public void testing_ToString_Attributes(){
        string pathToFile = _tempDir + "file.txt";
        File.WriteAllText(pathToFile, "abc");
        FileInfo fi = new(pathToFile);
        fi.Attributes |= FileAttributes.ReadOnly;

        NSKurtsTools.KurtsTools.MetaData md = NSKurtsTools.KurtsTools.GetMetaData(pathToFile);

        string s = md.ToString(NSKurtsTools.KurtsTools.MetaDataFlags.Attributes);
        Assert.That(s, Is.EqualTo("Attributes={ReadOnly, Archive, NotContentIndexed}"));

    }

    [SupportedOSPlatform("windows")]
    [Test]
    public void testing_ToString_Path(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        NSKurtsTools.KurtsTools.MetaData md = NSKurtsTools.KurtsTools.GetMetaData(path);

        string s = md.ToString(NSKurtsTools.KurtsTools.MetaDataFlags.Path);
        Assert.That(s, Is.EqualTo($"Path=<{path}>"));
    }

    [Test]
    public void testing_ToString_Exists(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        NSKurtsTools.KurtsTools.MetaData md = NSKurtsTools.KurtsTools.GetMetaData(path);

        string s = md.ToString(NSKurtsTools.KurtsTools.MetaDataFlags.Exists);
        Assert.That(s, Is.EqualTo($"Exists=<{true}>"));
    }

    [Test]
    public void testing_ToString_Hash(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        NSKurtsTools.KurtsTools.MetaData md = NSKurtsTools.KurtsTools.GetMetaData(path, true);

        string s = md.ToString(NSKurtsTools.KurtsTools.MetaDataFlags.Hash);
        Assert.That(s, Does.Match(@"Hash=\{\d{1,3}(,\s(\d{1,3}))*\}"));
    }

    [Test]
    public void testing_ToString_Hash_null(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        NSKurtsTools.KurtsTools.MetaData md = NSKurtsTools.KurtsTools.GetMetaData(path);

        string s = md.ToString(NSKurtsTools.KurtsTools.MetaDataFlags.Hash);
        Assert.That(s, Is.EqualTo("Hash=<null>"));
    }

    [Test]
    public void testing_ToString_Length(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        NSKurtsTools.KurtsTools.MetaData md = NSKurtsTools.KurtsTools.GetMetaData(path);

        string s = md.ToString(NSKurtsTools.KurtsTools.MetaDataFlags.Length);
        Assert.That(s, Does.Match(@"Length=<\d+>"));
    }
    
    [Test]
    public void testing_ToString_CreationTime(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        NSKurtsTools.KurtsTools.MetaData md = NSKurtsTools.KurtsTools.GetMetaData(path);

        string s = md.ToString(NSKurtsTools.KurtsTools.MetaDataFlags.CreationTime);
        Assert.That(s, Does.Match("CreationTime=<"+DataTimeRegex+">")); // >
    }

    [Test]
    public void testing_ToString_LastAccessTime(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        NSKurtsTools.KurtsTools.MetaData md = NSKurtsTools.KurtsTools.GetMetaData(path);

        string s = md.ToString(NSKurtsTools.KurtsTools.MetaDataFlags.LastAccessTime);
        Assert.That(s, Does.Match("LastAccessTime=<"+DataTimeRegex+">")); // >
    }

    [Test]
    public void testing_ToString_LastWriteTime(){
        string path = _tempDir + "file.txt";
        File.WriteAllText(path, "abc");
        NSKurtsTools.KurtsTools.MetaData md = NSKurtsTools.KurtsTools.GetMetaData(path);

        string s = md.ToString(NSKurtsTools.KurtsTools.MetaDataFlags.LastWriteTime);
        Assert.That(s, Does.Match("LastWriteTime=<"+DataTimeRegex+">")); // >
    }
    
    [SupportedOSPlatform("windows")]
    [Test]
    public void testing_Hash_is_equal(){
        string file1 = _tempDir + "file1.txt";
        File.WriteAllText(file1, "abc");

        string file2 = _tempDir + "file2.txt";
        File.WriteAllText(file2, "abc");

        NSKurtsTools.KurtsTools.MetaData md1 = NSKurtsTools.KurtsTools.GetMetaData(file1, true);
        NSKurtsTools.KurtsTools.MetaData md2 = NSKurtsTools.KurtsTools.GetMetaData(file2, true);

        Assume.That(md1.Hash, Is.Not.Null);
        Assume.That(md2.Hash, Is.Not.Null);

        Assert.That(md1.Hash, Is.EquivalentTo(md2.Hash));
    }

    [SupportedOSPlatform("windows")]
    [Test]
    public void testing_Hash_is_null(){
        string file = _tempDir + "file1.txt";
        File.WriteAllText(file, "abc");

        NSKurtsTools.KurtsTools.MetaData mdWithFileContent = NSKurtsTools.KurtsTools.GetMetaData(file, true);

        Assert.That(mdWithFileContent.Hash, Is.Not.Null);

        NSKurtsTools.KurtsTools.MetaData md2WithoutFileContent = NSKurtsTools.KurtsTools.GetMetaData(file);

        Assert.That(md2WithoutFileContent.Hash, Is.Null);
    }
}