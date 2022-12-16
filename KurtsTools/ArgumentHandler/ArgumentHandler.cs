// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable MemberCanBeMadeStatic.Global
// ReSharper disable All
namespace NSKurtsTools;

[Obsolete($"Class {nameof(ArgumentHandler)} is obsolete. Use class {nameof(ArgumentAnalyzer)} instead")]
public class ArgumentHandler{
    private readonly ListOfArguments _args;
    private readonly ArgumentModel _argumentModel;
    private readonly string[] _simpleKeys;
    private readonly List<SwitchArgument> _switches = new();
    // todo: extract switches and put rest in _simpleKeys

    // constructor

    [Obsolete($"Class {nameof(ArgumentHandler)} is obsolete. Use class {nameof(ArgumentAnalyzer)} instead")]
    public ArgumentHandler(ArgumentModel argumentModel, string[] args){
        _argumentModel = argumentModel;
        _args = new ListOfArguments(args);

        foreach (SwitchArgument switchArgument in _argumentModel.ListOfSwitchArguments){
            SwitchArgument? extractedSwitchArgument = _args.ExtracSwitchArgument(switchArgument);
            if(extractedSwitchArgument==null) continue;
            _switches.Add(extractedSwitchArgument);
        }


        _simpleKeys = GetSimpleKeys(args.Length) ?? throw new ArgumentHandlerException(
            $"Not able to match arguments with model\nArguments has {args.Length} items:\n{string.Join(' ', args)}\nModel:\n{this}");

    }

    [Obsolete($"Class {nameof(ArgumentHandler)} is obsolete. Use class {nameof(ArgumentAnalyzer)} instead")]
    private string[]? GetSimpleKeys(int itemCount){
        return _argumentModel.ListOfSimpleKeySets.Find(set => set.Length == itemCount);
    }


    [Obsolete($"Class {nameof(ArgumentHandler)} is obsolete. Use class {nameof(ArgumentAnalyzer)} instead")]
    public void SetSwitch(string switchId, params string[] subParams){
        throw new NotImplementedException();
    }

    [Obsolete($"Class {nameof(ArgumentHandler)} is obsolete. Use class {nameof(ArgumentAnalyzer)} instead")]
    public void SetSwitch(string[] switchId, params string[] subParams){
        throw new NotImplementedException();
    }

    [Obsolete($"Class {nameof(ArgumentHandler)} is obsolete. Use class {nameof(ArgumentAnalyzer)} instead")]
    public string GetArgumentValue(string namelessArgument){
        throw new NotImplementedException();
    }

    [Obsolete($"Class {nameof(ArgumentHandler)} is obsolete. Use class {nameof(ArgumentAnalyzer)} instead")]
    public bool SimpleArgumentExists(string simpleArgumentName){
        throw new NotImplementedException();
    }

    [Obsolete($"Class {nameof(ArgumentHandler)} is obsolete. Use class {nameof(ArgumentAnalyzer)} instead")]
    public string GetSimpleArgument(string simpleKey){
        int index = Array.IndexOf(_simpleKeys, simpleKey);
        if (index == -1) throw new ArgumentHandlerException($"\"{simpleKey}\" is not a valid key\n{this}");
        return _args[index];
    }

    [Obsolete($"Class {nameof(ArgumentHandler)} is obsolete. Use class {nameof(ArgumentAnalyzer)} instead")]
    public override string ToString(){
        return $"Arguments has {_args.Count} items:\n{string.Join(' ', _args)}\nModel:\n{_argumentModel}";
    }

    [Obsolete($"Class {nameof(ArgumentHandler)} is obsolete. Use class {nameof(ArgumentAnalyzer)} instead")]
    public bool SwitchExits(string s){
        for (int n = 0; n < _args.Count; n++){
            if (_args[n] == s){
                
            }
        }

        return false;
    }
}

[Obsolete($"Class {nameof(ListOfArguments)} is only used in class {nameof(ArgumentHandler)}. Class {nameof(ArgumentHandler)} is obsolete. Use class {nameof(ArgumentAnalyzer)} instead")]

internal class ListOfArguments : List<string>{
    [Obsolete($"Class {nameof(ListOfArguments)} is only used in class {nameof(ArgumentHandler)}. Class {nameof(ArgumentHandler)} is obsolete. Use class {nameof(ArgumentAnalyzer)} instead")]
    public ListOfArguments(IEnumerable<string> collection) : base(collection){ }

    [Obsolete($"Class {nameof(ListOfArguments)} is only used in class {nameof(ArgumentHandler)}. Class {nameof(ArgumentHandler)} is obsolete. Use class {nameof(ArgumentAnalyzer)} instead")]
    public int GetIndexOfSwitchInArgsArray(SwitchArgument switchArgument){
        for (int argsIndex = 0; argsIndex < this.Count; argsIndex++){
            if (switchArgument.Id.Contains(this[argsIndex])){
                if (argsIndex + switchArgument.SubKeys.Length >= this.Count)
                    throw new ArgumentHandlerException(
                        $"Switch \"{this[argsIndex]}\" found at argsIndex {argsIndex}. Swithc \"{this[argsIndex]}\" has {switchArgument.SubKeys.Length} sub arguments, but total number of arguments is {this.Count}");
                return argsIndex;
            }
        }

        return -1;
    }

    [Obsolete($"Class {nameof(ListOfArguments)} is only used in class {nameof(ArgumentHandler)}. Class {nameof(ArgumentHandler)} is obsolete. Use class {nameof(ArgumentAnalyzer)} instead")]
    public SwitchArgument? ExtracSwitchArgument(SwitchArgument switchArgument){
        for (int argsIndex = 0; argsIndex < this.Count; argsIndex++){
            if (switchArgument.Id.Contains(this[argsIndex])){
                if (argsIndex + switchArgument.SubKeys.Length >= this.Count)
                    throw new ArgumentHandlerException(
                        $"Switch \"{this[argsIndex]}\" found at argsIndex {argsIndex}. Swithc \"{this[argsIndex]}\" has {switchArgument.SubKeys.Length} sub arguments, but total number of arguments is {this.Count}");
                for (int subParamIndex = 0; subParamIndex < switchArgument.SubKeys.Length; subParamIndex++){
                    switchArgument.SubArgs[subParamIndex] = this[argsIndex + 1 + subParamIndex];
                    
                }
            }
        }

        return null; // dummy, so the function can compile
    }
}