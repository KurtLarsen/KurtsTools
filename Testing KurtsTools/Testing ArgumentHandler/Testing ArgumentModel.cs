using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_ArgumentHandler;

[TestFixture]
public class Testing_ArgumentModel{
    [Test]
    public void Testing_ToString(){
        ArgumentModel model = new();
        model.AddSimpleKeySet("key1","key2");
        model.AddSwitchKey(new []{"/x","/y"},"subKey1","subKey2");

        string result = model.ToString();
        
        Assert.That(result,Is.EqualTo("MODEL:\n    1 ListOfSimpleKeySets:\n        key1 key2\n    1 ListOfSwitchArguments:\n        [/x|/y] subKey1 subKey2\n"));
    }
}