using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// SubWindow基本范例
/// </summary>
public class TestWinE : MDIEditorWindow {

    [MenuItem("Test/TestWinE")]
    static void Init()
    {
        TestWinE win = TestWinA.CreateWindow<TestWinE>();
    }

    [SubWindow("SunWinA", SubWindowIcon.Game)]
    private void SubWinA(Rect main)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "SubWinA");
    }

    [SubWindow("SunWinB", SubWindowIcon.Project)]
    private void SubWinB(Rect main)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "SubWinB");
    }

    public void TestFunc()
    {
        Debug.Log("从SubWindow访问");
    }
}

[SubWindowClass(typeof(TestWinE))]
class TestDrawerA : SubWindowCustomObjectDrawer
{

    public override GUIContent Title
    {
        get { return m_Title; }
    }

    public override SubWindowToolbarType toolBar
    {
        get { return m_ToolBar; }
    }

    private SubWindowToolbarType m_ToolBar = SubWindowToolbarType.None;

    private GUIContent m_Title;

    public TestDrawerA()
    {
        m_Title = new GUIContent("DefaultTitle");
    }

    public override void DrawMainWindow(Rect mainRect)
    {
        base.DrawMainWindow(mainRect);
        if (GUI.Button(new Rect(mainRect.x, mainRect.y, mainRect.width, 20), "改变标题"))
        {
            m_Title = new GUIContent("新的标题");
        }
        if (GUI.Button(new Rect(mainRect.x, mainRect.y + 20, mainRect.width, 20), "显示Toolbar"))
        {
            m_ToolBar = SubWindowToolbarType.Normal;
        }
        if (GUI.Button(new Rect(mainRect.x, mainRect.y + 40, mainRect.width, 20), "关闭Toolbar"))
        {
            m_ToolBar = SubWindowToolbarType.None;
        }
        if (GUI.Button(new Rect(mainRect.x, mainRect.y + 60, mainRect.width, 20), "显示HelpBox"))
        {
            SetSubWindowHelpBoxType(SubWindowHelpBoxType.Locker);
        }
        if (GUI.Button(new Rect(mainRect.x, mainRect.y + 80, mainRect.width, 20), "关闭HelpBox"))
        {
            SetSubWindowHelpBoxType(SubWindowHelpBoxType.None);
        }
        if (GUI.Button(new Rect(mainRect.x, mainRect.y + 100, mainRect.width, 20), "访问容器窗体"))
        {
            ((TestWinE) container).TestFunc();
        }
    }
}
