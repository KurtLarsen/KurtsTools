namespace NSKurtsTools;

public class CmdArgumentHandler{
    private readonly string[] _arguments;

    /**
     * <param name="arguments">An array of string, as given to a console program</param>
     * <example><code>x=new CmdArgumentHandler(args);</code></example>
     */
    public CmdArgumentHandler(string[] arguments){
        _arguments = arguments;
    }


    /**
     * <param name="argumentToFind">The argument to find. Multiple versions of the same argument can be given</param>
     * <example><code>&#47;&#47; setup an array of string arguments
     * string[] args = new []{"/s","/f","abc.txt","/config","c.config"}<br/>
     * <para> </para>
     * &#47;&#47; create new CmdArgumentHandler
     * var cmdArgumentHandler = new CmdArgumentHandler(args);
     * <para> </para>
     * &#47;&#47; get item starting with "/f" or "/file"
     * var result = cmdArgumentHandler.GetArgumentItem("/f","/file");
     * <para> </para>
     * &#47;&#47; result.Key is now "/f"
     * &#47;&#47; result.SubParams is now {"abc.txt"}
     * </code>
     * </example>
     */
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


    // ReSharper disable once MemberCanBePrivate.Global
    public bool ArgumentExists( params string[] argumentToFind){
        return GetArgumentItem( argumentToFind).Key != "";
    }
}

public static class Extensions{
    public static T[] SubArray<T>(this T[] array, int offset, int length){
        T[] result = new T[length];
        Array.Copy(array, offset, result, 0, length);
        return result;
    }
}