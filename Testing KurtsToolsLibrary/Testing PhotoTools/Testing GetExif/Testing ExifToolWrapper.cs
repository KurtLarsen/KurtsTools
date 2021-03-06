using System.Xml;
using KurtsToolsLibrary.PhotoTools;
using NUnit.Framework;

namespace Testing_KurtsToolsLibrary.Testing_PhotoTools.Testing_GetExif;

[TestFixture]
public class Testing_GetExif{
    private const string ExifToolExe = @"C:\Program Files\exiftool\exiftool.exe";
    private static string _pathToTestDataForTestingExifWrapper = null!;

    [OneTimeSetUp]
    public void OneTimeSetup(){
        _pathToTestDataForTestingExifWrapper = TestContext.CurrentContext.TestDirectory +
                      @"/Testing PhotoTools/Testing GetExif/TestData for Testing GetExif";
        
        Assume.That(Directory.Exists(_pathToTestDataForTestingExifWrapper));
    }

    [Test]
    public void testing_exePath_is_null(){
        ExifArgument p = new(){ PathToExifToolExe = null };
        
        ExifException? e = Assert.Throws<ExifException>(
            delegate{
                PhotoTools.GetExif(p);
            });
        
        Assert.That(e!.Id,Is.EqualTo(ExifExceptionId.PathToExifToolExeIsNull));
    }

    [Test]
    public void testing_exePath_does_not_exist(){
        const string notExistingFile = @"c:\not_existing_file.exe";
        Assume.That(notExistingFile,Does.Not.Exist);
        
        ExifArgument p = new(){ PathToExifToolExe = notExistingFile };
        
        ExifException? e = Assert.Throws<ExifException>(
            delegate{
                PhotoTools.GetExif(p);
            });
        
        Assert.That(e!.Id,Is.EqualTo(ExifExceptionId.PathToExifToolExeNotFound));
        
        Assert.That(e.Message,Is.EqualTo(notExistingFile));
    }

    [Test]
    public void testing_files_is_null(){
        ExifArgument p = new(){ PathToExifToolExe = ExifToolExe, Files = null };

        ExifException? e = Assert.Throws<ExifException>(
            delegate{
                PhotoTools.GetExif(p);
            });
        
        Assert.That(e!.Id,Is.EqualTo(ExifExceptionId.FileArrayIsNull));
    }

    [Test]
    public void testing_files_is_empty(){
        ExifArgument p = new(){ PathToExifToolExe = ExifToolExe, Files = Array.Empty<string>() };

        ExifException? e = Assert.Throws<ExifException>(
            delegate{
                PhotoTools.GetExif(p);
            });
        
        Assert.That(e!.Id,Is.EqualTo(ExifExceptionId.FileArrayIsEmpty));
    }

    [Test]
    public void testing_on_cr3(){
        string file = _pathToTestDataForTestingExifWrapper + @"/cr3 files/7P2A1833.CR3";
        
        Assume.That(file, Does.Exist);
        
        ExifArgument p = new(){ PathToExifToolExe = ExifToolExe, Files = new[]{ file } };
        
        (XmlDocument xmlDocument, string? messageFromExitTool)  = PhotoTools.GetExif(p);
        
        Assert.That(messageFromExitTool, Is.Null, $"{nameof(messageFromExitTool)} after running {nameof(PhotoTools)}.{nameof(PhotoTools.GetExif)}()");

        XmlElement? root = xmlDocument.DocumentElement;
        
        Assert.That(root,Is.Not.Null,$"DocumentElement in {nameof(xmlDocument)} after running {nameof(PhotoTools)}.{nameof(PhotoTools.GetExif)}()");
    }

    [Test]
    public void testing_on_non_image_file(){
        string file = _pathToTestDataForTestingExifWrapper + @"\NonImageFile.txt";
        
        Assume.That(file, Does.Exist);
        
        ExifArgument p = new(){ PathToExifToolExe = ExifToolExe, Files = new[]{ file } };
        
        (XmlDocument xmlDocument, string? messageFromExitTool)  = PhotoTools.GetExif(p);

        Assert.That(messageFromExitTool, Is.Null, $"{nameof(messageFromExitTool)} after running {nameof(PhotoTools)}.{nameof(PhotoTools.GetExif)}()");

        XmlElement? root = xmlDocument.DocumentElement;
        
        Assert.That(root,Is.Not.Null,$"DocumentElement in {nameof(xmlDocument)} after running {nameof(PhotoTools)}.{nameof(PhotoTools.GetExif)}()");
    }

    [Test]
    public void testing_on_non_existing_file(){
        string file = TestContext.CurrentContext.TestDirectory +
                      @"\Testing File Tools\TestData for Testing ExifToolWrapper\nonExistingFile.jpg";
        Assume.That(file, Does.Not.Exist);
        ExifArgument p = new(){ PathToExifToolExe = ExifToolExe, Files = new[]{ file } };
        ExifException? e = Assert.Throws<ExifException>(
            delegate{
                PhotoTools.GetExif(p);
            });
        
        Assert.That(e!.Id,Is.EqualTo(ExifExceptionId.ExifToolDidNotReturnAnyOutput));
    }

    [Test]
    public void testing_on_directory(){
        string file = _pathToTestDataForTestingExifWrapper;
        
        Assume.That(file, Does.Exist);
        ExifArgument p = new(){ PathToExifToolExe = ExifToolExe, Files = new[]{ file } };
        (XmlDocument xmlDocument, string? messageFromExitTool) = PhotoTools.GetExif(p);

        Assert.That(messageFromExitTool, Is.EqualTo("1 directories scanned 16 image files read"),
            "result.MsgFormExifTool");

        XmlElement? root = xmlDocument.DocumentElement;
        
        Assert.That(root,Is.Not.Null,$"DocumentElement in {nameof(xmlDocument)} after running {nameof(PhotoTools)}.{nameof(PhotoTools.GetExif)}()");
    }

    [Test]
    public void testing_on_directory_with_backslash(){
        string file = _pathToTestDataForTestingExifWrapper + @"/";
        
        Assume.That(file, Does.Exist);
        ExifArgument p = new(){ PathToExifToolExe = ExifToolExe, Files = new[]{ file } };
        (XmlDocument xmlDocument, string? messageFromExitTool) = PhotoTools.GetExif(p);

        Assert.That(messageFromExitTool, Is.EqualTo("1 directories scanned 16 image files read"),
            "result.MsgFormExifTool");

        XmlElement? root = xmlDocument.DocumentElement;
        
        Assert.That(root,Is.Not.Null,$"DocumentElement in {nameof(xmlDocument)} after running {nameof(PhotoTools)}.{nameof(PhotoTools.GetExif)}()");
    }

    [Test]
    public void testing_on_directory_recursive(){
        string file = _pathToTestDataForTestingExifWrapper;
        
        Assume.That(file, Does.Exist);
        ExifArgument p = new(){ PathToExifToolExe = ExifToolExe, Files = new[]{ file }, Options = "-r" };

        (XmlDocument xmlDocument, string? messageFromExitTool) = PhotoTools.GetExif(p);

        Assert.That(messageFromExitTool, Is.EqualTo("3 directories scanned 29 image files read"),
            "result.MsgFormExifTool");

        XmlElement? root = xmlDocument.DocumentElement;
        
        Assert.That(root,Is.Not.Null,$"DocumentElement in {nameof(xmlDocument)} after running {nameof(PhotoTools)}.{nameof(PhotoTools.GetExif)}()");

    }

    [Test]
    public void testing_on_directory_with_backslash_recursive(){
        string file = _pathToTestDataForTestingExifWrapper + @"/";
        
        Assume.That(file, Does.Exist);
        ExifArgument p = new(){ PathToExifToolExe = ExifToolExe, Files = new[]{ file }, Options = "-r" };
        (XmlDocument xmlDocument, string? messageFromExitTool) = PhotoTools.GetExif(p);

        Assert.That(messageFromExitTool, Is.EqualTo("3 directories scanned 29 image files read"),
            "result.MsgFormExifTool");

        XmlElement? root = xmlDocument.DocumentElement;
        
        Assert.That(root,Is.Not.Null,$"DocumentElement in {nameof(xmlDocument)} after running {nameof(PhotoTools)}.{nameof(PhotoTools.GetExif)}()");
    }
}