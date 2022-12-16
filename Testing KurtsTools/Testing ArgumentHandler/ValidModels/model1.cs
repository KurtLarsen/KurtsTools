using NSKurtsTools;
using NUnit.Framework;
#pragma warning disable CS0618  // warn on using obsolete element

namespace NSTesting_KurtsTools.Testing_ArgumentHandler.ValidModels;

[TestFixture]
public class Model1{
    private ArgumentModel _argumentModel=null!;
    
    [OneTimeSetUp]
    public void OneTimeSetUp(){
        _argumentModel = new ArgumentModel();
        _argumentModel.AddSimpleKeySet("key1");
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

}