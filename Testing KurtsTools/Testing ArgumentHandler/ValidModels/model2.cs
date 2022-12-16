using NSKurtsTools;
using NUnit.Framework;
#pragma warning disable CS0618  // warn on using obsolete element

namespace NSTesting_KurtsTools.Testing_ArgumentHandler.ValidModels;

[TestFixture]
public class Model2{
    private ArgumentModel _argumentModel=null!;
    
    [OneTimeSetUp]
    public void OneTimeSetUp(){
        string[] switchId = { "/?", "/h" }; 

        _argumentModel = new ArgumentModel();
        _argumentModel.AddSimpleKeySet("key1");
        _argumentModel.AddSwitchKey(switchId);
    }


    [Test,Ignore($"Test object {nameof(ArgumentHandler)} is obsolete")]
    public void T1(){
        string[] args = { "arg1" };

        ArgumentHandler argumentHandler = new(_argumentModel, args);
        string x = argumentHandler.GetSimpleArgument("key1");
        
        Assert.That(x,Is.EqualTo("arg1"));
    }
    
    [Test,Ignore($"Test object {nameof(ArgumentHandler)} is obsolete")]
    public void T2(){
        string[] args = { "arg1","arg2" };

        ArgumentHandlerException? ex= Assert.Throws<ArgumentHandlerException>(delegate{
            ArgumentHandler unused = new(_argumentModel, args);
        });
        
        
        Assert.That(ex!.Message,Does.StartWith("Not able to match arguments with model"));
    }

    [Test,Ignore($"Test object {nameof(ArgumentHandler)} is obsolete")]
    public void T3(){
        const string notExistingSimpleKey = "notExistingSimpleKey";

        string[] args = { "arg1" };

        ArgumentHandler argumentHandler = new(_argumentModel, args);
        
        ArgumentHandlerException? ex =Assert.Throws<ArgumentHandlerException>(delegate{
            string unused= argumentHandler.GetSimpleArgument(notExistingSimpleKey);
        });
        
        Assert.That(ex!.Message,Is.EqualTo($"\"{notExistingSimpleKey}\" is not a valid key\n{argumentHandler}"));
    }

    [Test,Ignore($"Test object {nameof(ArgumentHandler)} is obsolete")]
    public void T4(){
        string[] args = { "/?" };
        ArgumentHandler argumentHandler = new(_argumentModel, args);

        bool result = argumentHandler.SwitchExits("/?");
        
        Assert.That(result,Is.True);
    }
}