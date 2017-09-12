using UnityEngine;
using UnityEditor;
using System.Collections;
using EditorWinEx.Internal;

/// <summary>
/// SubWindow自定义窗体、主窗体间的消息通信范例
/// </summary>
public class TestWinG : MDIEditorWindow {

    public enum TestWinGMessageID
    {
        FromWin1,
        FromWin2,
        NotifyMainWindow,
    }

    [MenuItem("SubWindow范例/7.自定义窗体、主窗体间的消息通信范例")]
    static void Init()
    {
        TestWinG win = TestWinA.CreateWindow<TestWinG>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.AddListener<string>((int) TestWinGMessageID.NotifyMainWindow, this.OnListenEvent);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.RemoveListener<string>((int)TestWinGMessageID.NotifyMainWindow, this.OnListenEvent);
    }

    private void OnListenEvent(string winName)
    {
        Debug.LogFormat("收到窗口：{0}的消息", winName);
    }
}

[EWSubWindowHandle(typeof(TestWinG))]
class TestDrawerB : SubWindowCustomDrawer
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

    private float m_Number;

    public TestDrawerB()
    {
        m_Title = new GUIContent("窗口1");
    }

    public override void OnEnable()
    {
        base.OnEnable();

        this.AddListener<float>((int)TestWinG.TestWinGMessageID.FromWin2, this.OnListenEvent);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        this.RemoveListener<float>((int)TestWinG.TestWinGMessageID.FromWin2, this.OnListenEvent);
    }

    public override void DrawMainWindow(Rect mainRect)
    {
        base.DrawMainWindow(mainRect);
        if (GUI.Button(new Rect(mainRect.x, mainRect.y, mainRect.width, 20), "向窗口2发送消息"))
        {
            this.Broadcast((int)TestWinG.TestWinGMessageID.FromWin1);
        }
        if (GUI.Button(new Rect(mainRect.x, mainRect.y + 20, mainRect.width, 20), "向主容器窗体发送消息"))
        {
            this.Broadcast<string>((int)TestWinG.TestWinGMessageID.NotifyMainWindow, m_Title.text);
        }
        GUI.Label(new Rect(mainRect.x, mainRect.y + 40, mainRect.width, 20), "收到窗口2的消息内容：" + m_Number);
    }

    private void OnListenEvent(float number)
    {
        this.m_Number = number;
        Debug.Log("收到来自窗口2的消息");
    }
   
}

[EWSubWindowHandle(typeof(TestWinG))]
class TestDrawerC : SubWindowCustomDrawer
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

    private int m_MessageCount;

    public TestDrawerC()
    {
        m_Title = new GUIContent("窗口2");
    }

    public override void OnEnable()
    {
        base.OnEnable();
        this.AddListener((int)TestWinG.TestWinGMessageID.FromWin1, this.OnListenEvent);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        this.RemoveListener((int)TestWinG.TestWinGMessageID.FromWin1, this.OnListenEvent);
    }

    public override void DrawMainWindow(Rect mainRect)
    {
        base.DrawMainWindow(mainRect);
        if (GUI.Button(new Rect(mainRect.x, mainRect.y, mainRect.width, 20), "向窗口1发送消息"))
        {
            this.Broadcast<float>((int) TestWinG.TestWinGMessageID.FromWin2, Random.Range(0f, 100f));
        }
        if (GUI.Button(new Rect(mainRect.x, mainRect.y + 20, mainRect.width, 20), "向主容器窗体发送消息"))
        {
            this.Broadcast<string>((int)TestWinG.TestWinGMessageID.NotifyMainWindow, m_Title.text);
        }
        GUI.Label(new Rect(mainRect.x, mainRect.y + 40, mainRect.width, 20), "收到窗口1的消息次数：" + m_MessageCount);
    }

    private void OnListenEvent()
    {
        this.m_MessageCount++;
        Debug.Log("收到来自窗口1的消息");
    }
}