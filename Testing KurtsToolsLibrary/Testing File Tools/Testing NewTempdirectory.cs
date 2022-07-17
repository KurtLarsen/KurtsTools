using NUnit.Framework;
using KurtsToolsLibrary.KurtsFileTools;

namespace Testing_KurtsToolsLibrary.Testing_File_Tools;

[TestFixture]
public class Testing_NewTempDirectory{

    [Test]
    public void test_of_something(){
        string tempDir = KurtsFileTools.NewTempDirectory();
        Assert.That(tempDir, Does.Exist);

        KurtsFileTools.DeleteDirectory(tempDir);
        
        Assert.That(tempDir,Does.Not.Exist);
    }
}