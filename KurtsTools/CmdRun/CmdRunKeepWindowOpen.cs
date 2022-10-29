using System.Diagnostics;

namespace NSKurtsTools;

public static partial class KurtsTools{
    public static void CmdRunKeepWindowOpen(string command, string? arguments = null){

        try{
            Process myProcess = new();
            myProcess.StartInfo = new ProcessStartInfo{
                UseShellExecute = true,
                FileName = command,
                Arguments = arguments,
                CreateNoWindow = false,
            };
            myProcess.Start();
        } catch (Exception){
            // ignored
        }
    }
}