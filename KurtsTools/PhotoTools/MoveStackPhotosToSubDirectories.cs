using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace NSKurtsTools;

public static partial class KurtsTools{
    public static void MoveStackPhotosToSubDirectories(
        string directoryPath,
        string pathToExifToolExe,
        int deltaMilliSeconds = 1000){
        
        List<PhotoInfo> listOfPhotoInfos = GetListOfPhotoInfos(directoryPath, pathToExifToolExe);
        if (listOfPhotoInfos.Count < 3) return;

        int groupCount = GroupPhotos(listOfPhotoInfos, deltaMilliSeconds);


        DirectoryInfo di = new(directoryPath);
        for (int n = 1; n <= groupCount; n++){
            List<PhotoInfo> selectedPhotos = listOfPhotoInfos.FindAll(p => p.Group == n);
            DirectoryInfo newSubdirectory = di.CreateSubdirectory($"group_{n}");
            foreach (PhotoInfo selectedPhoto in selectedPhotos){
                File.Move(di.FullName + selectedPhoto.Filename,
                    newSubdirectory.FullName + Path.DirectorySeparatorChar + selectedPhoto.Filename);
            }
        }
    }

    private static int GroupPhotos(List<PhotoInfo> listOfPhotoInfos, int deltaMilliSeconds){
        listOfPhotoInfos.Sort(CompareByTimeStamp);

        TimeSpan threshold = new(0, 0, 0, 0, deltaMilliSeconds);
        listOfPhotoInfos[0].Group = 0;
        PhotoInfo b = listOfPhotoInfos[0];
        int groupIndex = 0;

        for (int n = 1; n < listOfPhotoInfos.Count; n++){
            PhotoInfo a = b;
            b = listOfPhotoInfos[n];
            TimeSpan d = b.CaptureTime - a.CaptureTime;
            // ReSharper disable once InvertIf
            if (d <= threshold){
                if (a.Group == 0){
                    groupIndex++;
                    a.Group = groupIndex;
                }

                b.Group = groupIndex;
            }
        }

        return groupIndex;
    }

    private static int CompareByTimeStamp(PhotoInfo x, PhotoInfo y){
        return x.CaptureTime.CompareTo(y.CaptureTime);
    }


    private static List<PhotoInfo> GetListOfPhotoInfos(string directoryPath, string pathToExifToolExe){
        (XmlDocument xmlDocument, string? _) =
            GetExifDataAsXml(
                new ArgumentGivenToGetExifDataAsXml{ PathToExifToolExe = pathToExifToolExe, Files = new[]{ directoryPath } });

        List<PhotoInfo> listOfPhotoInfos = new();

        XmlElement? root = xmlDocument.DocumentElement;
        if (root == null) return listOfPhotoInfos;

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (XmlElement aExifRoot in root.ChildNodes){
            PhotoInfo? photoInfo = GetPhotoInfo(aExifRoot);
            if (photoInfo == null) continue;

            listOfPhotoInfos.Add(photoInfo);
        }


        return listOfPhotoInfos;
    }

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    private static PhotoInfo? GetPhotoInfo(XmlElement exifRoot){
        XmlNamespaceManager namespaceManager = GetNamespaceManager(exifRoot);

        string? xFileName = GetFilename(exifRoot, namespaceManager);
        if (xFileName == null) return null;

        DateTime? xTimestamp = GetTimeStamp(exifRoot, namespaceManager);
        if (xTimestamp == null) return null;

        return new PhotoInfo{ Filename = xFileName, CaptureTime = (DateTime)xTimestamp };
    }

    // ReSharper disable once SuggestBaseTypeForParameter
    private static string? GetFilename(XmlElement exifRoot, XmlNamespaceManager namespaceManager){
        XmlNode? fileNameNode = exifRoot.SelectSingleNode(".//System:FileName", namespaceManager);
        // aExifRoot.SelectSingleNode(".//System:Directory", namespaceManager);
        return fileNameNode?.InnerText;
    }

    [SuppressMessage("ReSharper", "ReplaceSubstringWithRangeIndexer")]
    [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
    private static DateTime? GetTimeStamp(XmlElement exifRoot, XmlNamespaceManager namespaceManager){
        XmlNode? timestampNode = exifRoot.SelectSingleNode(".//Track4:TimeStamp", namespaceManager);
        if (timestampNode == null) return null;

        string s = timestampNode.InnerText; // 2022:05:28 13:57:17.34

        int year = int.Parse(s.Substring(0, 4));
        int month = int.Parse(s.Substring(5, 2));
        int day = int.Parse(s.Substring(8, 2));
        int hour = int.Parse(s.Substring(11, 2));
        int minute = int.Parse(s.Substring(14, 2));
        int second = int.Parse(s.Substring(17, 2));
        int millisecond = int.Parse(s.Substring(20, 2));
        return new DateTime(year, month, day, hour, minute, second, millisecond);
    }

    private static XmlNamespaceManager GetNamespaceManager(XmlNode aExifRoot){
        XmlNamespaceManager namespaceManager = new(aExifRoot.OwnerDocument!.NameTable);

        IDictionary<string, string> nsInFirstNode =
            aExifRoot.CreateNavigator()!.GetNamespacesInScope(XmlNamespaceScope.All);
        foreach (KeyValuePair<string, string> pair in nsInFirstNode){
            namespaceManager.AddNamespace(pair.Key, pair.Value);
        }

        return namespaceManager;
    }
}


public record PhotoInfo{
    public  string? Filename;
    public DateTime CaptureTime;
    public int Group;
}