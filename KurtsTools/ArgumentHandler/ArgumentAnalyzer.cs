// ReSharper disable ArrangeAccessorOwnerBody

using System.Diagnostics.CodeAnalysis;

namespace NSKurtsTools;

public class ArgumentAnalyzer{
    private readonly ArgumentModel _model;
    private readonly string[] _args;
    private readonly List<string> _simpleMatch = new();
    private List<string[]> _switchMatch = new();
    private readonly string[] _relevantSimpleKeySet;

    /**
     * <summary>Constructor</summary>
     * <param name="model">The <see cref="ArgumentModel"/> to use</param>
     * <param name="args">The array of strings given to the program at start</param>
     */
    public ArgumentAnalyzer(ArgumentModel model, string[] args){
        _model = model;
        _args = args;
        int argIndex = 0;
        while (argIndex < args.Length){
            SwitchArgument? x = model.FindSwitch(args[argIndex]);
            if (x != null){
                string[] a = new string[x.AntItems];
                Array.Copy(args, argIndex, a, 0, x.AntItems);
                _switchMatch.Add(a);
                argIndex += x.AntItems;
                continue;
            }

            _simpleMatch.Add(args[argIndex++]);
        }

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (string[] simpleKeySet in model.ListOfSimpleKeySets){
            if (simpleKeySet.Length != args.Length) continue;
            _relevantSimpleKeySet = simpleKeySet;
            return;
        }


        throw new ArgumentHandlerException(
            "Analyzer says: # of simple arguments does not match model.\n" +
            $"Model:\n{model}\n" +
            $"Arguments:\n{string.Join(" ", args)}\n" +
            $"Analyzer:\n{this}");
    }

    public override string ToString(){
        return ToString(0);
    }

    public int SimpleArgumentCount{
        get{ return _simpleMatch.Count; }
    }

    /**
     * <param name="key">Name of the simple key</param>
     * <returns>The argument corresponding to the given key</returns>
     */
    public string GetSimpleArgument(string key){
        // if (!SimpleKeyExists(key))
        // throw new ArgumentHandlerException(
        // $"Key \"{key}\" does not exist in relevant key set: {{{string.Join(' ', _relevantSimpleKeySet)}}}\n{this}");
        // int keyIndex = _model.SimpleKeyIndex(SimpleArgumentCount, key);
        int keyIndex = Array.IndexOf(_relevantSimpleKeySet, key);
        if (keyIndex < 0)
            throw new ArgumentHandlerException(
                $"Key \"{key}\" does not exist in relevant key set: {{{string.Join(' ', _relevantSimpleKeySet)}}}\n{this}");
        return _args[keyIndex];
    }

    [SuppressMessage("ReSharper", "ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator")]
    public string ToString(int indent){
        string tab = new(' ', indent);
        string s = _model.ToString(indent);

        s += "ARGUMENTS:\n" +
             $"{tab}    " + string.Join(' ', _args);

        return s;
    }

    public bool SimpleKeyExists(string key){
        return _relevantSimpleKeySet.Contains(key);
    }
}