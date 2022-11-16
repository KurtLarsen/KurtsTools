using System.Runtime.Versioning;
using NSKurtsTools;
using NUnit.Framework;


namespace NSTesting_KurtsTools.Testing_PhotoTools;

[TestFixture, SupportedOSPlatform("windows")]
public class Testing_Class_ExitToolWrapper{
    private const string PathToExifTool = @"Testing PhotoTools\exiftool.exe\exiftool_v12.49.exe";
    private ExifToolWrapper _instanceOfExifToolWrapper = null!;

    [OneTimeSetUp]
    public void OneTimeSetup(){
        try{
            _instanceOfExifToolWrapper = new ExifToolWrapper(PathToExifTool);
        } catch (Exception e){
            Assume.That(false, $"An exception was thrown when creating instance of {nameof(EmptyDirectory)}\n{e}");
        }
    }

    private static IEnumerable<TestCaseData> Source(){
        yield return new TestCaseData(
            new TestInput{
                OkFiles = new[]{
                    @"Testing PhotoTools\TestData\image file from internet.jpg",
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
                NotExistingDirectories=new []{@"Testing PhotoTools\TestData\not existing directory\"},
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
                EmptyDirectories = new []{@"Testing PhotoTools\TestData\empty directory\"},
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
                EmptyDirectories = new []{@"Testing PhotoTools\TestData\empty directory"},
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
                    @"Testing PhotoTools\TestData\image file from internet.jpg",
                    @"Testing PhotoTools\TestData\jpg files\7P2A1833.jpg",
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
                    @"Testing PhotoTools\TestData\image file from internet.jpg",
                },
                NotExistingFiles = new[]{
                    @"Testing PhotoTools\TestData\notExistingFile.jpg",
                },
            },
            new ExpectedResult{
                ExifToolExitCode = 1,
                ExifToolStdOutStartsWith = "<?xml version='1.0' encoding='UTF-8'?>",
                ExifToolErrOutStartsWith =
                    "Error: File not found - Testing PhotoTools/TestData/notExistingFile.jpg\r\n    1 image files read\r\n    1 files could not be read\r\n",
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
                ConvertingToXmlException = new System.Xml.XmlException("Data at the root level is invalid. Line 1, position 1."),
                
                ExifToolStdOutStartsWith =
                    "NAME\r\n    exiftool - Read and write meta information in files\r\n\r\nRUNNING IN WINDOWS",
                ExifToolErrOutStartsWith = "",
            }
        ).SetName("No files");

        yield return new TestCaseData(
            new TestInput{
                OkFiles = new[]{
                    @"Testing PhotoTools\TestData\jpg files\",
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
                    @"Testing PhotoTools\TestData\jpg files",
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
    public void Test_function_GetExif(TestInput input, ExpectedResult expectedResult){
        GetExifParams getExifParams = new();

        foreach (string okFile in input.OkFiles){
            Assume.That(okFile, Does.Exist);
            getExifParams.InputFiles.Add(okFile);
        }

        foreach (string notExistingFile in input.NotExistingFiles){
            Assume.That(notExistingFile, Does.Not.Exist);
            getExifParams.InputFiles.Add(notExistingFile);
        }

        foreach (string emptyDirectory in input.EmptyDirectories){
            Assume.That(emptyDirectory,Does.Exist);
            getExifParams.InputFiles.Add(emptyDirectory);
        }

        foreach (string notExistingDirectory in input.NotExistingDirectories){
            Assume.That(notExistingDirectory,Does.Not.Exist);
            getExifParams.InputFiles.Add(notExistingDirectory);
        }

        ResultOfGetExif resultOfFunctionResultOfGetExif = _instanceOfExifToolWrapper.GetExif(getExifParams);

        Assert.That(resultOfFunctionResultOfGetExif, Is.Not.Null);

        if (expectedResult.ConvertingToXmlException == null){
            Assert.That(resultOfFunctionResultOfGetExif.ConvertingToXmlException,Is.Null,$"<{nameof(resultOfFunctionResultOfGetExif)}.{nameof(resultOfFunctionResultOfGetExif.ConvertingToXmlException)}> failed");
        }
        else{
            Assert.That(resultOfFunctionResultOfGetExif.ConvertingToXmlException,Is.Not.Null,$"<{nameof(resultOfFunctionResultOfGetExif)}.{nameof(resultOfFunctionResultOfGetExif.ConvertingToXmlException)}> failed");
            Assert.That(resultOfFunctionResultOfGetExif.ConvertingToXmlException!.GetType(),Is.EqualTo(expectedResult.ConvertingToXmlException.GetType()),$"<{nameof(resultOfFunctionResultOfGetExif)}.{nameof(resultOfFunctionResultOfGetExif.ConvertingToXmlException)}> failed");    
        }
        

        Assert.That(resultOfFunctionResultOfGetExif.ExitToolReturnCode, Is.EqualTo(expectedResult.ExifToolExitCode));

        if (expectedResult.ExifToolStdOutStartsWith.Equals(string.Empty)){
            Assert.That(resultOfFunctionResultOfGetExif.ExifToolStdOut, Is.Empty,
                $"Result of [{nameof(resultOfFunctionResultOfGetExif)}.{nameof(resultOfFunctionResultOfGetExif.ExifToolStdOut)}] failed:");
        }
        else{
            Assert.That(resultOfFunctionResultOfGetExif.ExifToolErrOut, Does.StartWith(expectedResult.ExifToolErrOutStartsWith),
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

        Assert.That(resultOfFunctionResultOfGetExif.ListOfSingleFiles.Count, Is.EqualTo(expectedResult.ExpectedExifFromMultipleFiles.Length),
            $"Result of [{nameof(resultOfFunctionResultOfGetExif)}.{nameof(resultOfFunctionResultOfGetExif.ListOfSingleFiles)}.Count] failed:");


        for (int n = 0; n < expectedResult.ExpectedExifFromMultipleFiles.Length; n++){
            Assert.That(resultOfFunctionResultOfGetExif.ListOfSingleFiles[n].Count,
                Is.EqualTo(expectedResult.ExpectedExifFromMultipleFiles[n].ExifItemCount),$"<{nameof(resultOfFunctionResultOfGetExif)}.{nameof(resultOfFunctionResultOfGetExif.ListOfSingleFiles)}[{n}].Count> failed");

            foreach (ExifItem aExpectedItem in expectedResult.ExpectedExifFromMultipleFiles[n].SomeOfTheExifItems){
                ExifItem? findResult = resultOfFunctionResultOfGetExif.ListOfSingleFiles[n].Find(item =>
                    item.Group.Equals(aExpectedItem.Group) && item.Key.Equals(aExpectedItem.Key) &&
                    item.Value.Equals(aExpectedItem.Value));
                Assert.That(findResult, Is.Not.Null,$"Failed finding expected {nameof(ExifItem)}{aExpectedItem.ToString()} in {nameof(resultOfFunctionResultOfGetExif)}.{nameof(resultOfFunctionResultOfGetExif.ListOfSingleFiles)}[{n}]:\n{resultOfFunctionResultOfGetExif.ListOfSingleFiles[n]}");
            }
        }
    }




    private const string EmptyDirectory = @"Testing PhotoTools\TestData\empty directory";
}

public record ExpectedExifFromSingleFile{
    public int ExifItemCount;
    public ExifItem[] SomeOfTheExifItems= Array.Empty<ExifItem>();
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