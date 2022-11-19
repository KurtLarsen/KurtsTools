

using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_ArgumentHandler;

[TestFixture]
public class TestingFunctionExists{
    private const string FileKey = "/file";
    private readonly string[] _fileParam = { "filename.txt" };
    private const string ConfigKey = "/config";
    private readonly string[] _configParam = { "filename.conf" };
    private const string DummyKey = "/d";
    private string[] _args = null!;
    private CmdArgumentHandler _cmdArgumentHandler = null!;

    [OneTimeSetUp]
    public void OneTimeSetup(){
        _args =new []{FileKey}
            .Concat(_fileParam)
            .Concat(new[]{ ConfigKey })
            .Concat(_configParam)
            .Concat(new []{DummyKey})
            .ToArray();
        _cmdArgumentHandler = new CmdArgumentHandler(_args);
    }

    [Test]
    public void function_Exists_return_true_when__argument_exists(){
        bool result = _cmdArgumentHandler.ArgumentKeyExists(ConfigKey);
        
        Assert.That(result,Is.True);
    }
    
    [Test]
    public void function_Exists_return_false_when__argument_does_not_exist(){
        bool result = _cmdArgumentHandler.ArgumentKeyExists("xxx");
        
        Assert.That(result,Is.False);
    }

}