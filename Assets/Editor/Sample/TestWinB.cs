using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// SubWindow样式范例
/// </summary>
public class TestWinB : MDIEditorWindow {

    [MenuItem("SubWindow范例/2.样式范例")]
    static void Init()
    {
        TestWinB win = TestWinA.CreateWindow<TestWinB>();
    }

    [EWSubWindow("Grid", EWSubWindowIcon.Game, true, SubWindowStyle.Grid)]
    private void SubWinA(Rect main)
    {
        if (GUI.Button(new Rect(main.x, main.y, 100, 20), "Btn"))
        {
            
        }
    }

    [EWSubWindow("Preview", EWSubWindowIcon.Project, true, SubWindowStyle.Preview)]
    private void SubWinB(Rect main)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "SubWinB");
    }

    [EWSubWindow("Default", EWSubWindowIcon.Search)]
    private void SubWinC(Rect main)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "SubWinC");
    }
}
