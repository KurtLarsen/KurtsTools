using System.Runtime.Versioning;
using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_PhotoTools;

[TestFixture,SupportedOSPlatform("windows")]
public class Testing_Class_ExifToolWrapper_Constructor{
    private static IEnumerable<TestCaseData> ExifToolVersion(){
        const string pathTpExifToolExeDirectory = @"Testing PhotoTools\exiftool.exe";
        Assume.That(pathTpExifToolExeDirectory, Does.Exist, $"Directory not found: {pathTpExifToolExeDirectory}");
        DirectoryInfo directory = new(pathTpExifToolExeDirectory);
        FileInfo[] exiftoolExeFiles = directory.GetFiles("exiftool_v??.??.exe");
        foreach (FileInfo exiftoolExeFile in exiftoolExeFiles){
            yield return new TestCaseData(exiftoolExeFile.FullName).SetName(exiftoolExeFile.Name.Substring(10, 5));
        }
    }

    [Test, TestCaseSource(nameof(ExifToolVersion))]
    public void testing_with_valid_path_to_ExifToolExe(string pathToExifTool){
        try{
            ExifToolWrapper unused = new(pathToExifTool);
        } catch (Exception e){
            Assert.That(false,
                $"Error constructing new instance of {nameof(NSKurtsTools)}.{nameof(ExifToolWrapper)}:\n{e}");
        }
        Assert.Pass();
    }

    [Test]
    public void testing_with_invalid_path_to_ExifToolExe(){
        const string invalidPathToExifToolExe = "xxx";
        Assume.That(invalidPathToExifToolExe, Does.Not.Exist);

        Assert.Throws<FileNotFoundException>(delegate{
            ExifToolWrapper unused = new(invalidPathToExifToolExe);
        });
    }
}