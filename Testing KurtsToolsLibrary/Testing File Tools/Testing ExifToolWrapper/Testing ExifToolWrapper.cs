using System.Xml;
using KurtsToolsLibrary.KurtsFileTools;
using NUnit.Framework;

namespace Testing_KurtsToolsLibrary.Testing_File_Tools.Testing_ExifToolWrapper;

[TestFixture]
public class Testing_ExifToolWrapper{
    private const string ExifToolExe = @"C:\Program Files\exiftool\exiftool.exe";

    [Test]
    public void testing_exePath_is_null(){
        ExifToolWrapperParameters p = new(){ PathToExifToolExe = null };
        
        ExifToolWrapperException? e = Assert.Throws<ExifToolWrapperException>(
            delegate{
                KurtsFileTools.ExifToolWrapper(p);
            });
        
        Assert.That(e!.ExceptionId,Is.EqualTo(ExifToolExceptionId.PathToExifToolExeIsNull));
    }

    [Test]
    public void testing_exePath_does_not_exist(){
        const string notExistingFile = @"c:\not_existing_file.exe";
        Assume.That(notExistingFile,Does.Not.Exist);
        
        ExifToolWrapperParameters p = new(){ PathToExifToolExe = notExistingFile };
        
        ExifToolWrapperException? e = Assert.Throws<ExifToolWrapperException>(
            delegate{
                KurtsFileTools.ExifToolWrapper(p);
            });
        
        Assert.That(e!.ExceptionId,Is.EqualTo(ExifToolExceptionId.PathToExifToolExeNotFound));
        
        Assert.That(e.Message,Is.EqualTo(notExistingFile));
    }

    [Test]
    public void testing_files_is_null(){
        ExifToolWrapperParameters p = new(){ PathToExifToolExe = ExifToolExe, Files = null };

        ExifToolWrapperException? e = Assert.Throws<ExifToolWrapperException>(
            delegate{
                KurtsFileTools.ExifToolWrapper(p);
            });
        
        Assert.That(e!.ExceptionId,Is.EqualTo(ExifToolExceptionId.FileArrayIsNull));
    }

    [Test]
    public void testing_files_is_empty(){
        ExifToolWrapperParameters p = new(){ PathToExifToolExe = ExifToolExe, Files = Array.Empty<string>() };

        ExifToolWrapperException? e = Assert.Throws<ExifToolWrapperException>(
            delegate{
                KurtsFileTools.ExifToolWrapper(p);
            });
        
        Assert.That(e!.ExceptionId,Is.EqualTo(ExifToolExceptionId.FileArrayIsEmpty));
    }

    [Test]
    public void testing_on_cr3(){
        string file = TestContext.CurrentContext.TestDirectory +
                      @"\Testing File Tools\Testing ExifToolWrapper\TestData for Testing ExifToolWrapper\cr3 files\7P2A1833.CR3";
        
        Assume.That(file, Does.Exist);
        
        ExifToolWrapperParameters p = new(){ PathToExifToolExe = ExifToolExe, Files = new[]{ file } };
        
        (XmlDocument xmlDocument, string? messageFromExitTool)  = KurtsFileTools.ExifToolWrapper(p);
        
        Assert.That(messageFromExitTool, Is.Null, $"{nameof(messageFromExitTool)} after running {nameof(KurtsFileTools)}.{nameof(KurtsFileTools.ExifToolWrapper)}()");

        XmlElement? root = xmlDocument.DocumentElement;
        
        Assert.That(root,Is.Not.Null,$"DocumentElement in {nameof(xmlDocument)} after running {nameof(KurtsFileTools)}.{nameof(KurtsFileTools.ExifToolWrapper)}()");
    }

    [Test]
    public void testing_on_non_image_file(){
        string file = TestContext.CurrentContext.TestDirectory +
                      @"\Testing File Tools\Testing ExifToolWrapper\TestData for Testing ExifToolWrapper\NonImageFile.txt";
        
        Assume.That(file, Does.Exist);
        
        ExifToolWrapperParameters p = new(){ PathToExifToolExe = ExifToolExe, Files = new[]{ file } };
        
        (XmlDocument xmlDocument, string? messageFromExitTool)  = KurtsFileTools.ExifToolWrapper(p);

        Assert.That(messageFromExitTool, Is.Null, $"{nameof(messageFromExitTool)} after running {nameof(KurtsFileTools)}.{nameof(KurtsFileTools.ExifToolWrapper)}()");

        XmlElement? root = xmlDocument.DocumentElement;
        
        Assert.That(root,Is.Not.Null,$"DocumentElement in {nameof(xmlDocument)} after running {nameof(KurtsFileTools)}.{nameof(KurtsFileTools.ExifToolWrapper)}()");
    }

    [Test]
    public void testing_on_non_existing_file(){
        string file = TestContext.CurrentContext.TestDirectory +
                      @"\Testing File Tools\TestData for Testing ExifToolWrapper\nonExistingFile.jpg";
        Assume.That(file, Does.Not.Exist);
        ExifToolWrapperParameters p = new(){ PathToExifToolExe = ExifToolExe, Files = new[]{ file } };
        ExifToolWrapperException? e = Assert.Throws<ExifToolWrapperException>(
            delegate{
                KurtsFileTools.ExifToolWrapper(p);
            });
        
        Assert.That(e!.ExceptionId,Is.EqualTo(ExifToolExceptionId.ExifToolDidNotReturnAnyOutput));
    }

    [Test]
    public void testing_on_directory(){
        string file = TestContext.CurrentContext.TestDirectory +
                      @"\Testing File Tools\Testing ExifToolWrapper\TestData for Testing ExifToolWrapper";
        Assume.That(file, Does.Exist);
        ExifToolWrapperParameters p = new(){ PathToExifToolExe = ExifToolExe, Files = new[]{ file } };
        (XmlDocument xmlDocument, string? messageFromExitTool) = KurtsFileTools.ExifToolWrapper(p);

        Assert.That(messageFromExitTool, Is.EqualTo("1 directories scanned 16 image files read"),
            "result.MsgFormExifTool");

        XmlElement? root = xmlDocument.DocumentElement;
        
        Assert.That(root,Is.Not.Null,$"DocumentElement in {nameof(xmlDocument)} after running {nameof(KurtsFileTools)}.{nameof(KurtsFileTools.ExifToolWrapper)}()");
    }

    [Test]
    public void testing_on_directory_with_backslash(){
        string file = TestContext.CurrentContext.TestDirectory +
                      @"\Testing File Tools\Testing ExifToolWrapper\TestData for Testing ExifToolWrapper\";
        Assume.That(file, Does.Exist);
        ExifToolWrapperParameters p = new(){ PathToExifToolExe = ExifToolExe, Files = new[]{ file } };
        (XmlDocument xmlDocument, string? messageFromExitTool) = KurtsFileTools.ExifToolWrapper(p);

        Assert.That(messageFromExitTool, Is.EqualTo("1 directories scanned 16 image files read"),
            "result.MsgFormExifTool");

        XmlElement? root = xmlDocument.DocumentElement;
        
        Assert.That(root,Is.Not.Null,$"DocumentElement in {nameof(xmlDocument)} after running {nameof(KurtsFileTools)}.{nameof(KurtsFileTools.ExifToolWrapper)}()");
    }

    [Test]
    public void testing_on_directory_recursive(){
        string file = TestContext.CurrentContext.TestDirectory +
                      @"\Testing File Tools\Testing ExifToolWrapper\TestData for Testing ExifToolWrapper";
        Assume.That(file, Does.Exist);
        ExifToolWrapperParameters p = new(){ PathToExifToolExe = ExifToolExe, Files = new[]{ file }, Options = "-r" };

        (XmlDocument xmlDocument, string? messageFromExitTool) = KurtsFileTools.ExifToolWrapper(p);

        Assert.That(messageFromExitTool, Is.EqualTo("3 directories scanned 29 image files read"),
            "result.MsgFormExifTool");

        XmlElement? root = xmlDocument.DocumentElement;
        
        Assert.That(root,Is.Not.Null,$"DocumentElement in {nameof(xmlDocument)} after running {nameof(KurtsFileTools)}.{nameof(KurtsFileTools.ExifToolWrapper)}()");

    }

    [Test]
    public void testing_on_directory_with_backslash_recursive(){
        string file = TestContext.CurrentContext.TestDirectory +
                      @"\Testing File Tools\Testing ExifToolWrapper\TestData for Testing ExifToolWrapper\";
        Assume.That(file, Does.Exist);
        ExifToolWrapperParameters p = new(){ PathToExifToolExe = ExifToolExe, Files = new[]{ file }, Options = "-r" };
        (XmlDocument xmlDocument, string? messageFromExitTool) = KurtsFileTools.ExifToolWrapper(p);

        Assert.That(messageFromExitTool, Is.EqualTo("3 directories scanned 29 image files read"),
            "result.MsgFormExifTool");

        XmlElement? root = xmlDocument.DocumentElement;
        
        Assert.That(root,Is.Not.Null,$"DocumentElement in {nameof(xmlDocument)} after running {nameof(KurtsFileTools)}.{nameof(KurtsFileTools.ExifToolWrapper)}()");
    }
}