// by Kurt, october 2021

using System.Runtime.Versioning;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_static_function_ReadConfigFile;

[SupportedOSPlatform("windows")]
[TestFixture]
public class TestingStaticFunctionReadConfigFile{
    private string _tempFolder = null!;
    
    [SetUp]
    public void SetUp(){
        _tempFolder = NSKurtsTools.KurtsTools.NewTempDirectory();
    }

    [SupportedOSPlatform("windows")]
    [TearDown]
    public void TearDown(){
        NSKurtsTools.KurtsTools.DeleteDirectory(_tempFolder);
    }
    
    [SupportedOSPlatform("windows")]
    [Test]
    public void testing_function_ReadConfigFile(){
        string pathToConfigFile= _tempFolder + "file.conf";

        const int antTestKeyValues = 7;
        string[] key = new string[antTestKeyValues];
        string[] value = new string[antTestKeyValues];
        for (int n = 0; n < antTestKeyValues; n++){
            key[n] = $"key{n}";
            value[n] = $"value number {n}";
        }
        
        File.WriteAllLines(pathToConfigFile,new []{
            $"{key[0]} {value[0]}",
            "",
            $"     {key[1]}      {value[1]}      ",
            $"\t\t{key[2]}\t\t{value[2]}\t\t",
            $"\t {key[3]}\t {value[3]}\t ",
            $"#{key[4]} {value[4]}",
            $"# {key[5]} {value[5]}",
            $"  # {key[6]} {value[6]}",
        });
        
        Dictionary<string, string> result= NSKurtsTools.KurtsTools.ReadConfigFile(pathToConfigFile);

        Dictionary<string, string> expected =
            new(new[]{
                new KeyValuePair<string, string>(key[0], value[0]),
                new KeyValuePair<string, string>(key[1], value[1]),
                new KeyValuePair<string, string>(key[2], value[2]),
                new KeyValuePair<string, string>(key[3], value[3]),
            });
        
        Assert.That(result,Is.EquivalentTo(expected));
    }

}