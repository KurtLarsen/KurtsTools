
namespace NSKurtsTools;

[Obsolete($"Use class {nameof(CmdArgumentHandler)}")]
public class CmdLineTokenizer{
    private readonly List<OptionDefinition> _parameterDefinitions = new();
    
    private readonly string[] _shortOptionIdentifier = { "-", "/" };
    private readonly string[] _longOptionIdentifier = { "--" };

    
    
    // constructor
    [Obsolete($"Use class {nameof(CmdArgumentHandler)}")]
    public CmdLineTokenizer(){ }

    [Obsolete($"Use class {nameof(CmdArgumentHandler)}")]
    // constructor
    public CmdLineTokenizer(IEnumerable<OptionDefinition> parameterDefinitions){
        foreach (OptionDefinition parameterDefinition in parameterDefinitions){
            AddParameterDefinition(parameterDefinition);
        }
    }

    private void AddParameterDefinition(OptionDefinition optionDefinition){
        if (_parameterDefinitions.Find(x => x.Name.Equals(optionDefinition.Name)) != null)
            throw new Exception($"ParameterDefinition \"{optionDefinition.Name}\" already exist");
        _parameterDefinitions.Add(optionDefinition);
    }

    [Obsolete($"Use class {nameof(CmdArgumentHandler)}")]
    public Token[] Tokens(string[] args){
        List<Token> tokenList = new();
        int p = 0;

        while (p < args.Length){
            OptionDefinition? parameterDefinition = GetParameterDefinition(args[p]);


            if (parameterDefinition == null){
                // TokenType.unknownOption
                tokenList.Add(new Token{ Type = TokenType.UnknownOption, Name = args[p] });
                break;
            }

            if (p + parameterDefinition.OptionValueCount >= args.Length){
                // TokenType.invalidOption
                tokenList.Add(new Token{ Type = TokenType.InvalidOption, Name = parameterDefinition.Name});
                break;
            }

            if (parameterDefinition.Name == ""){
                // TokenType.value
                tokenList.Add(new Token{ Type = TokenType.Value, Name = "", Values ={ args[p++] } });
                continue;
            }


            // TokenType.Option
            List<string> values = new();
            for (int n = 0; n < parameterDefinition.OptionValueCount; n++){
                values.Add(args[++p]);
            }
            tokenList.Add(new Token{ Type = TokenType.Option, Name = parameterDefinition.Name, Values = values });

            p++;
        }

        return tokenList.ToArray();
    }

    /**
     * <code>null if unknown option
     * ParameterDefinition if known option
     * ParameterDefinition{name=""} if value</code>
     */
    private OptionDefinition? GetParameterDefinition(string arg){
        foreach (string shortIdentifier in _shortOptionIdentifier){
            if (!arg.StartsWith(shortIdentifier)) continue;
            string text = arg[shortIdentifier.Length..];
            return _parameterDefinitions.Find(x => x.ShortId.Equals(text));
        }

        foreach (string longIdentifier in _longOptionIdentifier){
            if (!arg.StartsWith(longIdentifier)) continue;
            string text = arg[longIdentifier.Length..];
            return _parameterDefinitions.Find(x => x.LongId.Equals(text));
        }

        return new OptionDefinition();
    }
}

[Obsolete($"Use class {nameof(CmdArgumentHandler)}")]
public class OptionDefinition{
    public string Name = "";
    public string ShortId = "";
    public string LongId = "";
    public int OptionValueCount;
}

[Obsolete($"Use class {nameof(CmdArgumentHandler)}")]
public class Token{
    public TokenType Type;
    public string Name = "";
    public List<string> Values = new();


    public override string ToString(){
        return $"type={Type},name={Name},values=[{string.Join(',', Values.ToArray())}]";
    }
}

[Obsolete($"Use class {nameof(CmdArgumentHandler)}")]
public enum TokenType{
    Value,
    Option,
    UnknownOption,
    InvalidOption,
}