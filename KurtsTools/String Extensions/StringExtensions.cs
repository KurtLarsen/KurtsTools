namespace NSKurtsTools;

/**
 * <returns>Parent until space or tab</returns>
 */
public static class StringExtensions{
    private static readonly char[] SplitterChars ={ ' ', '\t','\n','\r'};
    
    /**
     * <summary>Splits parent at space or tab</summary>
     * <returns>First item</returns>
     */
    public static string FirstWord(this string value){
        return value.Trim().Split(SplitterChars)[0];
    }

    /**
     * <summary>Splits parent at space or tab</summary>
     * <returns>Second item</returns>
     */
    public static string SecondWord(this string value){
        #pragma warning disable CS0618
        (string _, string rest) = value.FirstWordAndRest();
        #pragma warning restore CS0618
        return rest.FirstWord();
    }

    
    [Obsolete("Do not use! All functions returning tuples will be removed from public access")]
    // ReSharper disable once MemberCanBePrivate.Global
    public static (string, string) FirstWordAndRest(this string value){
        string[] a = value.Trim().Split(SplitterChars, 2);
        return a.Length switch{
            0 => ("", ""),
            1 => (a[0], ""),
            _ => (a[0], a[1]),
        };
    }

    [Obsolete("Do not use! All functions returning tuples will be removed from public access")]
    // ReSharper disable once UnusedMember.Global
    public static (string, string) SecondWordAndRest(this string value){
        string[] a = value.Trim().Split(SplitterChars, 3);
        return a.Length switch{
            0 => ("", ""),
            1 => ("", ""),
            2 => (a[1], ""),
            _ => (a[1], a[2]),
        };
    }

    /**
     * <returns>Trimmed string where first word is deleted</returns>
     */
    public static string DeleteFirstWord(this string value){
        string[] a = value.Trim().Split(SplitterChars,2,StringSplitOptions.RemoveEmptyEntries);
        return a.Length == 2 ? a[1] : "";
    }

    /**
     * <summary><para>Returns directory (inclusive trailing directory separator character) from path</para>This only works correct if string is a valid path</summary>
     */
    public static string Directory(this string value){
        FileInfo fi = new(value);
        return value[..^fi.Name.Length];
    }
    
    /**
     * <summary><para>Returns file base name (filename without extension) from path</para>This only works correct if string is a valid path</summary>
     */
    public static string FileBaseName(this string value){
        FileInfo fi = new(value);
        return fi.Name[..^fi.Extension.Length];
    }
    
    /**
     * <summary><para>Returns file extension (inclusive dot) from path</para>This only works correct if string is a valid path</summary>
     */
    public static string FileExtension(this string value){
        FileInfo fi = new(value);
        return fi.Extension;
    }

    /**
     * <summary><para>Returns file name (base name + extension) from path</para>This only works correct if string is a valid path</summary>
     */
    public static string FileName(this string value){
        FileInfo fi = new(value);
        return fi.Name;
    }
}