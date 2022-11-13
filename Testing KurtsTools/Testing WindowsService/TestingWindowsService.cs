using System.Runtime.Versioning;
using System.ServiceProcess;
using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_WindowsService;

[SupportedOSPlatform("windows")]
[TestFixture]
public class TestingWindowsService{
    private const string ApacheRoot = @"Testing WindowsService/Apache/httpd-2.4.46-o111j-x64-vc15/Apache24";
    private const string DummyServiceName = "dummyApache2.4";

    private static string _httpdExe = null!;
    private static string _httpdConfigRelativeToApacheRoot = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp(){
        Assume.That(KurtsTools.IsAdministrator(),"User must be Administrator to run this test");
        
        Assume.That(ApacheRoot,Does.Exist,$"Directory not found: [{nameof(ApacheRoot)}] = {ApacheRoot}");
        _httpdExe = ApacheRoot + "/bin/httpd.exe";
        Assume.That(_httpdExe,Does.Exist,$"File not found: [{nameof(_httpdExe)}] = {_httpdExe}");
        
        _httpdConfigRelativeToApacheRoot = "conf/httpd.conf";
        Assume.That(ApacheRoot+"/"+ _httpdConfigRelativeToApacheRoot,Does.Exist);
    }

    [SetUp]
    public void SetUp(){
        StopAndRemoveDummyService();
    }

    [TearDown]
    public void TearDown(){
        StopAndRemoveDummyService();
    }

    private static void StopAndRemoveDummyService(){
        KurtsTools.CmdRun(_httpdExe, $"-k stop -n {DummyServiceName}");
        KurtsTools.CmdRun(_httpdExe, $"-k uninstall -n {DummyServiceName}");
        ServiceController? serviceController = KurtsTools.GetServiceControllerByNameOrDisplayName(DummyServiceName);
        Assume.That(serviceController,Is.Null);
    }
    
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
    public void service_can_be_removed(){
        /*
         * setup
         */
        const string dummyName = "dummyApache2.4";
        
        string installArguments = $"-k install -n {dummyName} -f \"{_httpdConfigRelativeToApacheRoot}\"";

        // KurtsTools.CmdRunResult cmdRunResult = null!;
        // if(KurtsTools.GetServiceControllerByNameOrDisplayName(dummyName)==null){
           KurtsTools.CmdRunResult cmdRunResult = KurtsTools.CmdRun(_httpdExe, installArguments);
        // }
        Assume.That(cmdRunResult.ExitCode, Is.Zero, cmdRunResult.ToString());
        
        Assume.That(KurtsTools.GetServiceControllerByNameOrDisplayName(dummyName)!=null);
        
        /*
         * test
         */

       bool result= KurtsTools.StopAndDeleteService(dummyName);
       Assert.That(result,Is.True);
       Assert.That(KurtsTools.GetServiceControllerByNameOrDisplayName(dummyName),Is.Null);

    }
}