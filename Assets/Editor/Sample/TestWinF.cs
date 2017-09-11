using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

/// <summary>
/// SubWindow自定义消息弹框范例
/// </summary>
public class TestWinF : MDIEditorWindow {

    [MenuItem("Test/TestWinF")]
    static void Init()
    {
        TestWinF win = TestWinA.CreateWindow<TestWinF>();
    }

    [SubWindow("SunWinA", SubWindowIcon.Game)]
    private void SubWinA(Rect main)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "SubWinA");
        if (GUI.Button(new Rect(main.x, main.y + 20, main.width, 20), "测试MsgBox"))
        {
            ShowMsgBox(2, null);
        }
    }
    
    
}

[MsgBoxHandle(typeof(TestWinF), 2)]
class TestMsgDrawer : EWMsgBoxCustomObjectDrawer
{
    public override float Height
    {
        get { return 0.6f; }
    }

    public override float Width
    {
        get { return 0.6f; }
    }

    public override float X
    {
        get { return 0.2f; }
    }

    public override float Y
    {
        get { return 0.2f; }
    }

    public override void DrawMsgBox(Rect rect, object obj)
    {
        base.DrawMsgBox(rect, obj);
        GUI.Box(new Rect(rect.x, rect.y, rect.width, 18), string.Empty, GUIStyleCache.GetStyle("Toolbar"));
        if (GUI.Button(new Rect(rect.x + rect.width - 21, rect.y + 4, 13, 11), string.Empty, GUIStyleCache.GetStyle("WinBtnClose")))
        {
            CloseMsgBox();
        }
        GUI.Label(new Rect(rect.x, rect.y + 30, rect.width, 20), "XXXXXXXXXXXXXX");
    }

    public override void Init()
    {
        base.Init();
        Debug.Log("Init");
    }

    public override void OnEnable()
    {
        base.OnEnable();
        Debug.Log("Enable");
    }

    public override void OnDisable()
    {
        base.OnDisable();
        Debug.Log("Disbale");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        Debug.Log("Destroy");
    }
}
