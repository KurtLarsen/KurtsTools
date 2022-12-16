using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NSKurtsTools;

public partial class KurtsTools{
    /**
     * <summary>Determines whether a key is up or down at the time the function is called, and whether the key has been pressed after the previous call to GetAsyncKeyState.</summary>
     * <returns>
     * <list type="bullet">
     * <item>is pressed (=32769=0x8001)</item>
     * <item>not pressed (=0)</item>
     * <item>not pressed, has been pressed and released since previous call (=1)</item>
     * </list>
     * </returns>
     * <seealso cref="!:https://learn.microsoft.com/en-us/windows/win32/api/winuser/" >learn.microsoft.com</seealso>
     * <seealso href="https://www.pinvoke.net/default.aspx/user32.getasynckeystate">pinvoke.net</seealso>
     * <seealso href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getasynckeystate">learn.microsoft.com</seealso>
     * <seealso href="https://stackoverflow.com/questions/32569965/how-to-use-getasynckeystate-in-c">stackoverflow.com: How to use GetAsyncKeyState in C#?</seealso>
     * <seealso href="https://github.com/johnkoerner/KeyboardListener">github.com/johnkoerner/KeyboardListener</seealso>
     */

    [DllImport(
        "user32.dll",
        CallingConvention = CallingConvention.StdCall,
        EntryPoint = "GetAsyncKeyState")]
    
    // [LibraryImport(libraryName: "user32.dll", EntryPoint = "GetAsyncKeyState")]
    #pragma warning disable CA1401
    #pragma warning disable SYSLIB1054
    public static extern KeyState GetAsyncKeyState(int vKey);
    #pragma warning restore SYSLIB1054
    #pragma warning restore CA1401
}

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public enum KeyState{
    Pressed = 32769,
    NotPressed = 0,
    NotPressedHasBeenPressedSincePreviousCall = 1,
}