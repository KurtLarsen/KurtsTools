using System.Runtime.Versioning;
using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_String_Extensions;

[TestFixture]
public class Testing_String_Extensions{
    [Test]
    [TestCase("\nABC\ndef",ExpectedResult="ABC",TestName = "<nl>ABC<nl>")]
    [TestCase("\nABC\rdef",ExpectedResult="ABC",TestName = "<nl>ABC<return>")]
    [TestCase("\nABC def",ExpectedResult="ABC",TestName = "<nl>ABC<space>")]
    [TestCase("\nABC\tdef",ExpectedResult="ABC",TestName = "<nl>ABC<tab>")]

    [TestCase("\rABC\ndef",ExpectedResult="ABC",TestName = "<return>ABC<nl>")]
    [TestCase("\rABC\rdef",ExpectedResult="ABC",TestName = "<return>ABC<return>")]
    [TestCase("\rABC def",ExpectedResult="ABC",TestName = "<return>ABC<space>")]
    [TestCase("\rABC\tdef",ExpectedResult="ABC",TestName = "<return>ABC<tab>")]

    [TestCase(" ABC\ndef",ExpectedResult="ABC",TestName = "<space>ABC<nl>")]
    [TestCase(" ABC\rdef",ExpectedResult="ABC",TestName = "<space>ABC<return>")]
    [TestCase(" ABC def",ExpectedResult="ABC",TestName = "<space>ABC<space>")]
    [TestCase(" ABC\tdef",ExpectedResult="ABC",TestName = "<space>ABC<tab>")]

    [TestCase("\tABC\ndef",ExpectedResult="ABC",TestName = "<tab>ABC<nl>")]
    [TestCase("\tABC\rdef",ExpectedResult="ABC",TestName = "<tab>ABC<return>")]
    [TestCase("\tABC def",ExpectedResult="ABC",TestName = "<tab>ABC<space>")]
    [TestCase("\tABC\tdef",ExpectedResult="ABC",TestName = "<tab>ABC<tab>")]

    [TestCase("ABC\ndef",ExpectedResult="ABC",TestName = "ABC<nl>")]
    [TestCase("ABC\rdef",ExpectedResult="ABC",TestName = "ABC<return>")]
    [TestCase("ABC def",ExpectedResult="ABC",TestName = "ABC<space>")]
    [TestCase("ABC\tdef",ExpectedResult="ABC",TestName = "ABC<tab>")]
    
    [TestCase(" \tabc",ExpectedResult = "abc")]
    public string FirstWord_returns_first_word_from_text(string text){
        return text.FirstWord();
    }


    [Test]
    public void FirstWord_return_empty_string_from_empty_string(){
        Assert.That("".FirstWord(),Is.EqualTo(string.Empty));
    }

    [Test]
    public void FirstWord_return_empty_string_from_only_space_string(){
        Assert.That("      ".FirstWord(),Is.EqualTo(string.Empty));
    }
    
    
    [Test]
    [TestCase("\nABC\ndef",ExpectedResult="def",TestName = "<nl>ABC<nl>def")]
    [TestCase("\nABC\ndef\n",ExpectedResult="def",TestName = "<nl>ABC<nl>def<nl>")]
    [TestCase("\nABC\ndef\r",ExpectedResult="def",TestName = "<nl>ABC<nl>def<return>")]
    [TestCase("\nABC\ndef ",ExpectedResult="def",TestName = "<nl>ABC<nl>def<space>")]
    [TestCase("\nABC\ndef\t",ExpectedResult="def",TestName = "<nl>ABC<nl>def<tab<")]

    
    [TestCase("\nABC\rdef",ExpectedResult="def",TestName = "<nl>      ABC<return>def")]
    [TestCase("\nABC def",ExpectedResult="def",TestName = "<nl>      ABC<space>def")]
    [TestCase("\nABC\tdef",ExpectedResult="def",TestName = "<nl>      ABC<tab>def")]

    [TestCase("\rABC\ndef",ExpectedResult="def",TestName = "<return>ABC<nl>def")]
    [TestCase("\rABC\rdef",ExpectedResult="def",TestName = "<return>ABC<return>def")]
    [TestCase("\rABC def",ExpectedResult="def",TestName = "<return>ABC<space>def")]
    [TestCase("\rABC\tdef",ExpectedResult="def",TestName = "<return>ABC<tab>def")]

    [TestCase(" ABC\ndef",ExpectedResult="def",TestName = "<space>ABC<nl>def")]
    [TestCase(" ABC\rdef",ExpectedResult="def",TestName = "<space>ABC<return>def")]
    [TestCase(" ABC def",ExpectedResult="def",TestName = "<space>ABC<space>def")]
    [TestCase(" ABC\tdef",ExpectedResult="def",TestName = "<space>ABC<tab>def")]

    [TestCase("\tABC\ndef",ExpectedResult="def",TestName = "<tab>ABC<nl>def")]
    [TestCase("\tABC\rdef",ExpectedResult="def",TestName = "<tab>ABC<return>def")]
    [TestCase("\tABC def",ExpectedResult="def",TestName = "<tab>ABC<space>def")]
    [TestCase("\tABC\tdef",ExpectedResult="def",TestName = "<tab>ABC<tab>def")]

    [TestCase("ABC\ndef",ExpectedResult="def",TestName = "ABC<nl>def")]
    [TestCase("ABC\rdef",ExpectedResult="def",TestName = "ABC<return>def")]
    [TestCase("ABC def",ExpectedResult="def",TestName = "ABC<space>def")]
    [TestCase("ABC\tdef",ExpectedResult="def",TestName = "ABC<tab>def")]
    public string SecondWord_returns_second_word_from_text(string text){
        return text.SecondWord();
    }

    [Test]
    [TestCase(@"c:\abc\def.ghi", ExpectedResult = ".ghi")]
    [TestCase(@"c:\abc\.ghi", ExpectedResult = @".ghi")]
    [TestCase(@"c:\abc\def ghi.jkl", ExpectedResult = @".jkl")]
    [TestCase(@"c:\abc\def", ExpectedResult = @"")]
    [TestCase(@"c:\abc\def ghi", ExpectedResult = @"")]
    [TestCase(@"c:\abc\", ExpectedResult = @"")]
    public string FileExtension_return_correct(string text){
        return text.FileExtension();
    }
    
    [Test]
    [TestCase(@"c:\abc\def.ghi", ExpectedResult = "def.ghi")]
    [TestCase(@"c:\abc\.ghi", ExpectedResult = @".ghi")]
    [TestCase(@"c:\abc\def ghi.jkl", ExpectedResult = @"def ghi.jkl")]
    [TestCase(@"c:\abc\def", ExpectedResult = @"def")]
    [TestCase(@"c:\abc\def ghi", ExpectedResult = @"def ghi")]
    [TestCase(@"c:\abc\", ExpectedResult = @"")]
    public string FileName_return_correct(string text){
        return text.FileName();
    }

    [Test]
    [TestCase(@"c:\abc\def.ghi", ExpectedResult = "def")]
    [TestCase(@"c:\abc\.ghi", ExpectedResult = @"")]
    [TestCase(@"c:\abc\def ghi.jkl", ExpectedResult = @"def ghi")]
    [TestCase(@"c:\abc\def", ExpectedResult = @"def")]
    [TestCase(@"c:\abc\def ghi", ExpectedResult = @"def ghi")]
    [TestCase(@"c:\abc\", ExpectedResult = @"")]
    public string FileBaseName_return_correct(string text){
        return text.FileBaseName();
    }
    
    [Test]
    [TestCase(@"c:\abc\def.ghi", ExpectedResult = @"c:\abc\")]
    [TestCase(@"c:\abc\.ghi", ExpectedResult = @"c:\abc\")]
    [TestCase(@"c:\abc\def ghi.jkl", ExpectedResult = @"c:\abc\")]
    [TestCase(@"c:\abc\def", ExpectedResult = @"c:\abc\")]
    [TestCase(@"c:\abc\def ghi", ExpectedResult = @"c:\abc\")]
    [TestCase(@"c:\abc\", ExpectedResult = @"c:\abc\")]
    public string Directory_return_correct(string text){
        return text.Directory();
    }

    [Test]
    [TestCase("abc def ghi",ExpectedResult = "def ghi")]
    [TestCase("  abc  \tdef  \tghi",ExpectedResult = "def  \tghi")]
    [TestCase("  abc  \t",ExpectedResult = "")]
    public string DeleteFirstWord_return_correct(string text){
        return text.DeleteFirstWord();
    }


    [Test,SupportedOSPlatform("windows")]
    [TestCase("{#}",ExpectedResult = true)]
    [TestCase("abc{#}",ExpectedResult = true)]
    [TestCase("abc",ExpectedResult = false)]
    public bool ContainsUniqueNamePlaceholder_works(string text){
        return text.ContainsUniqueNamePlaceholder();
    }
}