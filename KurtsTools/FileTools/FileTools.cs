using System.Security.Cryptography;

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
    private static string NewFileName(){
        return NewFileName(Path.GetTempPath());
    }

    /**
     * <summary>Return an unused file name in the given directory</summary>
     * <returns>Full path file name</returns>
     * <param name="directoryName">Path to directory</param>
     * <exception cref="DirectoryNotFoundException">If directory is not found</exception>
     */
    private static string NewFileName(string directoryName){
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
    private static string NewFileName(DirectoryInfo directoryInfo){
        return NewFileName(directoryInfo.FullName);
    }


}
