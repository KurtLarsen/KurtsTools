// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
namespace NSKurtsTools;

public static class StringExtensions{
    public static string FirstWord(this string value){
        return value.Trim(' ','\t').Split(' ', '\t')[0];
    }

    public static string SecondWord(this string value){
        (string _, string rest) = value.FirstWordAndRest();
        return rest.FirstWord();
    }

    public static (string, string) FirstWordAndRest(this string value){
        string[] a = value.Trim().Split(new[]{ ' ', '\t' }, 2);
        return a.Length switch{
            0 => ("", ""),
            1 => (a[0], ""),
            _ => (a[0], a[1]),
        };
    }

    public static (string, string) SecondWordAndRest(this string value){
        string[] a = value.Trim().Split(new[]{ ' ', '\t' }, 3);
        return a.Length switch{
            0 => ("", ""),
            1 => ("", ""),
            2 => (a[1], ""),
            _ => (a[1], a[2]),
        };
    }

    public static string TrimFirstWord(this string value){
        string[] a = value.Trim().Split(new[]{ ' ', '\t' }, 2);
        return a.Length == 2 ? a[1] : "";
    }
}