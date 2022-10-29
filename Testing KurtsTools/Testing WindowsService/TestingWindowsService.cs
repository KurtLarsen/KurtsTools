using System.Runtime.Versioning;
using System.ServiceProcess;
using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_WindowsService;

[SupportedOSPlatform("windows")]
[TestFixture]
public class TestingWindowsService{
    [Test]
    public void existing_service_is_found(){
        const string serviceName = "UserManager";
        const string displayName = "Brugerstyring";

        ServiceController? result = KurtsTools.GetServiceControllerByNameOrDisplayName(serviceName);
        Assert.That(result,Is.Not.Null);
        
        result =KurtsTools.GetServiceControllerByNameOrDisplayName(displayName);
        Assert.That(result,Is.Not.Null);

    }

    [Test]
    public void non_existing_service_is_found(){
        const string serviceName = "dummyServiceName";
        const string displayName = "dummyDisplayName";

        ServiceController? result = KurtsTools.GetServiceControllerByNameOrDisplayName(serviceName);
        Assert.That(result,Is.Null);
        
        result =KurtsTools.GetServiceControllerByNameOrDisplayName(displayName);
        Assert.That(result,Is.Null);
    }

    [Test]
    public void dummyService_can_be_removed(){
        Assume.That(KurtsTools.IsAdministrator(),"User must be Administrator");
        /*
         * setup
         */
        const string dummyName = "dummyApache2.4";
        const string httpdCommand = @"C:\Apache\httpd-2.4.46-o111j-x64-vc15\Apache24\bin\httpd.exe";
        const string installArguments = $"-k install -n {dummyName}";

        KurtsTools.CmdRunResult cmdRunResult = null!;
        if(KurtsTools.GetServiceControllerByNameOrDisplayName(dummyName)==null){
            cmdRunResult = KurtsTools.CmdRun(httpdCommand, installArguments);
        }
        Assume.That(cmdRunResult.ExitCode,Is.Zero,$"ExitCode in {cmdRunResult}");
        Assume.That(KurtsTools.GetServiceControllerByNameOrDisplayName(dummyName)!=null);
        
        /*
         * test
         */

       bool result= KurtsTools.StopAndDeleteService(dummyName);
       Assert.That(result,Is.True);
       Assert.That(KurtsTools.GetServiceControllerByNameOrDisplayName(dummyName),Is.Null);

    }
}