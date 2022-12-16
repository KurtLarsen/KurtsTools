using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_ArgumentHandler.Testing_class_SwitchArgument;

[TestFixture]
public class Testing_Class_SwitchArgument{
    
    [Test]
    public void testing_function_ToString_using_1_id(){
        const string id = "/?";
        
        SwitchArgument switchArgument = new(id);

        string s = switchArgument.ToString();
        
        Assert.That(s,Is.EqualTo($"[{id}]\n"));
    }
    
    [Test]
    public void testing_function_ToString_using_2_id(){
        const string id1 = "/i?";
        const string id2 = "/h";
        
        SwitchArgument switchArgument = new(new []{id1,id2 });

        string s = switchArgument.ToString();
        
        Assert.That(s,Is.EqualTo($"[{id1}|{id2}]\n"));
    }

    [Test]
    public void testing_function_ToString_using_1_id_and_1_subParam(){
        const string id = "/?";
        const string sp = "subKey";
        
        SwitchArgument switchArgument = new(id,sp);

        string s = switchArgument.ToString();
        
        Assert.That(s,Is.EqualTo($"[{id}] {sp}\n"));
    }

    [Test]
    public void testing_function_ToString_using_1_id_and_2_subParam(){
        const string id = "/?";
        const string sp1 = "subKey1";
        const string sp2 = "subKey2";
        
        SwitchArgument switchArgument = new(id,sp1,sp2);

        string s = switchArgument.ToString();
        
        Assert.That(s,Is.EqualTo($"[{id}] {sp1} {sp2}\n"));
    }

    [Test]
    public void testing_function_ToString_using_2_id_and_2_subParam(){
        const string id1 = "/?";
        const string id2 = "/h";
        const string sp1 = "subParam1";
        const string sp2 = "subParam2";
        
        SwitchArgument switchArgument = new(new []{id1,id2},sp1,sp2);

        string s = switchArgument.ToString();
        
        Assert.That(s,Is.EqualTo($"[{id1}|{id2}] {sp1} {sp2}\n"));
    }
}