using System.Runtime.Versioning;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_DirectoryTools;
[SupportedOSPlatform("windows")]
[TestFixture]
public class Testing_NewTempDirectory{

    [Test]
    public void test_of_something(){
        string tempDir = NSKurtsTools.KurtsTools.NewTempDirectory();
        Assert.That(tempDir, Does.Exist);

        NSKurtsTools.KurtsTools.DeleteDirectory(pathToDirectory: tempDir);
        
        Assert.That(tempDir,Does.Not.Exist);
    }
}