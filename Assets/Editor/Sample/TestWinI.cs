using UnityEngine;
using UnityEditor;
using System.Collections;

public class TestWinI : MDIEditorWindow
{

    [MenuItem("SubWindow范例/9.Handle范例")]
    static void OpenWindow()
    {
        TestWinI.CreateWindow<TestWinI>(typeof (HandleTest), new Vector3(1, 2, 3), 1.06f);
    }

    [EWSubWindow("SubWin")]
    private void DrawSubWin(Rect rect)
    {
        
    }
}

[System.Serializable]
class HandleTest
{
    public Vector3 arg0;
    public float arg1;

    public HandleTest(Vector3 arg0, float arg1)
    {
        this.arg0 = arg0;
        this.arg1 = arg1;
    }

    [EWSubWindow("HandleSubWin1")]
    private void DrawHandleSubWin1(Rect rect)
    {
        arg0 = EditorGUI.Vector3Field(new Rect(rect.x, rect.y, rect.width, 20), "Arg0:", arg0);
    }

    [EWSubWindow("HandleSubWin2")]
    private void DrawHandleSubWin2(Rect rect)
    {
        arg1 = EditorGUI.Slider(new Rect(rect.x, rect.y + 20, rect.width, 20), "Arg1:", arg1, 0, 200);
    }
}
