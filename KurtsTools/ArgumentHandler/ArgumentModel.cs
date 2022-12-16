// ReSharper disable MemberCanBeMadeStatic.Global
// ReSharper disable UnusedParameter.Global

using System.Diagnostics.CodeAnalysis;

namespace NSKurtsTools;

public class ArgumentModel{
    internal readonly List<string[]> ListOfSimpleKeySets = new();
    internal readonly List<SwitchArgument> ListOfSwitchArguments = new();

    public void AddSimpleKeySet(params string[] simpleKeySet){
        if (ListOfSimpleKeySets.Any(existingSet => simpleKeySet.Length == existingSet.Length)){
            throw new ArgumentHandlerException($"Key set with {simpleKeySet.Length} element{(simpleKeySet.Length==1?"":"s")} already exists");
        }

        ListOfSimpleKeySets.Add(simpleKeySet);
    }

    public void AddSwitchKey(string[] id,params string[] subParam){
        // todo: check if id already exists
        ListOfSwitchArguments.Add(new SwitchArgument(id,subParam));
    }

    public override string ToString(){
        return ToString(0);
    }
    

    [SuppressMessage("ReSharper", "ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator")]
    public string ToString(int indent){
        string tab = new(' ', indent);
        string s = "MODEL:\n" +
                   $"{tab}    {ListOfSimpleKeySets.Count} {nameof(ListOfSimpleKeySets)}{(ListOfSimpleKeySets.Count > 0 ? ":" : "")}\n";
        
        foreach (string[] simpleKeySet in ListOfSimpleKeySets){
            s += $"{tab}        {string.Join(' ',simpleKeySet)}\n";
        }

        s += $"{tab}    {ListOfSwitchArguments.Count} {nameof(ListOfSwitchArguments)}{(ListOfSwitchArguments.Count > 0 ? ":" : "")}\n";
        
        foreach (SwitchArgument switchArgument in ListOfSwitchArguments){
            s += switchArgument.ToString(indent+4);
        }

        return s;
    }

    public SwitchArgument? FindSwitch(string key){
       SwitchArgument? result= ListOfSwitchArguments.Find(x => x.Id.Contains(key));
       return result;
    }

    [Obsolete($"When class {nameof(ArgumentAnalyzer)} is constructed, the relevant set of simple arguments is registered. That way {nameof(ArgumentAnalyzer)} can get the keyIndex by using Array.IndexOf()")]
    // ReSharper disable once UnusedMember.Global
    public int SimpleKeyIndex(int simpleArgumentCount, string key){
        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (string[] simpleKeySet in ListOfSimpleKeySets){
            if (simpleKeySet.Length != simpleArgumentCount) continue;
            
            int index=Array.IndexOf(simpleKeySet, key);
            if (index == -1)
                throw new ArgumentHandlerException($"Simple key \"{key}\" does not exist in model:\n{this}");
            return index;
        }

        // todo: remove; already checked in ArgumentAnalyzer-constructor constructor
        throw new ArgumentHandlerException($"No set of simple keys with {simpleArgumentCount} items exists in model:\n{this}");
    }
}