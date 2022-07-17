

using System.Xml;
using NUnit.Framework;
using KurtsToolsLibrary.KurtsFileTools;
using KurtsToolsLibrary.XmlTools;

namespace Testing_KurtsToolsLibrary.Testing_File_Tools;

[TestFixture]
public class Testing_DirectoryStructureIsEqual{
    private string _tempDir = "";


    private static readonly XmlDocument BaseStructure = XmlTools.StringToXmlDocument(
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
    private static readonly XmlDocument WithoutFile1 = XmlTools.StringToXmlDocument(
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
    private static readonly XmlDocument WithoutFile2 = XmlTools.StringToXmlDocument(
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
    private static readonly XmlDocument WithoutFile5 = XmlTools.StringToXmlDocument(
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
    private static readonly XmlDocument WithoutSubdir1 = XmlTools.StringToXmlDocument(
        "<root>" +
        "   <file name='file1.txt'/>" +
        "   <folder name='subdir2'>" +
        "       <file name='file4.txt'/>" +
        "       <file name='file5.txt'/>" +
        "   </folder>" +
        "</root>");
    private static readonly XmlDocument ExtraFile6InRoot = XmlTools.StringToXmlDocument(
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
    private static readonly XmlDocument ExtraFile6InSubdir2 = XmlTools.StringToXmlDocument(
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
        _tempDir = KurtsFileTools.NewTempDirectory();
    }

    [TearDown]
    public void TearDown(){
        KurtsFileTools.DeleteDirectory(_tempDir);
    }

    public static IEnumerable<TestCaseData> TestCaseData(){
        yield return new TestCaseData(BaseStructure, BaseStructure,"").SetName("equal structures");
        yield return new TestCaseData(BaseStructure, WithoutFile1,"^Extra file \"file1.txt\" found in path \\S+\\\\$").SetName("without file1");
        yield return new TestCaseData(BaseStructure, WithoutFile2,"^Extra file \"file2.txt\" found in path \\S+\\\\subdir1$").SetName("without file2");
        yield return new TestCaseData(BaseStructure, WithoutFile5,"^Extra file \"file5.txt\" found in path \\S+\\\\subdir2$").SetName("without file5");
        yield return new TestCaseData(BaseStructure, WithoutSubdir1,"^Extra subdirectory \"subdir1\" found in path \\S+\\\\$").SetName("without subdir1");
        yield return new TestCaseData(BaseStructure, ExtraFile6InRoot,"^File \"file6.txt\" not found in path \\S+\\\\$").SetName("extra file6 in root");
        yield return new TestCaseData(BaseStructure, ExtraFile6InSubdir2,"^File \"file6.txt\" not found in path \\S+\\\\subdir2$").SetName("extra file6 in subdir2");
        // yield return new TestCaseData(baseStructure, extraSubdirInRoot,"^Subdirectory \"subdirectory3\" not found in path \\S+\\\\subdir2$").SetName("extra subdir3 in root");
    }


    [Test, TestCaseSource(nameof(TestCaseData))]
    public void test_of_DirectoryStructureEqual(XmlDocument directoryStructureToBuild,
        XmlDocument directoryStructureToCompareWith,string expectedResult){
        KurtsFileTools.BuildDirectoryStructure(_tempDir, directoryStructureToBuild);
        string result = KurtsFileTools.DirectoryStructureIsEqual(_tempDir, directoryStructureToCompareWith);
        Assert.That(result,Does.Match(expectedResult), $"Result of {nameof(KurtsFileTools)}.{nameof(KurtsFileTools.DirectoryStructureIsEqual)}()");
    }
}