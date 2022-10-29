namespace NSKurtsTools;

public static partial class KurtsTools{
    // todo: create test
    public static Dictionary<string, string> ReadConfigFile(string path){
        IEnumerable<string> x = File.ReadLines(path);

        Dictionary<string, string> d = new();
        char[] c = { ' ', '\t' };
        foreach (string s in x){
            string l = s.Trim();
            if (string.IsNullOrEmpty(l)) continue;
            if (l.StartsWith('#')) continue;
            string[] a = l.Split(c, 2);
            if (a.Length < 2) a[1] = "";
            d.Add(a[0], a[1].Trim());
        }

        return d;
    }

}