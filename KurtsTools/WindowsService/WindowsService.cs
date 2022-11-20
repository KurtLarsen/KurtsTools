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
        if (serviceControllerToBeUninstalled.Status != ServiceControllerStatus.Stopped){
            if (!serviceControllerToBeUninstalled.CanStop) throw new Exception("service can not stop");
            serviceControllerToBeUninstalled.Stop();
        }

        CmdRunResult result = CmdRun(new CmdRunSetUp{ Command = "SC", Arguments = $"DELETE {serviceControllerToBeUninstalled.ServiceName}" });

        if (result.ExitCode != 0)
            throw new Exception(
                $"Error deleting service {serviceControllerToBeUninstalled.ServiceName}\n{result}");
        

        return GetServiceControllerByNameOrDisplayName(serviceNameOrDisplayName) == null;
    }
}