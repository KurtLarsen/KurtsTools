namespace NSKurtsTools;

public static partial class KurtsTools{

    public static MetaData GetMetaData(string pathToFile, bool includeFileContent = false){
        return new MetaData(pathToFile, includeFileContent);
    }
    
    public static MetaDataFlags GetMetaDataDifferences(MetaData md1, MetaData md2, MetaDataFlags fieldsOfInterest=MetaDataFlags.All){
        MetaDataFlags result = ((MetaDataFlags)md1.Attributes ^ (MetaDataFlags)md2.Attributes) &
                                fieldsOfInterest;
        
        if ((fieldsOfInterest & MetaDataFlags.Path) != 0)
            result |= md1.Path != md2.Path ? MetaDataFlags.Path : MetaDataFlags.None;
        if ((fieldsOfInterest & MetaDataFlags.Exists) != 0)
            result |= md1.Exists != md2.Exists ? MetaDataFlags.Exists : MetaDataFlags.None;
        if ((fieldsOfInterest & MetaDataFlags.Hash) != 0)
            result |= md1.Hash != md2.Hash ? MetaDataFlags.Hash : MetaDataFlags.None;
        if ((fieldsOfInterest & MetaDataFlags.Length) != 0)
            result |= md1.Length != md2.Length ? MetaDataFlags.Length : MetaDataFlags.None;
        if ((fieldsOfInterest & MetaDataFlags.CreationTime) != 0)
            result |= md1.CreationTime != md2.CreationTime ? MetaDataFlags.CreationTime : MetaDataFlags.None;
        if ((fieldsOfInterest & MetaDataFlags.LastAccessTime) != 0)
            result |= md1.LastAccessTime != md2.LastAccessTime ? MetaDataFlags.LastAccessTime : MetaDataFlags.None;
        if ((fieldsOfInterest & MetaDataFlags.LastWriteTime) != 0)
            result |= md1.LastWriteTime != md2.LastWriteTime ? MetaDataFlags.LastWriteTime : MetaDataFlags.None;

        return result;
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

    // constructor can not be used outside this namespaces. Use function GetMetaData instead
    internal MetaData(string path, bool includeFileContent = false){
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
            Hash = GetMd5Hash(path);
    }

    public override string ToString(){
        return ToString(MetaDataFlags.All);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public string ToString(MetaDataFlags fieldsOfInterest){
        string s = "";

        if ((fieldsOfInterest & MetaDataFlags.CaptureTime) != 0){
            s += (s == "" ? "" : ", ") + $"CaptureTime=<{CaptureTime:O}>";
        }

        if ((fieldsOfInterest & MetaDataFlags.Attributes) != 0)
            s += (s == "" ? "" : ", ") + $"Attributes={{{(Attributes & (FileAttributes)fieldsOfInterest).ToString()}}}";

        if ((fieldsOfInterest & MetaDataFlags.Path) != 0){
            s += (s == "" ? "" : ", ") + $"Path=<{Path}>";
        }

        if ((fieldsOfInterest & MetaDataFlags.Exists) != 0){
            s += (s == "" ? "" : ", ") + $"Exists=<{Exists}>";
        }

        if ((fieldsOfInterest & MetaDataFlags.Hash) != 0){
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

        if ((fieldsOfInterest & MetaDataFlags.Length) != 0){
            s += (s == "" ? "" : ", ") + $"Length=<{Length}>";
        }

        if ((fieldsOfInterest & MetaDataFlags.CreationTime) != 0){
            s += (s == "" ? "" : ", ") + $"CreationTime=<{CreationTime:O}>";
        }

        if ((fieldsOfInterest & MetaDataFlags.LastWriteTime) != 0){
            s += (s == "" ? "" : ", ") + $"LastWriteTime=<{LastWriteTime:O}>";
        }

        if ((fieldsOfInterest & MetaDataFlags.LastAccessTime) != 0){
            s += (s == "" ? "" : ", ") + $"LastAccessTime=<{LastAccessTime:O}>";
        }

        return s;
    }
}

[Flags]
public enum MetaDataFlags{
    // @formatter:off
    // Index 0 to 17 is equivalent to Windows FileAttributes
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
}