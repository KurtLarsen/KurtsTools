using NSKurtsTools;
using NUnit.Framework;

#pragma warning disable CS0618

namespace NSTesting_KurtsTools.Testing_CmdLineTokenizer;

[TestFixture]
public class Testing_Constructor_For_CmdLineTokenizer{
    [Test]
    public void can_construct_ArgumentTokenizer_without_arguments(){
        CmdLineTokenizer? cmdLineTokenizer = null;
        Exception? ex = null;
        try{
            cmdLineTokenizer = new CmdLineTokenizer();
        } catch (Exception e){
            ex = e;
        }
        Assert.That(cmdLineTokenizer, Is.TypeOf<CmdLineTokenizer>(), ex?.ToString());
    }

    [Test]
    public void can_construct_CmdLineTokenizer_with_arguments(){
        OptionDefinition[] parameterDefinitions = {
            new(){ Name = "help", ShortId = "?", LongId = "help" },
            new(){Name = "deltatime",ShortId = "d",LongId = "delta",OptionValueCount = 1},
        };
        
        CmdLineTokenizer? cmdLineTokenizer = null;
        Exception? ex = null;
        try{
            cmdLineTokenizer = new CmdLineTokenizer(parameterDefinitions);
        } catch (Exception e){
            ex = e;
        }
        Assert.That(cmdLineTokenizer, Is.TypeOf<CmdLineTokenizer>(), ex?.ToString());
    }

}