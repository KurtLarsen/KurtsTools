using System.Runtime.Versioning;
using System.Xml;
using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_PhotoTools;

[TestFixture]
public class Testing_Class_ExitToolWrapper{
    private const string PathToExifToolWrapper = @"C:\Program Files\exiftool\exiftool.exe";
    private ExifToolWrapper _instanceOfExifToolWrapper = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp(){
        try{
            _instanceOfExifToolWrapper = new ExifToolWrapper(PathToExifToolWrapper);
        } catch (Exception e){
            Assume.That(false,
                $"Error constructing new instance of {nameof(NSKurtsTools)}.{nameof(ExifToolWrapper)}:\n{e}");
        }
    }

    // [Test]
    // public void Test_zero_files_given(){
    //     Exception? e = Assert.Throws<Exception>(() => _instanceOfExifToolWrapper.SetFiles());
    //     Assert.That(e!.Message, Is.EqualTo("Zero files"));
    // }

    // [Test]
    // public void Test_property_Files_with_one_file(){
    //     const string photoFile = @"Testing PhotoTools\TestData\jpg files\7P2A1833.jpg";
    //     Assume.That(photoFile, Does.Exist);
    //     _instanceOfExifToolWrapper.SetFiles(photoFile);
    //     string[] expectedFiles = new[]{ Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + photoFile };
    //     Assert.That(_instanceOfExifToolWrapper.Files, Is.EquivalentTo(expectedFiles));
    // }

    // [Test]
    // public void Test_property_Files_with_two_files(){
    //     const string photoFile1 = @"Testing PhotoTools\TestData\jpg files\7P2A1833.jpg";
    //     const string photoFile2 = @"Testing PhotoTools\TestData\jpg files\7P2A1834.jpg";
    //     Assume.That(photoFile1, Does.Exist);
    //     Assume.That(photoFile2, Does.Exist);
    //     _instanceOfExifToolWrapper.SetFiles(photoFile1, photoFile2);
    //     string abs = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar;
    //     string[] expectedFiles ={
    //         abs + photoFile1,
    //         abs + photoFile2,
    //     };
    //     Assert.That(_instanceOfExifToolWrapper.Files, Is.EquivalentTo(expectedFiles));
    // }

    [SupportedOSPlatform("windows")]
    [Test]
    public void Test_function_GetExif(){
        const string photoFile1 = @"Testing PhotoTools\TestData\jpg files\7P2A1833.jpg";
        const string photoFile2 = @"Testing PhotoTools\TestData\jpg files\7P2A1834.jpg";
        Assume.That(photoFile1, Does.Exist);
        Assume.That(photoFile2, Does.Exist);

        XmlDocument
            result = _instanceOfExifToolWrapper.GetExif(photoFile1, photoFile2); // todo: .KeyEquals(null,"make");
        Assert.That(result, Is.Not.Null);

        XmlElement? root = result.DocumentElement;
        Assume.That(root, Is.Not.Null);

        XmlNodeList? x1 = root!.SelectNodes("//*[name()='File:FileType']");
        var x2 = root.SelectNodes("//*[local-name()='FileType']");
        var x3 = root.SelectNodes("//*[contains(local-name(),'Time')]");
        var x5 = root.SelectNodes("//*[contains(translate(local-name(),'TIME','time'),'time')]");
    }
}