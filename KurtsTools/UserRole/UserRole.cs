using System.Runtime.Versioning;
using System.Security.Principal;

namespace NSKurtsTools;
[SupportedOSPlatform("windows")]
public static partial class KurtsTools{
    /*
 * https://stackoverflow.com/questions/3600322/check-if-the-current-user-is-administrator
 * https://stackoverflow.com/questions/11660184/c-sharp-check-if-run-as-administrator
 */
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static bool IsAdministrator(){
        using WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

}