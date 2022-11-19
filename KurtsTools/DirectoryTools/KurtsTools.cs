using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Xml;

namespace NSKurtsTools;

public static partial class KurtsTools{
    /**
     * <summary>Creates a new empty directory in the system temp path and returns the full path to it. The Path ends with a DirectorySeparatorChar</summary>
     * <returns>Full path to newly created directory</returns>
     */
    [SupportedOSPlatform("windows")]
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

    public static void BuildDirectoryStructure(string destinationDirectory, XmlDocument fileStructure){
        BuildDirectoryStructure(destinationDirectory, fileStructure.DocumentElement!);
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
}