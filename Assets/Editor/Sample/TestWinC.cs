using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// SubWindow工具栏和帮助栏范例
/// </summary>
public class TestWinC : MDIEditorWindow {

    [MenuItem("Test/TestWinC")]
    static void Init()
    {
        TestWinC win = TestWinA.CreateWindow<TestWinC>();
    }

    [SubWindow("SunWinA", SubWindowIcon.None)]
    private void SubWinA(Rect main)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "普通SubWindow");
    }

    [SubWindow("SunWinB", SubWindowIcon.None, true, SubWindowStyle.Default, SubWindowToolbarType.Normal)]
    private void SubWinB(Rect main, Rect toolbar)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "这是一个有Toolbar的SubWindow");

        if(GUIEx.ToolbarButton(new Rect(toolbar.x,toolbar.y, 100, toolbar.height), "btn")) { }
    }

    [SubWindow("SunWinC", SubWindowIcon.None, true, SubWindowStyle.Default, SubWindowToolbarType.None, SubWindowHelpBoxType.Locker)]
    private void SubWinC(Rect main, Rect helpBox)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "这是一个有HelpBox的SubWindow");
        GUI.Label(new Rect(helpBox.x, helpBox.y+10, helpBox.width, 20), "HelpBox");
    }

    [SubWindow("SunWinD", SubWindowIcon.None, true, SubWindowStyle.Default, SubWindowToolbarType.Normal, SubWindowHelpBoxType.Bottom)]
    private void SubWinD(Rect main, Rect toolbar, Rect helpBox)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "这是一个即有Toolbar又有HelpBox的SubWindow");
    }
}
