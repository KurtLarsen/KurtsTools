using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_ArgumentHandler;

[TestFixture]
// ReSharper disable once InconsistentNaming
public class Testing_function_Default{
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
    public void Testing_key_not_found_use_default(){
        const string notFoundKeyWord = "/config2";
        string[] defaultValue = { "xyz" };
        
        CmdArgumentItem result = _cmdArgumentHandler.GetArgumentItem(notFoundKeyWord).Default(defaultValue);
        
        Assert.That(result.Key,Is.EqualTo(""));
        Assert.That(result.SubParams,Is.EqualTo(defaultValue));
    }

    [Test]
    public void Testing_key_found_do_not_use_default(){
        string[] defaultValue = { "xyz" };

        CmdArgumentItem result = _cmdArgumentHandler.GetArgumentItem(ConfigKey).Default(defaultValue);
        
        Assert.That(result.Key,Is.EqualTo(ConfigKey));
        Assert.That(result.SubParams,Is.EqualTo(_configParam));
    }
}