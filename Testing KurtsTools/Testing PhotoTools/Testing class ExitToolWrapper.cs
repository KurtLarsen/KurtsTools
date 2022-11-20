using System.Runtime.Versioning;
using NSKurtsTools;
using NUnit.Framework;


namespace NSTesting_KurtsTools.Testing_PhotoTools;

[TestFixture, SupportedOSPlatform("windows")]
public class Testing_Class_ExitToolWrapper{
    private const string PathToExifTool = @"Testing PhotoTools\TestData\applicarions\exiftool_v12.49.exe";
    private const string TestFilesRoot = @"Testing PhotoTools\TestData\files\";
    private static ExifToolWrapper _instanceOfExifToolWrapper = null!;

    // private static string? _aEmptyDirectoryCreatedDuringCreatingTestCaseData;

    [OneTimeSetUp]
    public static void  OneTimeSetup(){
        Assume.That(PathToExifTool, Does.Exist);

        try{
            _instanceOfExifToolWrapper = new ExifToolWrapper(PathToExifTool);
        } catch (Exception e){
            Assume.That(false,
                $"An exception was thrown in {nameof(Testing_Class_ExitToolWrapper)}.{nameof(OneTimeSetup)}() when creating instance of class {nameof(PathToExifTool)}:\n\n{e}\n");
        }
    }

    // [OneTimeTearDown]
    // public static void OneTimeTearDown(){
    //     // KurtsTools.DeleteDirectory(pathToDirectory: _aEmptyDirectoryCreatedDuringCreatingTestCaseData);
    // }

    private static IEnumerable<TestCaseData> Source(){
        yield return new TestCaseData(
            new TestInput{
                OkFiles = new[]{ TestFilesRoot + @"image file from internet.jpg" },
                NotExistingFiles = Array.Empty<string>(),
            },
            new ExpectedResult{
                ExifToolExitCode = 0,
                ExpectedExifFromMultipleFiles = new[]{
                    new ExpectedExifFromSingleFile{
                        ExifItemCount = 35,
                        SomeOfTheExifItems = new[]{
                            new ExifItem{ Group = "ExifTool", Key = "ExifToolVersion", Value = "12.49" },
                            new ExifItem{ Group = "System", Key = "FileName", Value = "image file from internet.jpg" },
                        },
                    },
                },
                ExifToolStdOutStartsWith = "<?xml version='1.0' encoding='UTF-8'?>",
                ExifToolErrOutStartsWith = "",
            }
        ).SetName("1 ok");

        yield return new TestCaseData(
            new TestInput{
                OkFiles = Array.Empty<string>(),
                NotExistingFiles = Array.Empty<string>(),
                EmptyDirectories = Array.Empty<string>(),
                NotExistingDirectories = new[]{ TestFilesRoot + @"not existing directory\" },
            },
            new ExpectedResult{
                ExifToolExitCode = 1,
                ConvertingToXmlException = new System.Xml.XmlException("Root element is missing."),
                ExpectedExifFromMultipleFiles = Array.Empty<ExpectedExifFromSingleFile>(),
                ExifToolStdOutStartsWith = string.Empty,
                ExifToolErrOutStartsWith = "    1 directories scanned\r\n    0 image files read\r\n",
            }
        ).SetName("not existing directory - ending with backslash");


        yield return new TestCaseData(
            new TestInput{
                OkFiles = Array.Empty<string>(),
                NotExistingFiles = Array.Empty<string>(),
                // EmptyDirectories = new[]{_aEmptyDirectoryCreatedDuringCreatingTestCaseData ?? (_aEmptyDirectoryCreatedDuringCreatingTestCaseData = KurtsTools.NewTempDirectory())},
                EmptyDirectories = new[]{TestFilesRoot + @"empty directory" },
            },
            new ExpectedResult{
                ExifToolExitCode = 0,
                ConvertingToXmlException = new System.Xml.XmlException("Root element is missing."),
                ExpectedExifFromMultipleFiles = Array.Empty<ExpectedExifFromSingleFile>(),
                ExifToolStdOutStartsWith = string.Empty,
                ExifToolErrOutStartsWith = "    1 directories scanned\r\n    0 image files read\r\n",
            }
        ).SetName("empty directory - ending with backslash");

        yield return new TestCaseData(
            new TestInput{
                OkFiles = Array.Empty<string>(),
                NotExistingFiles = Array.Empty<string>(),
                EmptyDirectories = new[]{ TestFilesRoot + @"empty directory" },
            },
            new ExpectedResult{
                ExifToolExitCode = 0,
                ConvertingToXmlException = new System.Xml.XmlException("Root element is missing."),
                ExpectedExifFromMultipleFiles = Array.Empty<ExpectedExifFromSingleFile>(),
                ExifToolStdOutStartsWith = string.Empty,
                ExifToolErrOutStartsWith = "    1 directories scanned\r\n    0 image files read\r\n",
            }
        ).SetName("empty directory - ending without backslash");

        yield return new TestCaseData(
            new TestInput{
                OkFiles = new[]{
                    TestFilesRoot + @"image file from internet.jpg",
                    TestFilesRoot + @"jpg files\7P2A1833.jpg",
                },
                NotExistingFiles = Array.Empty<string>(),
            },
            new ExpectedResult{
                ExifToolExitCode = 0,
                ExpectedExifFromMultipleFiles = new[]{
                    new ExpectedExifFromSingleFile{
                        ExifItemCount = 35,
                        SomeOfTheExifItems = new[]{
                            new ExifItem{ Group = "ExifTool", Key = "ExifToolVersion", Value = "12.49" },
                            new ExifItem(group: "System", key: "FileName", value: "image file from internet.jpg"),
                        },
                    },
                    new ExpectedExifFromSingleFile{
                        ExifItemCount = 313,
                        SomeOfTheExifItems = new[]{
                            new ExifItem{ Group = "ExifTool", Key = "ExifToolVersion", Value = "12.49" },
                            new ExifItem{ Group = "System", Key = "FileName", Value = "7P2A1833.jpg" },
                        },
                    },
                },


                ExifToolStdOutStartsWith = "<?xml version='1.0' encoding='UTF-8'?>",
                ExifToolErrOutStartsWith = "    2 image files read\r\n",
            }
        ).SetName("2 ok");

        yield return new TestCaseData(
            new TestInput{
                OkFiles = new[]{
                    TestFilesRoot + @"image file from internet.jpg",
                },
                NotExistingFiles = new[]{
                    TestFilesRoot + @"notExistingFile.jpg",
                },
            },
            new ExpectedResult{
                ExifToolExitCode = 1,
                ExifToolStdOutStartsWith = "<?xml version='1.0' encoding='UTF-8'?>",
                ExifToolErrOutStartsWith =
                    $"Error: File not found - {TestFilesRoot.Replace('\\', '/') + @"notExistingFile.jpg"}\r\n    1 image files read\r\n    1 files could not be read\r\n",
                ExpectedExifFromMultipleFiles = new[]{
                    new ExpectedExifFromSingleFile{
                        ExifItemCount = 35,
                        SomeOfTheExifItems = new[]{
                            new ExifItem{ Group = "ExifTool", Key = "ExifToolVersion", Value = "12.49" },
                            new ExifItem{ Group = "System", Key = "FileName", Value = "image file from internet.jpg" },
                        },
                    },
                },
            }
        ).SetName("1 ok, 1 not existing");

        yield return new TestCaseData(
            new TestInput{
                OkFiles = Array.Empty<string>(),
                NotExistingFiles = Array.Empty<string>(),
            },
            new ExpectedResult{
                ExifToolExitCode = 0,
                ConvertingToXmlException =
                    new System.Xml.XmlException("Data at the root level is invalid. Line 1, position 1."),

                ExifToolStdOutStartsWith =
                    "NAME\r\n    exiftool - Read and write meta information in files\r\n\r\nRUNNING IN WINDOWS",
                ExifToolErrOutStartsWith = "",
            }
        ).SetName("No files");

        yield return new TestCaseData(
            new TestInput{
                OkFiles = new[]{
                    TestFilesRoot + @"jpg files\",
                },
                NotExistingFiles = Array.Empty<string>(),
            },
            new ExpectedResult{
                ExifToolExitCode = 0,
                ConvertingToXmlException = new System.Xml.XmlException("Root element is missing."),
                ExifToolStdOutStartsWith = string.Empty,
                ExifToolErrOutStartsWith = "    1 directories scanned\r\n    0 image files read\r\n",
            }
        ).SetName("1 directory - ending with backslash");

        yield return new TestCaseData(
            new TestInput{
                OkFiles = new[]{
                    TestFilesRoot + @"jpg files",
                },
                NotExistingFiles = Array.Empty<string>(),
            },
            new ExpectedResult{
                ExifToolExitCode = 0,
                ExpectedExifFromMultipleFiles = new[]{
                    new ExpectedExifFromSingleFile{
                        ExifItemCount = 313,
                        SomeOfTheExifItems = new[]{
                            new ExifItem{ Group = "ExifTool", Key = "ExifToolVersion", Value = "12.49" },
                            new ExifItem{ Group = "System", Key = "FileName", Value = "7P2A1833.jpg" },
                        },
                    },
                    new ExpectedExifFromSingleFile{
                        ExifItemCount = 15,
                        SomeOfTheExifItems = new[]{
                            new ExifItem{ Group = "ExifTool", Key = "ExifToolVersion", Value = "12.49" },
                            new ExifItem{ Group = "System", Key = "FileName", Value = "7P2A1833.jpg.exif" },
                        },
                    },
                    new ExpectedExifFromSingleFile{
                        ExifItemCount = 313,
                        SomeOfTheExifItems = new[]{
                            new ExifItem{ Group = "ExifTool", Key = "ExifToolVersion", Value = "12.49" },
                            new ExifItem{ Group = "System", Key = "FileName", Value = "7P2A1834.jpg" },
                        },
                    },
                    new ExpectedExifFromSingleFile{
                        ExifItemCount = 313,
                        SomeOfTheExifItems = new[]{
                            new ExifItem{ Group = "ExifTool", Key = "ExifToolVersion", Value = "12.49" },
                            new ExifItem{ Group = "System", Key = "FileName", Value = "7P2A1835.jpg" },
                        },
                    },
                    new ExpectedExifFromSingleFile{
                        ExifItemCount = 313,
                        SomeOfTheExifItems = new[]{
                            new ExifItem{ Group = "ExifTool", Key = "ExifToolVersion", Value = "12.49" },
                            new ExifItem{ Group = "System", Key = "FileName", Value = "7P2A1836.jpg" },
                        },
                    },
                    new ExpectedExifFromSingleFile{
                        ExifItemCount = 313,
                        SomeOfTheExifItems = new[]{
                            new ExifItem{ Group = "ExifTool", Key = "ExifToolVersion", Value = "12.49" },
                            new ExifItem{ Group = "System", Key = "FileName", Value = "7P2A1837.jpg" },
                        },
                    },
                    new ExpectedExifFromSingleFile{
                        ExifItemCount = 313,
                        SomeOfTheExifItems = new[]{
                            new ExifItem{ Group = "ExifTool", Key = "ExifToolVersion", Value = "12.49" },
                            new ExifItem{ Group = "System", Key = "FileName", Value = "7P2A1838.jpg" },
                        },
                    },
                },
                ExifToolStdOutStartsWith = "<?xml version='1.0' encoding='UTF-8'?>",
                ExifToolErrOutStartsWith = "    1 directories scanned\r\n    7 image files read\r\n",
            }
        ).SetName("1 directory - ending without backslash");
    }

    [Test, TestCaseSource(nameof(Source))]
    public void Test_fn_GetExif(TestInput testInput, ExpectedResult expectedResult){
        GetExifParams getExifParams = new();

        foreach (string okFile in testInput.OkFiles){
            Assume.That(okFile, Does.Exist,
                $"File in collection \"{nameof(testInput)}.{nameof(testInput.OkFiles)}\" not found:\n{okFile}\n");
            getExifParams.InputFiles.Add(okFile);
        }

        foreach (string notExistingFile in testInput.NotExistingFiles){
            Assume.That(notExistingFile, Does.Not.Exist,
                $"File in collection \"{nameof(testInput)}.{nameof(testInput.NotExistingFiles)}\" should not exist:\n{notExistingFile}\n");
            getExifParams.InputFiles.Add(notExistingFile);
        }

        foreach (string emptyDirectory in testInput.EmptyDirectories){
            Assume.That(emptyDirectory, Does.Exist,
                $"Directory in collection \"{nameof(testInput)}.{nameof(testInput.EmptyDirectories)}\" not found:\n{emptyDirectory}\n");
            getExifParams.InputFiles.Add(emptyDirectory);
        }

        foreach (string notExistingDirectory in testInput.NotExistingDirectories){
            Assume.That(notExistingDirectory, Does.Not.Exist,
                $"Directory in collection \"{nameof(testInput)}.{nameof(testInput.NotExistingDirectories)}\" should not exist:\n{notExistingDirectory}\n");
            getExifParams.InputFiles.Add(notExistingDirectory);
        }

        ResultOfGetExif resultOfFunctionResultOfGetExif = _instanceOfExifToolWrapper.GetExif(getExifParams);

        Assert.That(resultOfFunctionResultOfGetExif, Is.Not.Null);

        if (expectedResult.ConvertingToXmlException == null){
            Assert.That(resultOfFunctionResultOfGetExif.ConvertingToXmlException, Is.Null,
                $"<{nameof(resultOfFunctionResultOfGetExif)}.{nameof(resultOfFunctionResultOfGetExif.ConvertingToXmlException)}> failed");
        }
        else{
            Assert.That(resultOfFunctionResultOfGetExif.ConvertingToXmlException, Is.Not.Null,
                $"<{nameof(resultOfFunctionResultOfGetExif)}.{nameof(resultOfFunctionResultOfGetExif.ConvertingToXmlException)}> failed");
            Assert.That(resultOfFunctionResultOfGetExif.ConvertingToXmlException!.GetType(),
                Is.EqualTo(expectedResult.ConvertingToXmlException.GetType()),
                $"<{nameof(resultOfFunctionResultOfGetExif)}.{nameof(resultOfFunctionResultOfGetExif.ConvertingToXmlException)}> failed");
        }


        Assert.That(resultOfFunctionResultOfGetExif.ExitToolReturnCode, Is.EqualTo(expectedResult.ExifToolExitCode));

        if (expectedResult.ExifToolStdOutStartsWith.Equals(string.Empty)){
            Assert.That(resultOfFunctionResultOfGetExif.ExifToolStdOut, Is.Empty,
                $"Result of [{nameof(resultOfFunctionResultOfGetExif)}.{nameof(resultOfFunctionResultOfGetExif.ExifToolStdOut)}] failed:");
        }
        else{
            Assert.That(resultOfFunctionResultOfGetExif.ExifToolErrOut,
                Does.StartWith(expectedResult.ExifToolErrOutStartsWith),
                $"Result of [{nameof(resultOfFunctionResultOfGetExif)}.{nameof(resultOfFunctionResultOfGetExif.ExifToolStdOut)}] failed:");
        }

        if (expectedResult.ExifToolErrOutStartsWith.Equals(string.Empty)){
            Assert.That(resultOfFunctionResultOfGetExif.ExifToolErrOut, Is.Empty,
                $"Result of [{nameof(resultOfFunctionResultOfGetExif)}.{nameof(resultOfFunctionResultOfGetExif.ExifToolErrOut)}] failed:");
        }
        else{
            Assert.That(resultOfFunctionResultOfGetExif.ExifToolStdOut,
                Does.StartWith(expectedResult.ExifToolStdOutStartsWith));
        }

        Assert.That(resultOfFunctionResultOfGetExif.ListOfSingleFiles.Count,
            Is.EqualTo(expectedResult.ExpectedExifFromMultipleFiles.Length),
            $"Result of [{nameof(resultOfFunctionResultOfGetExif)}.{nameof(resultOfFunctionResultOfGetExif.ListOfSingleFiles)}.Count] failed:");


        for (int n = 0; n < expectedResult.ExpectedExifFromMultipleFiles.Length; n++){
            Assert.That(resultOfFunctionResultOfGetExif.ListOfSingleFiles[n].Count,
                Is.EqualTo(expectedResult.ExpectedExifFromMultipleFiles[n].ExifItemCount),
                $"<{nameof(resultOfFunctionResultOfGetExif)}.{nameof(resultOfFunctionResultOfGetExif.ListOfSingleFiles)}[{n}].Count> failed");

            foreach (ExifItem aExpectedItem in expectedResult.ExpectedExifFromMultipleFiles[n].SomeOfTheExifItems){
                ExifItem? findResult = resultOfFunctionResultOfGetExif.ListOfSingleFiles[n].Find(item =>
                    item.Group.Equals(aExpectedItem.Group) && item.Key.Equals(aExpectedItem.Key) &&
                    item.Value.Equals(aExpectedItem.Value));
                Assert.That(findResult, Is.Not.Null,
                    $"Failed finding expected {nameof(ExifItem)}{aExpectedItem.ToString()} in {nameof(resultOfFunctionResultOfGetExif)}.{nameof(resultOfFunctionResultOfGetExif.ListOfSingleFiles)}[{n}]:\n{resultOfFunctionResultOfGetExif.ListOfSingleFiles[n]}");
            }
        }
    }
}

public record ExpectedExifFromSingleFile{
    public int ExifItemCount;
    public ExifItem[] SomeOfTheExifItems = Array.Empty<ExifItem>();
}

public record TestInput{
    public string[] OkFiles = Array.Empty<string>();
    public string[] NotExistingFiles = Array.Empty<string>();
    public string[] EmptyDirectories = Array.Empty<string>();
    public string[] NotExistingDirectories = Array.Empty<string>();
}

public record ExpectedResult{
    public int ExifToolExitCode;

    // public string ErrOut = "";
    // public int ExifListCount;
    public string ExifToolStdOutStartsWith = "";
    public string ExifToolErrOutStartsWith = "";
    public ExpectedExifFromSingleFile[] ExpectedExifFromMultipleFiles = Array.Empty<ExpectedExifFromSingleFile>();
    public Exception? ConvertingToXmlException;
}