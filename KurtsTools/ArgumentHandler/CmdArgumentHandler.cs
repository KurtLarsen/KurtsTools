﻿// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
namespace NSKurtsTools;

/// <summary>
/// Class for handling command line arguments
/// </summary>
[Obsolete($"Class {nameof(CmdArgumentHandler)} is obsolete. Use {nameof(ArgumentAnalyzer)} instead")]
public class CmdArgumentHandler{
    private readonly string[] _arguments;

    /**
     * <summary>Constructor</summary>
     * <param name="arguments">An array of string, as given to a console program</param>
     * <example><code>x=new CmdArgumentHandler(args);</code></example>
     */
    [Obsolete($"Class {nameof(CmdArgumentHandler)} is obsolete. Use {nameof(ArgumentAnalyzer)} instead")]
    public CmdArgumentHandler(string[] arguments){
        _arguments = arguments;
    }


    /**
     * <param name="argumentToFind">The argument to find. Multiple versions of the same argument can be given</param>
     * <example><code>
     * &#47;&#47;
     * &#47;&#47; A program was started at the command line with some additional arguments:
     * &#47;&#47;
     * &#47;&#47;     myProgram.exe /s /f abc.txt /config c.config
     * &#47;&#47;
     * <para> </para>
     * var cmdArgumentHandler = new CmdArgumentHandler(args);
     * <para> </para>
     * var result = cmdArgumentHandler.GetArgumentItem("/f","/file");
     * <para> </para>
     * &#47;&#47;
     * &#47;&#47; result.Key       = "/f"
     * &#47;&#47; result.SubParams = {"abc.txt"}
     * &#47;&#47;
     * </code>
     * </example>
     * <returns><see cref="CmdArgumentItem"/></returns>
     */
    [Obsolete($"Class {nameof(CmdArgumentHandler)} is obsolete. Use {nameof(ArgumentAnalyzer)} instead")]
    public CmdArgumentItem GetArgumentItem(params string[] argumentToFind){
        char switchId = argumentToFind[0][0];
        for (int argsIndex = 0; argsIndex < _arguments.Length; argsIndex++){
            if (!_arguments[argsIndex].StartsWith(switchId)) continue;
            if (!argumentToFind.Contains(_arguments[argsIndex])) continue;
            int argIndex2 = argsIndex + 1;
            while (argIndex2<_arguments.Length && !_arguments[argIndex2].StartsWith(switchId)){
                argIndex2++;
            }
            return new CmdArgumentItem(_arguments.SubArray(argsIndex,argIndex2-argsIndex));
        }

        return new CmdArgumentItem(Array.Empty<string>());
    }

    // public CmdArgumentItem GetArgumentItem(ArgumentItemPattern argumentItemPattern){
        // return new CmdArgumentItem();
    // }

    /// <summary>
    /// Look for one or more arguments keys in the given argument string
    /// </summary>
    /// <param name="keyToFind">One or more argument keys to look for inclusive the leading argument id string</param>
    /// <returns>True if the argument key exists otherwise False</returns>
    /// <example><code>
    /// CmdArgumentHandler cmdArgumentHandler = new CmdArgumentHandler(args);
    /// bool hasHelpKey = cmdArgumentHandler.ArgumentExists("-?","-help","/?","/help");
    /// </code></example>
    [Obsolete($"Class {nameof(CmdArgumentHandler)} is obsolete. Use {nameof(ArgumentAnalyzer)} instead")]
    // ReSharper disable once UnusedMember.Global
    public bool ArgumentExists( params string[] keyToFind){
        return GetArgumentItem( keyToFind).Key != "";
    }
    
    /// <summary>
    /// Look for one or more arguments keys in the given argument string
    /// </summary>
    /// <param name="keyToFind">One or more argument keys to look for inclusive the leading argument id string</param>
    /// <returns>True if the argument key exists otherwise False</returns>
    /// <example><code>
    /// CmdArgumentHandler cmdArgumentHandler = new CmdArgumentHandler(args);
    /// bool hasHelpKey = cmdArgumentHandler.ArgumentKeyExists("-?","-help","/?","/help");
    /// </code></example>
    [Obsolete($"Class {nameof(CmdArgumentHandler)} is obsolete. Use {nameof(ArgumentAnalyzer)} instead")]
    public bool ArgumentKeyExists( params string[] keyToFind){
        return GetArgumentItem( keyToFind).Key != "";
    }

}


