using System.Runtime.Versioning;
using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_File_Tools;

[TestFixture,SupportedOSPlatform("windows")]
public class TestingFnUniqueFileName{
    private string _tempDirectory=null!;

    [SetUp]
    public void SetUp(){
       _tempDirectory= KurtsTools.NewTempDirectory();
    }

    [TearDown]
    public void TearDown(){
        KurtsTools.DeleteDirectory(pathToDirectory: _tempDirectory);
    }

    [Test]
    public void without_numbering_and_without_startIndex(){
        string existingFileName = _tempDirectory + "abc.txt";
        File.WriteAllText(existingFileName,"");

       string result= KurtsTools.UniqueFileName(existingFileName);
       
       Assert.That(result,Does.EndWith("abc2.txt"));

    }

    [Test]
    public void with_numbering_and_without_startIndex(){
        string existingFileName = _tempDirectory + "ab{0}c.txt";
        File.WriteAllText(existingFileName,"");

        string result= KurtsTools.UniqueFileName(existingFileName);
       
        Assert.That(result,Does.EndWith("ab2c.txt"));

    }


}