
using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_ArgumentHandler;

[TestFixture]
public class TestingFunctionGetArgumentItem{
    private const string FileKey = "/file";
    private readonly string[] _fileParam ={ "filename.txt" };
    private const string ConfigKey = "/config";
    private readonly string[] _configParam ={ "filename.conf" };
    private const string DummyKey = "/d";
    private string[] _args = null!;
    private CmdArgumentHandler _cmdArgumentHandler = null!;

    [OneTimeSetUp]
    public void OneTimeSetup(){
        _args = new[]
                { FileKey }
            .Concat(_fileParam)
            .Concat(new[]{ ConfigKey })
            .Concat(_configParam)
            .Concat(new[]{ DummyKey })
            .ToArray();
        _cmdArgumentHandler = new CmdArgumentHandler(_args);
    }

    [Test]
    public void Testing_key_not_found(){
        CmdArgumentItem result = _cmdArgumentHandler.GetArgumentItem("/xx");
        Assert.That(result.Key, Is.EqualTo(""));
        Assert.That(result.SubParams, Is.EqualTo(Array.Empty<string>()));
    }

    [Test]
    public void Testing_config_abc(){
        CmdArgumentItem result = _cmdArgumentHandler.GetArgumentItem(ConfigKey);
        Assert.That(result.Key, Is.EqualTo(ConfigKey));
        Assert.That(result.SubParams, Is.EqualTo(_configParam));
    }
}