using System.Configuration.Install;
using System.ServiceProcess;

namespace NSKurtsTools;

public static partial class KurtsTools{

    public static ServiceController? GetServiceControllerByNameOrDisplayName(string name){
        ServiceController[]? scServices = ServiceController.GetServices();
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (ServiceController serviceController in scServices){
            if (serviceController.ServiceName.Equals(name)||serviceController.DisplayName.Equals(name)) return serviceController;
        }

        return null;
    }

    public static bool StopAndDeleteService(string serviceNameOrDisplayName){
        ServiceController? serviceControllerToBeUninstalled = GetServiceControllerByNameOrDisplayName(serviceNameOrDisplayName);
        if (serviceControllerToBeUninstalled == null) return true;
        
        // https://stackoverflow.com/questions/12201365/programmatically-remove-a-service-using-c-sharp
        ServiceInstaller serviceInstallerObj = new(); 
        // todo: what does Context do?
        // todo: what does "<<log file path>>" mean?
        InstallContext context = new("<<log file path>>", null); 
        serviceInstallerObj.Context = context;
        serviceInstallerObj.ServiceName = serviceControllerToBeUninstalled.ServiceName; 
        serviceInstallerObj.Uninstall(null); 

        serviceControllerToBeUninstalled = GetServiceControllerByNameOrDisplayName(serviceNameOrDisplayName);
        return serviceControllerToBeUninstalled == null;
    }
}