using NSKurtsTools;
using NUnit.Framework;

// ReSharper disable RedundantStringInterpolation
#pragma warning disable CS0618

namespace NSTesting_KurtsTools.Testing_CmdLineTokenizer;

[TestFixture]
public class Testing_Function_Tokens{
    private static readonly OptionDefinition HelpDefinition = new(){ Name = "help", ShortId = "?", LongId = "help" };

    private static readonly OptionDefinition DeltaDefinition = new()
        { Name = "deltaTime", ShortId = "d", LongId = "delta", OptionValueCount = 1 };

    private static readonly OptionDefinition RecursiveDefinition = new(){ Name = "recursive", ShortId = "r" };


    private static CmdLineTokenizer? _cmdLineTokenizer;
    
    [OneTimeSetUp]
    public static void OneTimeSetUp(){
        _cmdLineTokenizer = new CmdLineTokenizer(new[]{
                HelpDefinition,
                DeltaDefinition,
                RecursiveDefinition,
            }
        );
    }

    private static IEnumerable<TestCaseData> CmdLinesAndExpectedResult(){
        yield return new TestCaseData(
            new[]{ "/?" },
            new[]{
                new Token{ Name = HelpDefinition.Name, Type = TokenType.Option},
            }).SetName("[ok arguments]    /?");
        yield return new TestCaseData(
            new[]{ "/d", "500", "abc.jpg" },
            new[]{
                new Token{ Name = DeltaDefinition.Name, Type = TokenType.Option, Values ={ "500" }},
                new Token{ Name = "", Type = TokenType.Value, Values ={ "abc.jpg" } },
            }).SetName("[ok arguments]    /d 500 abc.jpg");
        yield return new TestCaseData(
            new[]{ "abc.jpg", "/d", "500" },
            new[]{
                new Token{ Name = "", Type = TokenType.Value, Values ={ "abc.jpg" } },
                new Token{ Name = DeltaDefinition.Name, Type = TokenType.Option, Values ={ "500" }},
            }).SetName("[ok arguments]    abc.jpg /d 500");
        yield return new TestCaseData(
            new[]{ "/d", "500", "abc.jpg", "/r" },
            new[]{
                new Token{ Type = TokenType.Option, Name = DeltaDefinition.Name, Values ={ "500" }},
                new Token{ Type = TokenType.Value, Name = "", Values ={ "abc.jpg" } },
                new Token{ Type = TokenType.Option, Name = RecursiveDefinition.Name},
            }).SetName("[ok arguments]    /d 500 abc.jpg /r");
        yield return new TestCaseData(
            new[]{ "/x" },
            new[]{
                new Token{ Type = TokenType.UnknownOption, Name = "/x"},
            }).SetName("[arguments with unknown option]    /x");
        yield return new TestCaseData(
            new[]{ "/x", "abc.jpg" },
            new[]{
                new Token{ Type = TokenType.UnknownOption, Name = "/x"},
            }).SetName("[arguments with unknown option]    /x abc.jpg");
        yield return new TestCaseData(
            new[]{ "abc.jpg", "/x" },
            new[]{
                new Token{ Type = TokenType.Value, Name = "", Values ={ "abc.jpg" } },
                new Token{ Type = TokenType.UnknownOption, Name = "/x" },
            }).SetName("[arguments with unknown option]    abc.jpg /x");
        yield return new TestCaseData(
            new[]{ "/d" },
            new[]{
                new Token{ Type = TokenType.InvalidOption, Name = DeltaDefinition.Name },
            }).SetName("[arguments with invalid option values]    /d");
    }

    [Test, TestCaseSource(nameof(CmdLinesAndExpectedResult))]
    public void testing_result_from_Token(string[] args, Token[] expectedTokens){
        Token[] result = _cmdLineTokenizer!.Tokens(args);
        Assert.That(result.Length, Is.EqualTo(expectedTokens.Length),
            "Number of Tokens in result differs from expected");
        for (int n = 0; n < result.Length; n++){
            Assert.That(result[n].Type, Is.EqualTo(expectedTokens[n].Type),
                $"_type is different in token {n}.\n" +
                $" Expected\n {TokensToString(expectedTokens)}\n" +
                $"But was\n" +
                $" {TokensToString(result)}\n");
            Assert.That(result[n].Name, Is.EqualTo(expectedTokens[n].Name));
            Assert.That(result[n].Values, Is.EquivalentTo(expectedTokens[n].Values));
        }
    }

    private static string TokensToString(IEnumerable<Token> tokens){
        string s = "";
        foreach (Token token in tokens){
            if (s != "") s += ", ";
            s += "<" + token + ">";
        }

        return s;
    }
}