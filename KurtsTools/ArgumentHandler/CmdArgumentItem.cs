namespace NSKurtsTools;

public record CmdArgumentItem{
    // constructor
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
    private CmdArgumentItem(string key, string[] subParams){
        Key = key;
        SubParams = subParams;
    }

    public string Key{ get; }

    public string[] SubParams{ get; }


    public CmdArgumentItem Default(params string[] defaultValue){
        return SubParams != Array.Empty<string>() ? this : new CmdArgumentItem(Key, defaultValue);
    }
}