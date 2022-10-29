using System.Diagnostics;
using System.Text;

namespace NSKurtsTools;

public static partial class KurtsTools{
    
    
    /*
     * https://stackoverflow.com/questions/139593/processstartinfo-hanging-on-waitforexit-why
     */
    public static CmdRunResult CmdRun(string command, string? arguments = null, int timeoutMilliSec = 10 * 1000){ // default timeout = 10 sec 
        using Process process = new(){
            StartInfo = new ProcessStartInfo{
                FileName = command,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            },
        };

        StringBuilder outputStringBuilder = new();
        StringBuilder errorStringBuilder = new();

        using AutoResetEvent outputWaitHandle = new(false);
        using AutoResetEvent errorWaitHandle = new(false);
        process.OutputDataReceived += (_, e) => {
            if (e.Data == null){
                // ReSharper disable once AccessToDisposedClosure
                outputWaitHandle.Set();
            }
            else{
                outputStringBuilder.AppendLine(e.Data);
            }
        };
        process.ErrorDataReceived += (_, e) => {
            if (e.Data == null){
                // ReSharper disable once AccessToDisposedClosure
                errorWaitHandle.Set();
            }
            else{
                errorStringBuilder.AppendLine(e.Data);
            }
        };

        try{
            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            if (process.WaitForExit(timeoutMilliSec) && outputWaitHandle.WaitOne(timeoutMilliSec) &&
                errorWaitHandle.WaitOne(timeoutMilliSec)){
                return new CmdRunResult(command, arguments, process.ExitCode, outputStringBuilder.ToString(),
                    errorStringBuilder.ToString());
            }

            if (!process.HasExited) process.Kill();
            return new CmdRunResult(command, arguments, CmdRunResult.ExitCodeTimeOut, "", "process timed out");
        } catch (Exception e){
            try{
                process.Kill();
            } catch (Exception){
                // ignore
            }

            return new CmdRunResult(command, arguments, CmdRunResult.ExitCodeFatalError, "", e.Message);
        }
    }
}