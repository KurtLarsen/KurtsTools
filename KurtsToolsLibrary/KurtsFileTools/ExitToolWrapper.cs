using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace KurtsToolsLibrary.KurtsFileTools;

public static partial class KurtsFileTools{
    public static (XmlDocument xmlDocument, string? messageFromExitTool) ExifToolWrapper(ExifToolWrapperParameters exifToolWrapperParameters){
        if (exifToolWrapperParameters.PathToExifToolExe == null)
            throw new ExifToolWrapperException(ExifToolExceptionId.PathToExifToolExeIsNull);
        if (!File.Exists(exifToolWrapperParameters.PathToExifToolExe))
            throw new ExifToolWrapperException(ExifToolExceptionId.PathToExifToolExeNotFound,exifToolWrapperParameters.PathToExifToolExe);
        if (exifToolWrapperParameters.Files == null)
            throw new ExifToolWrapperException(ExifToolExceptionId.FileArrayIsNull);
        if ( exifToolWrapperParameters.Files.Length == 0)
            throw new ExifToolWrapperException(ExifToolExceptionId.FileArrayIsEmpty);
        
        Process p = new(){
            StartInfo = new ProcessStartInfo{
                FileName = exifToolWrapperParameters.PathToExifToolExe,
                Arguments = OptionBuilder(exifToolWrapperParameters.Options) +
                            FileBuilder(exifToolWrapperParameters.Files),
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
            p.WaitForExit(exifToolWrapperParameters.TimeoutMilliSec); // continues after max. TimeoutMilliSec.
        } while (!p.HasExited && processHasOutput);

        p.WaitForExit();

        if (string.IsNullOrEmpty(stdOutBuffer)){
            throw new ExifToolWrapperException(ExifToolExceptionId.ExifToolDidNotReturnAnyOutput);
        }


        XmlDocument xmlDocument = new();
        try{
            xmlDocument.LoadXml(stdOutBuffer);
        } catch (Exception e){
            throw new ExifToolWrapperException(ExifToolExceptionId.ErrorLoadingOutputFromExifToolIntoXmlDocument,e.Message);
        }


        return (xmlDocument, messageFromExitTool);
    }

    private static string OptionBuilder(string? options){
        if (options == null || options.Trim().Length == 0) return "-X ";
        if (!(options + " ").Contains("-X ")) options += " -X";
        return options.Trim() + " ";
    }

    private static string FileBuilder(IEnumerable<string> files){
        string s = "";
        foreach (string file in files){
            if (s.Length > 0) s += " ";
            s += '"' + file.Trim('\\') + '"';
        }

        return s;
    }
}

public class ExifToolWrapperException : Exception{
    private readonly ExifToolExceptionId _exceptionId;

    public ExifToolWrapperException(ExifToolExceptionId id){
        _exceptionId = id;
    }

    public ExifToolWrapperException(ExifToolExceptionId id, string data):base(data){
        _exceptionId = id;
    }

    // ReSharper disable once ConvertToAutoProperty
    public ExifToolExceptionId ExceptionId => _exceptionId;
}

public enum ExifToolExceptionId{
    PathToExifToolExeIsNull=1,
    PathToExifToolExeNotFound=2,
    FileArrayIsNull=3,
    FileArrayIsEmpty=4,
    ExifToolDidNotReturnAnyOutput=5,
    ErrorLoadingOutputFromExifToolIntoXmlDocument=6,
}

[SuppressMessage("ReSharper", "ConvertToConstant.Global")]
[SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
public record ExifToolWrapperParameters{
    public string? PathToExifToolExe;
    public string[]? Files;
    public string? Options;
    public int TimeoutMilliSec = 5000; // 5 seconds
    
}

