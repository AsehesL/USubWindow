using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// SubWindow样式范例
/// </summary>
public class TestWinB : MDIEditorWindow {

    [MenuItem("Test/TestWinB")]
    static void Init()
    {
        TestWinB win = TestWinA.CreateWindow<TestWinB>();
    }

    [SubWindow("Grid", SubWindowIcon.Game, true, SubWindowStyle.Grid)]
    private void SubWinA(Rect main)
    {
        if (GUI.Button(new Rect(main.x, main.y, 100, 20), "Btn"))
        {
            
        }
    }

    [SubWindow("Preview", SubWindowIcon.Project, true, SubWindowStyle.Preview)]
    private void SubWinB(Rect main)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "SubWinB");
    }

    [SubWindow("Default", SubWindowIcon.Search)]
    private void SubWinC(Rect main)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "SubWinC");
    }
}
