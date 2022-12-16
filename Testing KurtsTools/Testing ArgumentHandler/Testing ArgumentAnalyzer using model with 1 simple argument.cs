using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_ArgumentHandler;

[TestFixture]
public class Testing_ArgumentAnalyzer{
    private ArgumentModel _model=null!;

    [OneTimeSetUp]
    public void OneTimeSetup(){
            _model = new ArgumentModel();
            _model.AddSimpleKeySet("key1");
    }

    [Test]
    public void Testing_ArgumentAnalyzer_constructor_using_0_argument(){
        string[] args = Array.Empty<string>();
        
      ArgumentHandlerException? ex=  Assert.Throws<ArgumentHandlerException>(delegate{
            ArgumentAnalyzer unused = new(_model, args);
        });
        
        Assert.That(ex!.Message,Does.StartWith("Analyzer says: # of simple arguments does not match model.\nModel:\n"));
    }
    
    [Test]
    public void Testing_ArgumentAnalyzer_constructor_using_1_argument(){
        string[] args = { "arg1" };
        
        ArgumentAnalyzer x = new(_model, args);
        Assert.That(x,Is.Not.Null);
    }

    [Test]
    public void Testing_SimpleArgumentCount_using_0_argument(){
        string[] args = Array.Empty<string>();
        
        ArgumentHandlerException? ex=  Assert.Throws<ArgumentHandlerException>(delegate{
            ArgumentAnalyzer unused = new(_model, args);
        });
        
        Assert.That(ex!.Message,Does.StartWith("Analyzer says: # of simple arguments does not match model.\nModel:\n"));
    }

    [Test]
    public void Testing_SimpleArgumentCount_using_1_argument(){
        string[] args = { "arg1" };
        
        ArgumentAnalyzer x = new(_model, args);
        Assume.That(x,Is.Not.Null);

        int y = x.SimpleArgumentCount;
        Assert.That(y,Is.EqualTo(1));
    }
    
    [Test]
    public void Testing_GetSimpleArgument_using_1_argument(){
        string[] args = { "arg1" };
        
        ArgumentAnalyzer x = new(_model, args);
        Assume.That(x,Is.Not.Null);

        string y = x.GetSimpleArgument("key1");
        Assert.That(y,Is.EqualTo("arg1"));
    }

    [Test]
    public void Testing_GetSimpleArgument_using_2_argument_when_model_only_is_defined_with_1_arguments(){
        string[] args = { "arg1", "arg2" };

        ArgumentHandlerException? ex=  Assert.Throws<ArgumentHandlerException>(delegate{
            ArgumentAnalyzer unused = new(_model, args);
        });
        
        Assert.That(ex!.Message,Does.StartWith("Analyzer says: # of simple arguments does not match model.\nModel:\n"));

    }

    [Test]
    public void Testing_ToString(){
        string[] args = { "arg1"};

        ArgumentAnalyzer x = new(_model, args);
        
        Assert.That(x.ToString(),Is.EqualTo("MODEL:\n    1 ListOfSimpleKeySets:\n        key1\n    0 ListOfSwitchArguments\nARGUMENTS:\n    arg1"));
        
    }

    [Test]
    public void Testing_ToString_with_indent(){
        string[] args = { "arg1"};

        ArgumentAnalyzer x = new(_model, args);
        
        Assert.That(x.ToString(4),Is.EqualTo("MODEL:\n        1 ListOfSimpleKeySets:\n            key1\n        0 ListOfSwitchArguments\nARGUMENTS:\n        arg1"));
        
    }

}