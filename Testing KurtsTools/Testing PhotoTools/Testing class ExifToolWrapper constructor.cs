using System.Runtime.Versioning;
using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_PhotoTools;

[TestFixture,SupportedOSPlatform("windows")]
public class Testing_Class_ExifToolWrapper_Constructor{
    private const  string ValidPathToExifToolExe=@"Testing PhotoTools\TestData\applicarions\exiftool_v12.49.exe";
    private const string InvalidPathToExifToolExe=@"Testing PhotoTools\TestData\applicarions\notExistingFile";

    [OneTimeSetUp]
    public static void OneTimeSetUp(){
        Assume.That(ValidPathToExifToolExe,Does.Exist);
        Assume.That(InvalidPathToExifToolExe,Does.Not.Exist);
    }
    
    [Test]
    public void testing_with_valid_path_to_ExifToolExe(){
        try{
            ExifToolWrapper unused = new(ValidPathToExifToolExe);
        } catch (Exception e){
            Assert.That(false,
                $"Error constructing new instance of {nameof(NSKurtsTools)}.{nameof(ExifToolWrapper)}:\n{e}");
        }
        Assert.Pass();
    }

    [Test]
    public void testing_with_invalid_path_to_ExifToolExe(){
        Assert.Throws<FileNotFoundException>(delegate{
            ExifToolWrapper unused = new(InvalidPathToExifToolExe);
        });
    }
}