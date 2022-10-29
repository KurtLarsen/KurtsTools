// by Kurt, october 2021

using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_String_Extensions;

[TestFixture]
public class Testing_String_Extensions{
    public static IEnumerable<TestCaseData> TestCaseData(){
        CharToTest[] charsToTest ={
            new(" ", true, "<space>"),
            new("\t", true, "<tab>"),
            new(".", false, "<dot>"),
            new("", false, "<empty>"),
        };
        const string word1 = "ABC";
        const string word2 = "DEF";
        const string word3 = "GHI";

        foreach (CharToTest charToTest1 in charsToTest){
            foreach (CharToTest charToTest2 in charsToTest){
                foreach (CharToTest charToTest3 in charsToTest){
                    foreach (CharToTest charToTest4 in charsToTest){
                        string testString = charToTest1.C 
                                            + word1 + charToTest2.C 
                                            + word2 + charToTest3.C 
                                            + word3 + charToTest4.C;
                        string expectedResult = charToTest1.IsDelimiter ? "" : charToTest1.C;
                        expectedResult += word1;
                        expectedResult += charToTest2.IsDelimiter ? "" : charToTest2.C;
                        if (!charToTest2.IsDelimiter){
                            expectedResult += word2;
                            expectedResult += charToTest3.IsDelimiter ? "" : charToTest3.C;
                            if (!charToTest3.IsDelimiter){
                                expectedResult += word3;
                                expectedResult += charToTest4.IsDelimiter ? "" : charToTest4.C;
                            }
                        }
                        string testName = charToTest1.Name + word1 + charToTest2.Name + word2 + charToTest3.Name + word3 +
                                          charToTest4.Name+$" (expected result: {expectedResult})";

                        yield return new TestCaseData(testString, expectedResult).SetName(testName);
                    }
                }
            }
        }
    }

    [Test, TestCaseSource(nameof(TestCaseData))]
    public void FirstWord_returns_first_word_from_text(string text, string expectedResult){
        Assert.That(text.FirstWord(), Is.EqualTo(expectedResult));
    }

    [Test]
    public void FirstWord_return_empty_string_from_empty_string(){
        Assert.That("".FirstWord(),Is.EqualTo(string.Empty));
    }

    [Test]
    public void FirstWord_return_empty_string_from_only_space_string(){
        Assert.That("      ".FirstWord(),Is.EqualTo(string.Empty));
    }

    private record CharToTest(string C, bool IsDelimiter, string Name);
}