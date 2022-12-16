// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
namespace NSKurtsTools;

[Obsolete($"Class {nameof(CmdArgumentHandler)} is obsolete. Use {nameof(ArgumentAnalyzer)} instead")]
public record CmdArgumentItem{
    // constructor
    [Obsolete($"Class {nameof(CmdArgumentHandler)} is obsolete. Use {nameof(ArgumentAnalyzer)} instead")]
    public CmdArgumentItem(IReadOnlyList<string> subArray){
        if (subArray == Array.Empty<string>()){
            Key = "";
            SubParams = Array.Empty<string>();
        }else{
            Key = subArray[0];
            SubParams = subArray.Skip(1).ToArray();
        }
    }

    // constructor
    [Obsolete($"Class {nameof(CmdArgumentHandler)} is obsolete. Use {nameof(ArgumentAnalyzer)} instead")]
    private CmdArgumentItem(string key, string[] subParams){
        Key = key;
        SubParams = subParams;
    }

    [Obsolete($"Class {nameof(CmdArgumentHandler)} is obsolete. Use {nameof(ArgumentAnalyzer)} instead")]
    public string Key{ get; }

    [Obsolete($"Class {nameof(CmdArgumentHandler)} is obsolete. Use {nameof(ArgumentAnalyzer)} instead")]
    public string[] SubParams{ get; }


    [Obsolete($"Class {nameof(CmdArgumentHandler)} is obsolete. Use {nameof(ArgumentAnalyzer)} instead")]
    public CmdArgumentItem Default(params string[] defaultValue){
        return SubParams != Array.Empty<string>() ? this : new CmdArgumentItem(Key, defaultValue);
    }
}