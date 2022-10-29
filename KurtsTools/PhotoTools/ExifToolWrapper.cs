using System.Diagnostics;
using System.Xml;

namespace NSKurtsTools;

public class ExifToolWrapper{
    private string _pathToExifToolExe;
    private string[]? _files = null;
    private int _timeOutMilliSeconds = 1000;

    public ExifToolWrapper(string pathToExifToolExe){
        _pathToExifToolExe = pathToExifToolExe;
        if (!File.Exists(pathToExifToolExe)) throw new FileNotFoundException(pathToExifToolExe);
    }

    public string[]? Files{
        get => _files;
    }

    public int TimeOut{
        get => _timeOutMilliSeconds;
        set => _timeOutMilliSeconds = value;
    }

    // public void SetFiles(params string[] files){
    //     _files = files;
    //     if (Files!.Length == 0) throw new Exception("Zero files");
    //     string currentDir = Directory.GetCurrentDirectory()+Path.DirectorySeparatorChar;
    //     for (int n = 0; n < files.Length; n++){
    //         string absolutePath = currentDir + _files[n];
    //         if (!File.Exists(absolutePath))
    //             throw new FileNotFoundException($"path = {_files[n]}\nabsPath = {absolutePath}");
    //         _files[n] = absolutePath;
    //     }
    // }

    public void SetKeyFilter(string nameSpace, string key){
        
    }

    public XmlDocument GetExif(params string[] files){
        // if (_files == null || _files.Length == 0) throw new Exception("No files to process");
        
        Process p = new(){
            StartInfo = new ProcessStartInfo{
                FileName =_pathToExifToolExe,
                Arguments = OptionBuilder("") +
                            FileBuilder(files),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            },
        };

        string? messageFromExitTool=null;
        
        bool processHasOutput = false;
        
        string stdOutBuffer = "";
        p.OutputDataReceived += (_, args) => {
            processHasOutput = true;
            if (string.IsNullOrEmpty(args.Data)) return;
            stdOutBuffer += args.Data;
        };

        p.ErrorDataReceived += (_, args) => {
            processHasOutput = true;
            if (string.IsNullOrEmpty(args.Data)) return;
            if (!string.IsNullOrEmpty(messageFromExitTool)) messageFromExitTool += " ";
            messageFromExitTool += args.Data.Trim();
        };

        p.Start();
        p.BeginErrorReadLine();
        p.BeginOutputReadLine();
        
        do{
            processHasOutput = false;
            p.WaitForExit(_timeOutMilliSeconds);
        } while (!p.HasExited && processHasOutput);

        p.WaitForExit();

        if (string.IsNullOrEmpty(stdOutBuffer)){
            throw new ExifException(ExifExceptionId.ExifToolDidNotReturnAnyOutput);
        }


        XmlDocument xmlDocument = new();
        try{
            xmlDocument.LoadXml(stdOutBuffer);
        } catch (Exception e){
            throw new ExifException(ExifExceptionId.ErrorLoadingOutputFromExifToolIntoXmlDocument,e.Message);
        }


        return xmlDocument;
    }

    private string FileBuilder(string[] files){
       return "\""+ string.Join("\" \"", files)+"\"";
    }

    private string OptionBuilder(string options){
        return " -X ";
    }
}