using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using EditorWinEx.Internal;

/// <summary>
/// SubWindow自定义消息弹框范例
/// </summary>
public class TestWinF : MDIEditorWindow {

    [MenuItem("SubWindow范例/6.自定义消息弹框范例")]
    static void InitWin()
    {
        TestWinA.CreateWindow<TestWinF>();
    }

    [EWSubWindow("SunWinA", EWSubWindowIcon.Game)]
    private void SubWinA(Rect main)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "SubWinA");
        if (GUI.Button(new Rect(main.x, main.y + 20, main.width, 20), "测试MsgBox"))
        {
            ShowMsgBox(2, null);
        }
    }
    
    
}

[EWMsgBoxHandle(typeof(TestWinF), 2)]
[Serializable]
class TestMsgDrawer : EWMsgBoxCustomDrawer
{
    public override EWRectangle Recttangle
    {
        get { return m_RectTangle; }
    }

    [SerializeField]
    private EWRectangle m_RectTangle = new EWRectangle(0.2f, 0.2f, 0.6f, 0.6f);

    [SerializeField] private Vector3 m_Value = Vector3.zero;

    public override void DrawMsgBox(Rect rect, object obj)
    {
        base.DrawMsgBox(rect, obj);
        GUI.Box(new Rect(rect.x, rect.y, rect.width, 18), string.Empty, GUIStyleCache.GetStyle("Toolbar"));
        if (GUI.Button(new Rect(rect.x + rect.width - 21, rect.y + 4, 13, 11), string.Empty, GUIStyleCache.GetStyle("WinBtnClose")))
        {
            CloseMsgBox();
        } 
        GUI.Label(new Rect(rect.x, rect.y + 30, rect.width, 20), "XXXXXXXXXXXXXX");
        m_Value = EditorGUI.Vector3Field(new Rect(rect.x, rect.y + 50, rect.width, 20), "Value:", m_Value);
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
