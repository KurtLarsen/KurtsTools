using System.Security.Cryptography;

namespace KurtsToolsLibrary.FileTools;

public static partial class FileTools{
    /*
     * source: https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
     */


    public static byte[] GetMd5Hash(string path){
        using MD5 md5 = MD5.Create();
        using FileStream inputStream = File.OpenRead(path);
        return md5.ComputeHash(inputStream);
    }


}
