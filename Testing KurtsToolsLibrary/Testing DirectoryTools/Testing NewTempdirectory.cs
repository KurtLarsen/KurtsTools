using KurtsToolsLibrary.DirectoryTools;
using NUnit.Framework;

namespace Testing_KurtsToolsLibrary.Testing_DirectoryTools;

[TestFixture]
public class Testing_NewTempDirectory{

    [Test]
    public void test_of_something(){
        string tempDir = DirectoryTools.NewTempDirectory();
        Assert.That(tempDir, Does.Exist);

        DirectoryTools.DeleteDirectory(tempDir);
        
        Assert.That(tempDir,Does.Not.Exist);
    }
}