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


}
