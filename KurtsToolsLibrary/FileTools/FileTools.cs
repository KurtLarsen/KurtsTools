using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Xml;
using DirectoryInfo = System.IO.DirectoryInfo;

namespace KurtsToolsLibrary.FileTools;

public static class FileTools{
    public static string DirectoryStructureIsEqual(string path, XmlDocument directoryStructure){
        return DirectoryStructureIsEqual(path, directoryStructure.DocumentElement!);
    }

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public static string DirectoryStructureIsEqual(string path, XmlElement directoryStructure){
        DirectoryInfo di = new(path);

        if (!di.Exists) return $"Path not found: {path}";

        return DirectoryIsEqualRecursive(path, directoryStructure);
    }

    [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    private static string DirectoryIsEqualRecursive(string path, XmlElement directoryStructure){
        DirectoryInfo di = new(path);
        List<FileInfo> listOfFileInfos = new(di.GetFiles());
        List<DirectoryInfo> listOfSubdirectoriesInDirectory = new(di.GetDirectories());
        foreach (XmlElement directoryChild in directoryStructure.ChildNodes){
            switch (directoryChild.Name){
                case "folder":
                case "directory":{
                    string directoryName = directoryChild.GetAttribute("name");
                    int indexOfMatchingDirectory =
                        listOfSubdirectoriesInDirectory.FindIndex(x => x.Name == directoryName);
                    if (indexOfMatchingDirectory == -1)
                        return $"Directory \"{directoryName}\" not found in path {path}";
                    listOfSubdirectoriesInDirectory.RemoveAt(indexOfMatchingDirectory);
                    string result = DirectoryIsEqualRecursive(path + Path.DirectorySeparatorChar + directoryName,
                        directoryChild);
                    if (result.Length != 0) return result;
                    break;
                }

                case "file":{
                    string fileName = directoryChild.GetAttribute("name");
                    int indexOfMatchingFile = listOfFileInfos.FindIndex(x => x.Name == fileName);
                    if (indexOfMatchingFile == -1) return $"File \"{fileName}\" not found in path {path}";
                    listOfFileInfos.RemoveAt(indexOfMatchingFile);
                    break;
                }

                default: throw new Exception($"Unknown xml element: <{directoryChild.Name}>");
            }
        }

        if (listOfSubdirectoriesInDirectory.Count > 0)
            return $"Extra subdirectory \"{listOfSubdirectoriesInDirectory.First().Name}\" found in path {path}";
        if (listOfFileInfos.Count > 0) return $"Extra file \"{listOfFileInfos.First().Name}\" found in path {path}";
        return "";
    }

    /*
     * source: https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
     */
    /**
      <param name="fromDir">Path to existing directory</param>
      <param name="toDir">Path to existing or non existing directory</param>
      <param name="recursive">Include subdirectories and there content</param>
     */
    public static void CopyDirectory(string fromDir, string toDir, bool recursive = true){
        // Get information about the source directory
        DirectoryInfo fromDirInfo = new(fromDir);

        // Check if the source directory exists
        if (!fromDirInfo.Exists)
            throw new DirectoryNotFoundException($"Source directory not found: {fromDirInfo.FullName}");

        // Cache directories before we start copying
        DirectoryInfo[] fromSubDirInfos = fromDirInfo.GetDirectories();

        // Create the destination directory
        if (!Directory.Exists(toDir))
            Directory.CreateDirectory(toDir);

        // Get the files in the source directory and copy to the destination directory
        foreach (FileInfo file in fromDirInfo.GetFiles()){
            string targetFilePath = Path.Combine(toDir, file.Name);
            file.CopyTo(targetFilePath);
        }

        if (!recursive) return;

        foreach (DirectoryInfo subDir in fromSubDirInfos){
            string newDestinationDir = Path.Combine(toDir, subDir.Name);
            // ReSharper disable once RedundantArgumentDefaultValue
            CopyDirectory(subDir.FullName, newDestinationDir, true);
        }
    }


    /**
       Creates a new empty directory in the system temp path
       <returns>String with full path to newly created directory</returns>
     */
    public static string NewTempDirectory(){
        return Path.GetTempPath() +
               Directory.CreateDirectory(Path.GetTempPath() + Guid.NewGuid())
                   .Name + Path.DirectorySeparatorChar;
    }

    public static void DeleteDirectory(string pathToDirectory){
        DirectoryInfo directoryInfo = new(pathToDirectory);
        if (!directoryInfo.Exists)
            return;
        if ((directoryInfo.Attributes & FileAttributes.ReadOnly) != 0)
            directoryInfo.Attributes &= ~FileAttributes.ReadOnly;
        foreach (FileInfo enumerateFile in directoryInfo.EnumerateFiles()){
            if (enumerateFile.IsReadOnly)
                enumerateFile.Attributes &= ~FileAttributes.ReadOnly;
            enumerateFile.Delete();
        }

        foreach (DirectoryInfo enumerateDirectory in directoryInfo.EnumerateDirectories())
            DeleteDirectory(enumerateDirectory.FullName);
        directoryInfo.Delete();
    }

    public static byte[] GetMd5Hash(string path){
        using MD5 md5 = MD5.Create();
        using FileStream inputStream = File.OpenRead(path);
        return md5.ComputeHash(inputStream);
    }


    /**
    <summary>Adds files and subdirectories to the destination directory</summary>
    <remarks>&lt;folder&gt; can be used instead of &lt;directory&gt;</remarks>
    <param name="destinationDirectory">The directory where the file structure is created. This directory must exist</param>
    <param name="fileStructure">A xmlDocument in the form:
        <code>
            &lt;root&gt;
                &lt;directory name='subdir1'&gt;
                    &lt;file name='file1.txt' /&gt;
                    &lt;file name='file2.txt' /&gt;
                &lt;/directory&gt;
                &lt;directory name='subdir2' /&gt;
                &lt;file name='file3.txt' /&gt;
                &lt;file name='file4.txt'&gt;
                    content of file
                &lt;/file&gt;
            &lt;/root&gt;
         </code>
    </param>
     */
    public static void BuildDirectoryStructure(string destinationDirectory, XmlElement fileStructure){
        foreach (XmlElement childNode in fileStructure.ChildNodes){
            switch (childNode.Name){
                case "folder":
                case "directory":{
                    DirectoryInfo di = new(destinationDirectory);
                    DirectoryInfo subdir = di.CreateSubdirectory(childNode.Attributes["name"]!.Value);
                    BuildDirectoryStructure(subdir.FullName, childNode);
                    break;
                }
                case "file":{
                    string name = childNode.GetAttribute("name");
                    string path = destinationDirectory + Path.DirectorySeparatorChar + name;
                    string content = GetContentToBePutInFile(childNode);
                    File.WriteAllText(path, content);
                    break;
                }

                default: throw new Exception($"Unknown node name in file structure : <{childNode.Name}>");
            }
        }
    }

    private const string NameOfContentAttribute = "content";
    private const string NameOfSourceAttribute = "source";
    
    [SuppressMessage("ReSharper", "InvertIf")]
    private static string GetContentToBePutInFile(XmlElement xmlElement){
        if (xmlElement.HasAttribute(NameOfContentAttribute))
            return xmlElement.GetAttribute(NameOfContentAttribute);
        if (xmlElement.HasAttribute(NameOfSourceAttribute)){
            string path = xmlElement.GetAttribute(NameOfSourceAttribute);
            return File.ReadAllText(path);
        }
        return xmlElement.InnerText;
    }

    // public static void BuildDirectoryStructure(string destinationDirectory, XmlDocument fileStructureDocument){
    //     throw new NotImplementedException();
    // }

    public static MetaDataFilter GetMetaDataDifFlags(MetaData md1, MetaData md2, MetaDataFilter fieldsOfInterest){
        MetaDataFilter result = ((MetaDataFilter)md1.Attributes ^ (MetaDataFilter)md2.Attributes) &
                                fieldsOfInterest;
        if ((fieldsOfInterest & MetaDataFilter.Path) != 0)
            result |= md1.Path != md2.Path ? MetaDataFilter.Path : MetaDataFilter.None;
        if ((fieldsOfInterest & MetaDataFilter.Exists) != 0)
            result |= md1.Exists != md2.Exists ? MetaDataFilter.Exists : MetaDataFilter.None;
        if ((fieldsOfInterest & MetaDataFilter.Hash) != 0)
            result |= md1.Hash != md2.Hash ? MetaDataFilter.Hash : MetaDataFilter.None;
        if ((fieldsOfInterest & MetaDataFilter.Length) != 0)
            result |= md1.Length != md2.Length ? MetaDataFilter.Length : MetaDataFilter.None;
        if ((fieldsOfInterest & MetaDataFilter.CreationTime) != 0)
            result |= md1.CreationTime != md2.CreationTime ? MetaDataFilter.CreationTime : MetaDataFilter.None;
        if ((fieldsOfInterest & MetaDataFilter.LastAccessTime) != 0)
            result |= md1.LastAccessTime != md2.LastAccessTime ? MetaDataFilter.LastAccessTime : MetaDataFilter.None;
        if ((fieldsOfInterest & MetaDataFilter.LastWriteTime) != 0)
            result |= md1.LastWriteTime != md2.LastWriteTime ? MetaDataFilter.LastWriteTime : MetaDataFilter.None;

        return result;
    }

    public static void BuildDirectoryStructure(string destinationDirectory, XmlDocument fileStructure){
        BuildDirectoryStructure(destinationDirectory, fileStructure.DocumentElement!);
    }
}

public record MetaData{
    // ReSharper disable once MemberCanBePrivate.Global
    public readonly DateTime CaptureTime;

    public readonly string Path;
    public readonly bool Exists;

    public readonly long Length;
    public readonly DateTime CreationTime;
    public readonly DateTime LastAccessTime;
    public readonly DateTime LastWriteTime;
    public readonly FileAttributes Attributes;

    public readonly byte[]? Hash;

    // constructor
    public MetaData(string path, bool includeFileContent = false){
        Path = path;
        CaptureTime = DateTime.Now;

        Exists = File.Exists(path);

        if (!Exists) return;

        FileInfo fileInfo = new(path);
        Attributes = fileInfo.Attributes;

        Length = fileInfo.Length;
        CreationTime = fileInfo.CreationTime;
        LastAccessTime = fileInfo.LastAccessTime;
        LastWriteTime = fileInfo.LastWriteTime;

        if (includeFileContent)
            Hash = FileTools.GetMd5Hash(path);
    }

    public override string ToString(){
        return ToString(MetaDataFilter.All);
    }

    public string ToString(MetaDataFilter fieldsOfInterest){
        string s = "";

        if ((fieldsOfInterest & MetaDataFilter.CaptureTime) != 0){
            s += (s == "" ? "" : ", ") + $"CaptureTime=<{CaptureTime:O}>";
        }

        if ((fieldsOfInterest & MetaDataFilter.Attributes) != 0)
            s += (s == "" ? "" : ", ") + $"Attributes={{{(Attributes & (FileAttributes)fieldsOfInterest).ToString()}}}";

        if ((fieldsOfInterest & MetaDataFilter.Path) != 0){
            s += (s == "" ? "" : ", ") + $"Path=<{Path}>";
        }

        if ((fieldsOfInterest & MetaDataFilter.Exists) != 0){
            s += (s == "" ? "" : ", ") + $"Exists=<{Exists}>";
        }

        if ((fieldsOfInterest & MetaDataFilter.Hash) != 0){
            string value;
            if (Hash == null){
                value = "<null>";
            }
            else{
                string[] result = Array.ConvertAll(Hash, x => x.ToString());
                value = $"{{{string.Join(", ", result)}}}";
            }

            s += (s == "" ? "" : ", ") + $"Hash={value}";
        }

        if ((fieldsOfInterest & MetaDataFilter.Length) != 0){
            s += (s == "" ? "" : ", ") + $"Length=<{Length}>";
        }

        if ((fieldsOfInterest & MetaDataFilter.CreationTime) != 0){
            s += (s == "" ? "" : ", ") + $"CreationTime=<{CreationTime:O}>";
        }

        if ((fieldsOfInterest & MetaDataFilter.LastWriteTime) != 0){
            s += (s == "" ? "" : ", ") + $"LastWriteTime=<{LastWriteTime:O}>";
        }

        if ((fieldsOfInterest & MetaDataFilter.LastAccessTime) != 0){
            s += (s == "" ? "" : ", ") + $"LastAccessTime=<{LastAccessTime:O}>";
        }

        return s;
    }
}

[Flags]
public enum MetaDataFilter{
    // @formatter:off
    None              = 0,
    ReadOnly          = 1 << 0,
    Hidden            = 1 << 1,
    System            = 1 << 2,
    //  3
    Directory         = 1 << 4,
    Archive           = 1 << 5,
    Device            = 1 << 6,
    Normal            = 1 << 7, //all other FileAttributes are 0
    Temporary         = 1 << 8,
    SparseFile        = 1 << 9,
    ReparsePoint      = 1 << 10,
    Compressed        = 1 << 11,
    Offline           = 1 << 12,
    NotContentIndexed = 1 << 13,
    Encrypted         = 1 << 14,
    IntegrityStream   = 1 << 15,
    // 16
    NoScrubData       = 1 << 17,
    // 18
    // 19
    // 20
    // 21
    // 22
    // 23
    Path              = 1 << 24,
    Exists            = 1 << 25,
    Hash              = 1 << 26,
    Length            = 1 << 27,
    CreationTime      = 1 << 28,
    LastAccessTime    = 1 << 29,
    LastWriteTime     = 1 << 30,
    CaptureTime       = 1 << 31,
    // @formatter:on

    // All does not include LastAccessTime
    All = ReadOnly | Hidden | System | Directory | Archive | Device | Normal | Temporary | SparseFile | ReparsePoint |
          Compressed | Offline | NotContentIndexed | Encrypted | IntegrityStream | NoScrubData | Path |
          Exists | Hash | Length | CreationTime | LastAccessTime | LastWriteTime | CaptureTime,

    Attributes = ReadOnly | Hidden | System | Directory | Archive | Device | Normal | Temporary | SparseFile |
                 ReparsePoint | Compressed | Offline | NotContentIndexed | Encrypted | IntegrityStream |
                 NoScrubData,
}