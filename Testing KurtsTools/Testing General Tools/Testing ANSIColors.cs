using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools;

[TestFixture]
public class TestingAnsiColors{
    

    [Test]
    public void ColorTest(){
     const string s = $"{Ansi.FrontRed}abc{Ansi.Reset}{Ansi.BackMagenta}{Ansi.FrontWhite}def{Ansi.Reset}";
     Console.WriteLine(s);
     Assert.Pass("ok");
    }

}