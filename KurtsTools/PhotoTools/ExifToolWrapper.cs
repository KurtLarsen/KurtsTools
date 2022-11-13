using System.Runtime.Versioning;
using System.Xml;
using static NSKurtsTools.KurtsTools;

// ReSharper disable ConvertToAutoPropertyWhenPossible
// ReSharper disable LoopCanBeConvertedToQuery

// ReSharper disable ConvertToAutoProperty
// ReSharper disable ArrangeAccessorOwnerBody

namespace NSKurtsTools;

[SupportedOSPlatform("windows")]
public class ExifToolWrapper{
    private readonly string _pathToExifToolExe;

    // private int _defaultTimeoutMilliSeconds = 1000;
    // bool outputDataHandlerIsRunning=false;

    // constructor
    public ExifToolWrapper(string pathToExifToolExe){
        _pathToExifToolExe = pathToExifToolExe;
        if (!File.Exists(pathToExifToolExe)) throw new FileNotFoundException(pathToExifToolExe);
    }


    // ReSharper disable once MemberCanBePrivate.Global
    // public int DefaultTimeoutMilliSeconds{
    //     get => _defaultTimeoutMilliSeconds;
    //     // ReSharper disable once UnusedMember.Global
    //     set => _defaultTimeoutMilliSeconds = value;
    // }


    public ResultOfGetExif GetExif(GetExifParams getExifParams){
        CmdRunResult x = CmdRun(_pathToExifToolExe, $" -X {getExifParams.InputFiles.ToArgumentString()}");
        
        ResultOfGetExif resultOfGetExifResult = new(){
            ExitToolReturnCode = x.ExitCode,
            ExifToolErrOut = x.ErrOut,
            ExifToolStdOut = x.StdOut,
            ListOfSingleFiles = new List<SingleFileExifData>(),
        };

        XmlDocument xmlDocument = new();
        
        try{
            xmlDocument.LoadXml(x.StdOut);
        } catch (Exception? e){
            resultOfGetExifResult.ConvertingToXmlException = e;
            return resultOfGetExifResult;
        }
        
        XmlElement? root = xmlDocument.DocumentElement;
        if (root == null){
            resultOfGetExifResult.ConvertingToXmlException = new Exception("Root element not found in XML");
            return resultOfGetExifResult;
        }

        foreach (XmlNode singleFileXmlExifData in root.ChildNodes){
            SingleFileExifData l = new();
            resultOfGetExifResult.ListOfSingleFiles.Add(l);
            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach (XmlElement xmlElement in singleFileXmlExifData){
                ExifItem e = new(){
                    Group = xmlElement.Prefix,
                    Key = xmlElement.LocalName,
                    Value = xmlElement.InnerText,
                };
                l.Add(e);
            }
        }

        

        
        return resultOfGetExifResult;
    }

    // [Obsolete("Use GetExit() instead")]
    // public ExifData[] GetExif_old(params string[] files){
    //     return GetExif_old(DefaultTimeoutMilliSeconds, files);
    // }


    // [Obsolete("Use GetExit() instead")]
    // public ExifData[] GetExif_old(int timeoutMilliSeconds, params string[] files){
    //     XmlDocument xmlDocument = GetExifDataAsXmlDocument(files, timeoutMilliSeconds);
    //
    //     return XmlDocumentWithDataFromMultipleFilesToArrayOfExifData(xmlDocument);
    // }
}

public class GetExifParams{
    public readonly InputFiles InputFiles = new();
}

public class InputFiles : List<string>{
    public string ToArgumentString(){
        return this.Aggregate("", (current, inputFile) => current + " \"" + inputFile + "\"");
    }
}

public class ResultOfGetExif{
    private readonly int _exitToolReturnCode = 99;
    private readonly string _exifToolErrOut = "";
    private readonly string _exifToolStdOut = "";
    private Exception? _convertingToXmlException;
    
    // private ExifData[][] _exifData;
    private readonly List<SingleFileExifData> _listOfSingleFiles=new();

    public int ExitToolReturnCode{
        get => _exitToolReturnCode;
        init => _exitToolReturnCode = value;
    }

    // public ExifData[][] ExifData{
        // get{ return _exifData; }
        // set{ _exifData = value; }
    // }

    public string ExifToolErrOut{
        get{ return _exifToolErrOut; }
        init{ _exifToolErrOut = value; }
    }

    public string ExifToolStdOut{
        get => _exifToolStdOut;
        init => _exifToolStdOut = value;
    }

    public Exception? ConvertingToXmlException{
        get => _convertingToXmlException;
        set => _convertingToXmlException = value;
    }

    public List<SingleFileExifData> ListOfSingleFiles{
        get => _listOfSingleFiles;
        init => _listOfSingleFiles = value;
    }
}

public class SingleFileExifData:List<ExifItem>{
    public override string ToString(){
        string s = "";
        foreach (ExifItem exifItem in this){
            s += exifItem.ToString();
            if (s.Length <= 100) continue;
            s += "...";
            break;
        }

        return s;
    }
}

public class ExifItem{
    public string Group=string.Empty;
    public string Key=string.Empty;
    public string Value=string.Empty;
    public ExifItem(){ }

    public ExifItem(string group, string key, string value){
        Group = group;
        Key = key;
        Value = value;
    }

    public override string ToString(){
        return $"<{Group},{Key},{Value}>";
    }
}