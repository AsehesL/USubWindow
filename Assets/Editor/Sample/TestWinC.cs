using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// SubWindow工具栏和帮助栏范例
/// </summary>
public class TestWinC : MDIEditorWindow {

    [MenuItem("SubWindow范例/3.工具栏和帮助栏范例")]
    static void Init()
    {
        TestWinC win = TestWinA.CreateWindow<TestWinC>();
    }

    [EWSubWindow("SunWinA", EWSubWindowIcon.None)]
    private void SubWinA(Rect main)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "普通SubWindow");
    }

    [EWSubWindow("SunWinB", EWSubWindowIcon.None, true, SubWindowStyle.Default, EWSubWindowToolbarType.Normal)]
    private void SubWinB(Rect main, Rect toolbar)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "这是一个有Toolbar的SubWindow");

        if(GUIEx.ToolbarButton(new Rect(toolbar.x,toolbar.y, 100, toolbar.height), "btn")) { }
    }

    [EWSubWindow("SunWinC", EWSubWindowIcon.None, true, SubWindowStyle.Default, EWSubWindowToolbarType.None, SubWindowHelpBoxType.Locker)]
    private void SubWinC(Rect main, Rect helpBox)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "这是一个有HelpBox的SubWindow");
        GUI.Label(new Rect(helpBox.x, helpBox.y+10, helpBox.width, 20), "HelpBox");
    }

    [EWSubWindow("SunWinD", EWSubWindowIcon.None, true, SubWindowStyle.Default, EWSubWindowToolbarType.Normal, SubWindowHelpBoxType.Bottom)]
    private void SubWinD(Rect main, Rect toolbar, Rect helpBox)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "这是一个即有Toolbar又有HelpBox的SubWindow");
    }
}
