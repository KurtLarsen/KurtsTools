using KurtsToolsLibrary.FileTools;
using NUnit.Framework;

namespace Testing_KurtsToolsLibrary.Testing_File_Tools;

[TestFixture]
public class Testing_NewTempDirectory{

    [Test]
    public void test_of_something(){
        string tempDir = FileTools.NewTempDirectory();
        Assert.That(tempDir, Does.Exist);

        FileTools.DeleteDirectory(tempDir);
        
        Assert.That(tempDir,Does.Not.Exist);
    }
}