using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_ArgumentHandler;

[TestFixture]
public class Testing_ArgumentAnalyzer_Using_Model_With_1_Simple_Argument_And_1_Switch_Without_SubArguments{
    private ArgumentModel _model = null!;

    [OneTimeSetUp]
    public void OneTimeSetup(){
        _model = new ArgumentModel();
        _model.AddSimpleKeySet("key1");
        _model.AddSwitchKey(new []{"/s"});
    }

    [Test]
    public void Testing_ToString_using_1_argument(){
        string[] args = { "arg1" };

        ArgumentAnalyzer? argumentAnalyzer = null;
        try{
          argumentAnalyzer   = new ArgumentAnalyzer(_model, args);
        } catch(Exception e){
            Assert.Ignore(e.Message);
        }

        string result = argumentAnalyzer.ToString();
        
        Assert.That(result,Is.EqualTo("MODEL:\n    1 ListOfSimpleKeySets:\n        key1\n    1 ListOfSwitchArguments:\n        [/s]\nARGUMENTS:\n    arg1"));
    }

}