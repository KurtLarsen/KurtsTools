using System.Security.Cryptography;
// ReSharper disable ConvertToAutoPropertyWhenPossible

namespace NSKurtsTools;

public static partial class KurtsTools{
    /*
     * source: https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
     */


    // ReSharper disable once MemberCanBePrivate.Global
    public static byte[] GetMd5Hash(string path){
        using MD5 md5 = MD5.Create();
        using FileStream inputStream = File.OpenRead(path);
        return md5.ComputeHash(inputStream);
    }

    /**
     * <summary>Return an unused file name in the Windows TMP directory</summary>
     * <returns>Full path file name</returns>
     * <example><code>string newFileName=NewFileName()+".txt"</code></example>
     */
    public static string NewFileName(){
        return NewFileName(Path.GetTempPath());
    }

    /**
     * <summary>Return an unused file name in the given directory</summary>
     * <returns>Full path file name</returns>
     * <param name="directoryName">Path to directory</param>
     * <exception cref="DirectoryNotFoundException">If directory is not found</exception>
     */
    public static string NewFileName(string directoryName){
        if (!Directory.Exists(directoryName)) throw new DirectoryNotFoundException(directoryName);
        if (!directoryName.EndsWith(Path.DirectorySeparatorChar)) directoryName += Path.DirectorySeparatorChar;
        string newFileName;
        do{
            newFileName = directoryName + Guid.NewGuid();
        } while (File.Exists(newFileName));

        return newFileName;
    }
    
    /**
     * <summary>Return an unused file name in the given directory</summary>
     * <returns>Full path file name</returns>
     * <param name="directoryInfo">The directory of the new file name</param>
     * <exception cref="DirectoryNotFoundException">If directory is not found</exception>
     */
    // ReSharper disable once SuggestBaseTypeForParameter
    public static string NewFileName(DirectoryInfo directoryInfo){
        return NewFileName(directoryInfo.FullName);
    }


    /**
     * <summary>Return a unique filename for the directory</summary>
     * <param name="filePathWithPlaceholder">Filename containing placeholder "{0}"</param>
     * <param name="indexStart">Start value to be inserted into placeholder</param>
     * <returns>Unique filename</returns>
     * <example><code>string s=UniqueFileName("myfile{0}.txt")</code></example>
     */
    public static string UniqueFileName(string filePathWithPlaceholder,int indexStart = 2){
        string dir = filePathWithPlaceholder.Directory();
        if (string.Format(dir, 1) != dir) throw new ArgumentException("Placeholder in directory not allowed");
        string filename = filePathWithPlaceholder.FileName();
        if (string.Format(filename, 1) == filename){
            filePathWithPlaceholder = dir + filePathWithPlaceholder.FileBaseName() + "{0}" +
                                      filePathWithPlaceholder.FileExtension();
        }

        string result;
        do{
            result = string.Format(filePathWithPlaceholder, indexStart++);
        } while (File.Exists(result));

        return result;
    }
}