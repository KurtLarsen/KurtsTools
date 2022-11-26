


using System;
using System.IO;
using System.Runtime.Versioning;
using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools;

[TestFixture]
public class Testing_General_Tools{
    [Test]
    public void Testing_fn_UniqueName(){
        // todo: build test
    }

    [Test,SupportedOSPlatform("windows")]
    public void Testing_fn_RandomWord(){
        for (int n = 0; n < 100; n++){
            string result = KurtsTools.RandomWord();
            Assert.That(result.Length,Is.GreaterThanOrEqualTo(1),$"{nameof(result)}.Length of word \"{result}\"");
            Assert.That(result.Length,Is.LessThanOrEqualTo(10),$"{nameof(result)}.Length of word \"{result}\"");
            foreach (char t in result){
                Assert.That(t>='a');
                Assert.That(t<='z');
            }
        }
    }
}