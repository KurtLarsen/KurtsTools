using NSKurtsTools;
using NUnit.Framework;

// ReSharper disable UnusedVariable
#pragma warning disable CS0618


namespace NSTesting_KurtsTools.Testing_CmdLineTokenizer;

[TestFixture]
public class Testing_Arguments{
    [Test,Ignore($"Ignore testing obsolete class {nameof(CmdLineTokenizer)}")]
    public void test_of_something(){
        string[] args ={ "-t","5", "-h" };

        Arguments arguments = new(args);
        

        while (arguments.NextItem){
            switch (arguments.CurrentItem){
                case "-h":
                    // showHelp();
                    break;
                case "-t":
                case "--timeout":
                    string? timeout = arguments.GetOptionArgumentOrMissingOptionArgumentFail;
                    string? timeout2 = arguments.GetOptionArgumentOrMissingOptionArgumentFail;
                    string? timeout3 = arguments.GetOptionArgumentOrMissingOptionArgumentFail;
                    break;
                default:
                    string? value = arguments.CurrentItemAsValueOrUnknownOptionFail;
                    break;
            }
        }

        Assert.That(arguments.HasError, Is.True);
        Assert.That(arguments.ErrorText, Is.EqualTo("Option -t is missing argument"));
    }
}