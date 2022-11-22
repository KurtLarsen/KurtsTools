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
    public void without_placeholder(){

        string maskWithoutPlaceholder = _tempDirectory + "abc.txt";
        
        string newFileName= KurtsTools.UniqueFileName(maskWithoutPlaceholder);
       
        Assert.That(newFileName,Does.EndWith(@"\abc.txt"));
        
        File.WriteAllText(newFileName,"dummy content");

        newFileName= KurtsTools.UniqueFileName(maskWithoutPlaceholder);
       
        Assert.That(newFileName,Does.EndWith(@"\abc.txt2"));
        
        File.WriteAllText(newFileName,"dummy content");
        
        newFileName= KurtsTools.UniqueFileName(maskWithoutPlaceholder);
       
        Assert.That(newFileName,Does.EndWith(@"\abc.txt3"));

    }

    [Test]
    public void with_placeholder(){

        string maskWithPlaceholder = _tempDirectory + "abc{#}.txt";
        
        string newFileName= KurtsTools.UniqueFileName(maskWithPlaceholder);
       
        Assert.That(newFileName,Does.EndWith(@"\abc.txt"));
        
        File.WriteAllText(newFileName,"dummy content");

        newFileName= KurtsTools.UniqueFileName(maskWithPlaceholder);
       
        Assert.That(newFileName,Does.EndWith(@"\abc2.txt"));
        
        File.WriteAllText(newFileName,"dummy content");
        
        newFileName= KurtsTools.UniqueFileName(maskWithPlaceholder);
       
        Assert.That(newFileName,Does.EndWith(@"\abc3.txt"));

    }


}