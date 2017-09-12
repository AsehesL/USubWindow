using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// SubWindow自定义SubWindow对象范例
/// </summary>
public class TestWinE : MDIEditorWindow {

    [MenuItem("SubWindow范例/5.自定义SubWindow对象范例")]
    static void Init()
    {
        TestWinE win = TestWinA.CreateWindow<TestWinE>();
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

    public void TestFunc()
    {
        Debug.Log("从SubWindow访问");
    }
}

[EWSubWindowHandle(typeof(TestWinE))]
class TestDrawerA : SubWindowCustomDrawer
{

    public override GUIContent Title
    {
        get { return m_Title; }
    }

    public override EWSubWindowToolbarType toolBar
    {
        get { return m_ToolBar; }
    }

    private EWSubWindowToolbarType m_ToolBar = EWSubWindowToolbarType.None;

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
            m_ToolBar = EWSubWindowToolbarType.Normal;
        }
        if (GUI.Button(new Rect(mainRect.x, mainRect.y + 40, mainRect.width, 20), "关闭Toolbar"))
        {
            m_ToolBar = EWSubWindowToolbarType.None;
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

    public override void Init()
    {
        base.Init();
        Debug.Log("Init");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        Debug.Log("Destroy");
    }

    public override void OnDisable()
    {
        base.OnDisable();
        Debug.Log("Disbale");
    }

    public override void OnEnable()
    {
        base.OnEnable();
        Debug.Log("Enalbe");
    }
   
}
