using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// SubWindow主工具栏扩展和自定义消息弹框范例
/// </summary>
public class TestWinD : MDIEditorWindow {

    private enum TestMsg
    {
        Msg1,
        Msg2,
    }

    [MenuItem("SubWindow范例/4.主工具栏扩展和自定义消息弹框范例")]
    static void InitWin()
    {
        TestWinA.CreateWindow<TestWinD>();
    }

    [EWSubWindow("SunWinA", EWSubWindowIcon.None)]
    private void SubWinA(Rect main)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "SubWinA");
        if (GUI.Button(new Rect(main.x, main.y + 20, main.width, 20), "Btn1"))
        {
            Debug.Log("按下了Btn1");
        }
    }

    [EWSubWindow("SunWinB", EWSubWindowIcon.None)]
    private void SubWinB(Rect main)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "SubWinB");
        if (GUI.Button(new Rect(main.x, main.y + 20, main.width, 20), "Btn2"))
        {
            Debug.Log("按下了Btn2");
        }
    }

    [EWToolBar("工具/Test1")]
    private void Test1()
    {
        Debug.Log("按下了Test1");
    }

    [EWToolBar("工具/Test2")]
    private void Test2()
    {
        Debug.Log("按下了Test2");
    }

    [EWToolBar("工具/Test3")]
    private void Test3()
    {
        ShowMsgBox((int) TestMsg.Msg1, null);
    }


    [EWToolBar("工具/Test4")]
    private void Test4()
    {
        ShowMsgBox((int)TestMsg.Msg2, "参数xxxx");
    }

    [EWMsgBox((int) TestMsg.Msg1, 0.2f, 0.2f, 0.6f, 0.6f)]
    private void Msg1(Rect rect, System.Object obj)
    {
        if (GUI.Button(new Rect(rect.x, rect.y, rect.width, 20),"关闭"))
        {
            HideMsgBox();
        }
    }

    [EWMsgBox((int)TestMsg.Msg2, 100, 100, 200, 200, true, true, true, true)]
    private void Msg2(Rect rect, System.Object obj)
    {
        if (obj != null)
        {
            string arg = (string) obj;
            GUI.Label(new Rect(rect.x, rect.y, rect.width, 20), "参数：" + arg);
        }
        if (GUI.Button(new Rect(rect.x, rect.y + rect.height - 20, rect.width, 20), "关闭"))
        {
            HideMsgBox();
        }
    }
}
