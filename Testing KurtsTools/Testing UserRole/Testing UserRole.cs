

using System.Runtime.Versioning;
using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_UserRole;

[SupportedOSPlatform("windows")]
[TestFixture]
public class Testing_UserRole{
    [Test]
    public void IsAdministrator_can_be_called(){
      KurtsTools.IsAdministrator();
        Assert.Pass();
    }

}