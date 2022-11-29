using System.Management;
using System.Runtime.Versioning;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_WindowsService;

[TestFixture]
public class Testing_WindowsService2{
    [Test,SupportedOSPlatform("windows"),Ignore("this test hangs")]
    public void Something(){
        // ApacheServiceControllerWrapper apacheServiceControllerWrapper = new("Apache2.4");
        // Assert.That(apacheServiceControllerWrapper.GetStatus(),Is.EqualTo(ExtendedServiceControllerStatus.Running));
        RunManagementEventWatcherForWindowsServices();
    }


    [SupportedOSPlatform("windows")]
    private static void RunManagementEventWatcherForWindowsServices(){
        EventQuery eventQuery = new(){
            QueryString = "SELECT * FROM __InstanceModificationEvent within 2 WHERE targetinstance isa 'Win32_Service'",
        };
        
        ManagementEventWatcher demoWatcher = new(eventQuery);
        demoWatcher.Options.Timeout = new TimeSpan(1, 0, 0);
        
        Console.WriteLine("Perform the appropriate change in a Windows service according to your query");
        
        ManagementBaseObject nextEvent = demoWatcher.WaitForNextEvent();
        ManagementBaseObject targetInstance = (ManagementBaseObject)nextEvent["targetinstance"];
        PropertyDataCollection props = targetInstance.Properties;
        foreach (PropertyData prop in props){
            Console.WriteLine("Property name: {0}, property value: {1}", prop.Name, prop.Value);
        }

        demoWatcher.Stop();
    }
}

// internal enum ExtendedServiceControllerStatus{
//     ContinuePenning,
//     Paused,
//     PausePenning,
//     Running,
//     StartPenning,
//     Stopped,
//     StopPenning,
//     NotInstalled,
// }