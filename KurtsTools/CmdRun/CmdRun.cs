using System.Diagnostics;
using System.Text;

namespace NSKurtsTools;

public static partial class KurtsTools{
    public static CmdRunResult CmdRun(string command, string? arguments = null){
        CmdRunSetUp setup = new(){ Command = command };
        if (arguments != null) setup.Arguments = arguments;
        return CmdRun(setup);
    }


    /*
     * https://stackoverflow.com/questions/139593/processstartinfo-hanging-on-waitforexit-why
     */
    public static CmdRunResult CmdRun(CmdRunSetUp cmdRunSetUp){

        cmdRunSetUp.WorkingDirectory ??= Directory.GetCurrentDirectory();
        
        ProcessStartInfo startInfo = new(){
            FileName = $"\"{cmdRunSetUp.Command}\"",
            Arguments = cmdRunSetUp.Arguments,
            WorkingDirectory = cmdRunSetUp.WorkingDirectory,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };

        using Process process = new(){ StartInfo = startInfo };

        StringBuilder outputStringBuilder = new();
        StringBuilder errorStringBuilder = new();

        using AutoResetEvent outputWaitHandle = new(false);
        using AutoResetEvent errorWaitHandle = new(false);
        process.OutputDataReceived += (_, e) => {
            if (e.Data == null){
                // ReSharper disable once AccessToDisposedClosure
                outputWaitHandle.Set(); // Sets the state of the event to signaled, allowing one or more waiting threads to proceed.
            }
            else{
                outputStringBuilder.AppendLine(e.Data);
            }
        };
        process.ErrorDataReceived += (_, e) => {
            if (e.Data == null){
                // ReSharper disable once AccessToDisposedClosure
                errorWaitHandle.Set();  // Sets the state of the event to signaled, allowing one or more waiting threads to proceed.
            }
            else{
                errorStringBuilder.AppendLine(e.Data);
            }
        };

        try{
            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            if (process.WaitForExit(cmdRunSetUp.TimeOutMilliSec) &&
                outputWaitHandle.WaitOne(cmdRunSetUp.TimeOutMilliSec) &&
                errorWaitHandle.WaitOne(cmdRunSetUp.TimeOutMilliSec)){
                return new CmdRunResult(cmdRunSetUp, process.ExitCode, outputStringBuilder.ToString(),
                    errorStringBuilder.ToString());
            }

            if (!process.HasExited) process.Kill();
            return new CmdRunResult(cmdRunSetUp, CmdRunResult.ExitCodeTimeOut, "", "process timed out");
        } catch (Exception e){
            try{
                process.Kill();
            } catch (Exception){
                // ignore
            }

            return new CmdRunResult(cmdRunSetUp, CmdRunResult.ExitCodeFatalError, "", e.Message);
        }
    }
}