using System.Runtime.Versioning;
using System.Xml;
using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_DirectoryTools;
[SupportedOSPlatform("windows")]
[TestFixture]
public class Testing_DirectoryStructureIsEqual{
    private string _tempDir = "";


    private static readonly XmlDocument BaseStructure = KurtsTools.StringToXmlDocument(
        "<root>" +
        "   <file name='file1.txt'/>" +
        "   <folder name='subdir1'>" +
        "       <file name='file2.txt'/>" +
        "       <file name='file3.txt'/>" +
        "   </folder>" +
        "   <folder name='subdir2'>" +
        "       <file name='file4.txt'/>" +
        "       <file name='file5.txt'/>" +
        "   </folder>" +
        "</root>");
    private static readonly XmlDocument WithoutFile1 = KurtsTools.StringToXmlDocument(
        "<root>" +
        "   <folder name='subdir1'>" +
        "       <file name='file2.txt'/>" +
        "       <file name='file3.txt'/>" +
        "   </folder>" +
        "   <folder name='subdir2'>" +
        "       <file name='file4.txt'/>" +
        "       <file name='file5.txt'/>" +
        "   </folder>" +
        "</root>");
    private static readonly XmlDocument WithoutFile2 = KurtsTools.StringToXmlDocument(
        "<root>" +
        "   <file name='file1.txt'/>" +
        "   <folder name='subdir1'>" +
        "       <file name='file3.txt'/>" +
        "   </folder>" +
        "   <folder name='subdir2'>" +
        "       <file name='file4.txt'/>" +
        "       <file name='file5.txt'/>" +
        "   </folder>" +
        "</root>");
    private static readonly XmlDocument WithoutFile5 = KurtsTools.StringToXmlDocument(
        "<root>" +
        "   <file name='file1.txt'/>" +
        "   <folder name='subdir1'>" +
        "       <file name='file2.txt'/>" +
        "       <file name='file3.txt'/>" +
        "   </folder>" +
        "   <folder name='subdir2'>" +
        "       <file name='file4.txt'/>" +
        "   </folder>" +
        "</root>");
    private static readonly XmlDocument WithoutSubdir1 = KurtsTools.StringToXmlDocument(
        "<root>" +
        "   <file name='file1.txt'/>" +
        "   <folder name='subdir2'>" +
        "       <file name='file4.txt'/>" +
        "       <file name='file5.txt'/>" +
        "   </folder>" +
        "</root>");
    private static readonly XmlDocument ExtraFile6InRoot = KurtsTools.StringToXmlDocument(
        "<root>" +
        "   <file name='file1.txt'/>" +
        "   <folder name='subdir1'>" +
        "       <file name='file2.txt'/>" +
        "       <file name='file3.txt'/>" +
        "   </folder>" +
        "   <folder name='subdir2'>" +
        "       <file name='file4.txt'/>" +
        "       <file name='file5.txt'/>" +
        "   </folder>" +
        "   <file name='file6.txt'/>" +
        "</root>");
    private static readonly XmlDocument ExtraFile6InSubdir2 = KurtsTools.StringToXmlDocument(
        "<root>" +
        "   <file name='file1.txt'/>" +
        "   <folder name='subdir1'>" +
        "       <file name='file2.txt'/>" +
        "       <file name='file3.txt'/>" +
        "   </folder>" +
        "   <folder name='subdir2'>" +
        "       <file name='file4.txt'/>" +
        "       <file name='file5.txt'/>" +
        "       <file name='file6.txt'/>" +
        "   </folder>" +
        "</root>");


    [SetUp]
    public void SetUp(){
        _tempDir = KurtsTools.NewTempDirectory();
    }

    [TearDown]
    public void TearDown(){
        KurtsTools.DeleteDirectory(pathToDirectory: _tempDir);
    }

    public static IEnumerable<TestCaseData> TestCaseData(){
        yield return new TestCaseData(BaseStructure, BaseStructure,"").SetName("equal structures");
        yield return new TestCaseData(BaseStructure, WithoutFile1,"^Extra file \"file1.txt\" found in path \\S+\\\\$").SetName("without file1");
        yield return new TestCaseData(BaseStructure, WithoutFile2,"^Extra file \"file2.txt\" found in path \\S+\\\\subdir1$").SetName("without file2");
        yield return new TestCaseData(BaseStructure, WithoutFile5,"^Extra file \"file5.txt\" found in path \\S+\\\\subdir2$").SetName("without file5");
        yield return new TestCaseData(BaseStructure, WithoutSubdir1,"^Extra subdirectory \"subdir1\" found in path \\S+\\\\$").SetName("without subdir1");
        yield return new TestCaseData(BaseStructure, ExtraFile6InRoot,"^File \"file6.txt\" not found in path \\S+\\\\$").SetName("extra file6 in root");
        yield return new TestCaseData(BaseStructure, ExtraFile6InSubdir2,"^File \"file6.txt\" not found in path \\S+\\\\subdir2$").SetName("extra file6 in subdir2");
    }


    [Test, TestCaseSource(nameof(TestCaseData))]
    public void test_of_DirectoryStructureEqual(XmlDocument directoryStructureToBuild,
        XmlDocument directoryStructureToCompareWith,string expectedResult){
        KurtsTools.BuildDirectoryStructure(_tempDir, directoryStructureToBuild);
        string result = KurtsTools.DirectoryStructureIsEqual(_tempDir, directoryStructureToCompareWith);
        Assert.That(result,Does.Match(expectedResult), $"Result of {nameof(KurtsTools)}.{nameof(KurtsTools.DirectoryStructureIsEqual)}()");
    }
}