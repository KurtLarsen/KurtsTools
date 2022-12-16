using System.Diagnostics;
using System.Runtime.Versioning;
using NSKurtsTools;
using NUnit.Framework;

namespace NSTesting_KurtsTools;

[TestFixture, SupportedOSPlatform("windows")]
public class Testing_Keyboard{
    [Test ,Ignore("This test should only be run manually and in Debug-mode")]
    // To run this test comment out the Ignore attribute above
    // Output is sent to Tool-window "Debug", tab "Debug Output"
    public void testing_GetAsyncKeyState(){
            KeyState oldX = KeyState.NotPressed;
            while (true){
                KeyState x = KurtsTools.GetAsyncKeyState(65);
                if (x != oldX){
                    oldX = x;
                    Debug.WriteLine("");
                }
                Debug.Write($"{x},");
                Thread.Sleep(500);
            }
            // ReSharper disable once FunctionNeverReturns
    }
}