using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools.Testing_Array_Extensions;

[TestFixture]
public class Testing_Array_Extensions{
private readonly string[] _mainArray = { "red", "blue", "green", "yellow", "purple" };

    [Test]
    public void Testing_fn_SubArray(){
        string[] subArray = _mainArray.SubArray(1, 3);
        
        Assert.That(subArray,Is.EquivalentTo(new []{"blue", "green", "yellow"}));
    }

    
    [Test]
    public void Testing_fn_SubArray_where_requested_length_reaches_beyond_end_of_mainArray(){
        string[] subArray = _mainArray.SubArray(3, 3);
        
        Assert.That(subArray,Is.EquivalentTo(new []{"yellow", "purple"}));
    }

    [Test]
    public void Testing_fn_SubArray_where_offset_is_beyond_end_of_mainArray(){
        string[] subArray = _mainArray.SubArray(5, 3);
        
        Assert.That(subArray,Is.EquivalentTo(Array.Empty<string>()));
    }

}