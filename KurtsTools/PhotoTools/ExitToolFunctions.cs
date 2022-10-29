using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace NSKurtsTools;

public static partial class KurtsTools{
    
    //todo: change function so it only returns ONE value: a xml document that includes the message fra ExifTool
    //todo: create ExifTool wrapper class and encapsulate function GetExifDataAsXml inside it 
    /**
     * <example><code>   
        private const string ExifToolExe = @"C:\Program Files\exiftool\exiftool.exe";
        
        ArgumentGivenToGetExifDataAsXml p = new(){ 
            PathToExifToolExe = ExifToolExe, 
            Files = new[]{ "7P2A1833.CR3" } 
        };
        
        (XmlDocument xmlDocument, string? messageFromExitTool)  = KurtsTools.GetExifDataAsXml(p);
        </code></example>
     */
    public static (XmlDocument xmlDocument, string? messageFromExitTool) GetExifDataAsXml(ArgumentGivenToGetExifDataAsXml argumentGivenToGetExifDataAsXml){
        if (argumentGivenToGetExifDataAsXml.PathToExifToolExe == null)
            throw new ExifException(ExifExceptionId.PathToExifToolExeIsNull);
        if (!File.Exists(argumentGivenToGetExifDataAsXml.PathToExifToolExe))
            throw new ExifException(ExifExceptionId.PathToExifToolExeNotFound,argumentGivenToGetExifDataAsXml.PathToExifToolExe);
        if (argumentGivenToGetExifDataAsXml.Files == null)
            throw new ExifException(ExifExceptionId.FileArrayIsNull);
        if ( argumentGivenToGetExifDataAsXml.Files.Length == 0)
            throw new ExifException(ExifExceptionId.FileArrayIsEmpty);
        
        Process p = new(){
            StartInfo = new ProcessStartInfo{
                FileName = argumentGivenToGetExifDataAsXml.PathToExifToolExe,
                Arguments = OptionBuilder(argumentGivenToGetExifDataAsXml.Options) +
                            FileBuilder(argumentGivenToGetExifDataAsXml.Files),
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
            p.WaitForExit(argumentGivenToGetExifDataAsXml.TimeoutMilliSec); // continues after max. TimeoutMilliSec.
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

public class ExifException : Exception{
    private readonly ExifExceptionId _id;

    public ExifException(ExifExceptionId exifExceptionId){
        _id = exifExceptionId;
    }

    public ExifException(ExifExceptionId exifExceptionId, string data):base(data){
        _id = exifExceptionId;
    }

    // ReSharper disable once ConvertToAutoProperty
    public ExifExceptionId Id => _id;
}

public enum ExifExceptionId{
    PathToExifToolExeIsNull=1,
    PathToExifToolExeNotFound=2,
    FileArrayIsNull=3,
    FileArrayIsEmpty=4,
    ExifToolDidNotReturnAnyOutput=5,
    ErrorLoadingOutputFromExifToolIntoXmlDocument=6,
}

[SuppressMessage("ReSharper", "ConvertToConstant.Global")]
[SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
public record ArgumentGivenToGetExifDataAsXml{
    public string? PathToExifToolExe;
    public string[]? Files;
    public string? Options;
    public int TimeoutMilliSec = 5000; // 5 seconds
    
}

