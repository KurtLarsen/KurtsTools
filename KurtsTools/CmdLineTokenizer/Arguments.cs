
namespace NSKurtsTools;

[Obsolete($"Use class {nameof(CmdArgumentHandler)}")]
public class Arguments{
    private readonly string[] _args;
    private int _index;
    private string? _errorText;
    private bool _hasError;
    private int _indexOfLastItem;

    [Obsolete($"Use class {nameof(CmdArgumentHandler)}")]
    // constructor
    public Arguments(string[] args){
        _args = args;
        _index = -1;
        _hasError = false;
        _errorText = null;
    }
    
    [Obsolete($"Use class {nameof(CmdArgumentHandler)}")]
    public string? GetOptionArgumentOrMissingOptionArgumentFail{
        get{
            if (_hasError) return null;
            
            _index++;
            if (!Eof && !IsOption(_index)) return _args[_index];
            
            SetUpFail($"Option {CurrentItem} is missing argument");
            return null;

        }
    }
    
    [Obsolete($"Use class {nameof(CmdArgumentHandler)}")]
    private bool IsOption(int i){
        return _args[i].StartsWith('-');
    }

    [Obsolete($"Use class {nameof(CmdArgumentHandler)}")]
    // ReSharper disable once MemberCanBePrivate.Global
    public bool Eof => _index >= _args.Length;


    [Obsolete($"Use class {nameof(CmdArgumentHandler)}")]
    public string? CurrentItemAsValueOrUnknownOptionFail{
        get{
            if (_hasError) return null;
            
            if (!IsOption(_index)) return _args[_index];
            
            SetUpFail($"Unknown option {_args[_index]}");
            return null;

        }
    }

    [Obsolete($"Use class {nameof(CmdArgumentHandler)}")]
    private void SetUpFail(string failText){
        _hasError = true;
        _errorText=failText;
        _index = _args.Length+1;
        
    }

    [Obsolete($"Use class {nameof(CmdArgumentHandler)}")]
    // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
    public string? ErrorText => _errorText;

    // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
    public bool HasError => _hasError;

    [Obsolete($"Use class {nameof(CmdArgumentHandler)}")]
    public bool NextItem{
        get{
            _index++;
            if (_index >= _args.Length) return false;
            _indexOfLastItem = _index;
            return true;
        }
    }

    [Obsolete($"Use class {nameof(CmdArgumentHandler)}")]
    public string CurrentItem => _args[_indexOfLastItem];
}