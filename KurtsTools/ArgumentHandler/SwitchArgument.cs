// ReSharper disable ArrangeAccessorOwnerBody
namespace NSKurtsTools;

public class SwitchArgument{
    internal readonly string[] Id;
    internal readonly string[] SubKeys;
    public readonly string[] SubArgs;

    // constructor
    public SwitchArgument(string[] id,params string[] subKeys){
        Id = id;
        SubKeys = subKeys;
        SubArgs = new string[subKeys.Length];
    }

    public SwitchArgument(string id,params string[] subKeys){
        Id = new[]{ id };
        SubKeys = subKeys;
        SubArgs = new string[subKeys.Length];
    }

    public int AntItems{
        get{ return 1+SubArgs.Length; }
    }

    
    public override string ToString(){
        return ToString(0);
    }

    public string ToString(int indent){
        string tab = new(' ', indent);
        string s = $"{tab}[{string.Join("|", Id)}]";
        if (SubKeys.Length > 0)
            s += " "+string.Join(' ', SubKeys);
        return $"{tab}{s}\n";
    }
}