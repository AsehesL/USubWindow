using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// SubWindow基本范例
/// </summary>
public class TestWinA : MDIEditorWindow {

    [MenuItem("Test/TestWinA")]
    static void Init()
    {
        TestWinA win = TestWinA.CreateWindow<TestWinA>();
    }

    [EWSubWindow("SunWinA", EWSubWindowIcon.Game)]
    private void SubWinA(Rect main)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "SubWinA");
    }

    [EWSubWindow("SunWinB", EWSubWindowIcon.Project)]
    private void SubWinB(Rect main)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "SubWinB");
    }

    [EWSubWindow("SunWinC", EWSubWindowIcon.Search)]
    private void SubWinC(Rect main)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "SubWinC");
    }

    [EWSubWindow("SunWinD", EWSubWindowIcon.None)]
    private void SubWinD(Rect main)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "SubWinD");
    }
}
