using System.Runtime.Versioning;
using System.ServiceProcess;
using NSKurtsTools;
using NUnit.Framework;
using static NSKurtsTools.KurtsTools;

namespace NSTesting_KurtsTools.Testing_WindowsService;

[SupportedOSPlatform("windows")]
[TestFixture]
public class TestingWindowsService{
    private const string ApacheRoot = @"Testing WindowsService/Apache/httpd-2.4.46-o111j-x64-vc15/Apache24";

    private static string _dummyServiceName = null!;

    private static string _httpdExe = null!;
    private static string _httpdConfigRelativeToApacheRoot = null!;
    private static string _tmpDirectory = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp(){
        Assume.That(IsAdministrator(), "User must be Administrator to run this test");

        Assume.That(ApacheRoot, Does.Exist, $"Directory not found: [{nameof(ApacheRoot)}] = {ApacheRoot}");

        _httpdExe = ApacheRoot + "/bin/httpd.exe";
        Assume.That(_httpdExe, Does.Exist, $"File not found: [{nameof(_httpdExe)}] = {_httpdExe}");

        _tmpDirectory = NewTempDirectory();
        ApacheConfigBuilderProperties minimalProperties = new(){ Listen = "*:81",ServerName = "localhost:81",ErrorLog = $"{_tmpDirectory}Error.log"};
        string[] minimalConfigLines = ApacheConfigBuilder(minimalProperties);

        _httpdConfigRelativeToApacheRoot = _tmpDirectory + "minimal.conf";
        File.WriteAllLines(_httpdConfigRelativeToApacheRoot, minimalConfigLines);


        do{
            _dummyServiceName = "_DummyService_" + Guid.NewGuid();
        } while (GetServiceControllerByNameOrDisplayName(_dummyServiceName) != null);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown(){
        DeleteDirectory(_tmpDirectory);
    }

    [SetUp]
    public void SetUp(){
        StopAndRemoveDummyService();
    }

    [TearDown]
    public void TearDown(){
        StopAndRemoveDummyService();
    }


    [Test]
    public void testing_fn_GetServiceControllerByNameOrDisplayName_on_existing_service(){
        const string serviceName = "UserManager";
        const string displayName = "Brugerstyring";

        ServiceController? result = GetServiceControllerByNameOrDisplayName(serviceName);
        Assert.That(result, Is.Not.Null);

        result = GetServiceControllerByNameOrDisplayName(displayName);
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void testing_fn_GetServiceControllerByNameOrDisplayName_on_not_existing_service(){
        const string serviceName = "notExistingServiceName";
        const string displayName = "notExistingDisplayName";

        ServiceController? result = GetServiceControllerByNameOrDisplayName(serviceName);
        Assert.That(result, Is.Null);

        result = GetServiceControllerByNameOrDisplayName(displayName);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void testing_fn_StopAndDeleteService(){
        /*
         * setup
         */

        string installArguments = $"-k install -n {_dummyServiceName} -f \"{_httpdConfigRelativeToApacheRoot}\"";
        // CmdRunResult cmdRunResult = CmdRun(_httpdExe, installArguments);
        CmdRunResult cmdRunResult = CmdRun(new CmdRunSetUp
            { Command = _httpdExe, Arguments = installArguments, WorkingDirectory = ApacheRoot });
        Assume.That(cmdRunResult.ExitCode, Is.Zero,
            $"Fail on {nameof(cmdRunResult)}.{nameof(cmdRunResult.ExitCode)} = <{cmdRunResult.ExitCode}>\n" +
            cmdRunResult);

        Assume.That(GetServiceControllerByNameOrDisplayName(_dummyServiceName) != null);

        /*
         * test
         */

        bool result = StopAndDeleteService(_dummyServiceName);
        Assert.That(result, Is.True);
        Assert.That(GetServiceControllerByNameOrDisplayName(_dummyServiceName), Is.Null);
    }

    private static void StopAndRemoveDummyService(){
        CmdRun(_httpdExe, $"-k stop -n {_dummyServiceName}");
        CmdRun(_httpdExe, $"-k uninstall -n {_dummyServiceName}");
        ServiceController? serviceController = GetServiceControllerByNameOrDisplayName(_dummyServiceName);
        Assume.That(serviceController, Is.Null);
    }

    private static string[] ApacheConfigBuilder(ApacheConfigBuilderProperties apacheConfigBuilderProperties){
        List<string> lines = new();
        if(apacheConfigBuilderProperties.Listen!=null) lines.Add($"Listen {apacheConfigBuilderProperties.Listen}");
        if(apacheConfigBuilderProperties.ServerName!=null) lines.Add($"ServerName {apacheConfigBuilderProperties.ServerName}");
        if(apacheConfigBuilderProperties.ErrorLog!=null) lines.Add($"ErrorLog {apacheConfigBuilderProperties.ErrorLog}");
        return lines.ToArray();
    }
}

internal record ApacheConfigBuilderProperties{
    public string? Listen;
    public string? ServerName;
    public string? ErrorLog;
}