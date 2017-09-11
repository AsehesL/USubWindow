using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using EditorWinEx.Internal;

public delegate void SubWindowAction(Rect rect);

public delegate void SubWindowActionHalf(Rect rect, Rect subrect);

public delegate void SubWindowActionFull(Rect rect, Rect toolBar, Rect helpBox);

/// <summary>
/// MDIEditorWindow-可单独传入Handle打开，也可扩展该类
/// </summary>
public class MDIEditorWindow : EditorWindow
{
    /// <summary>
    /// 进行绘制的handle对象（可以是任何能通过构造函数构造的类型）
    /// </summary>
    protected System.Object handle;
    /// <summary>
    /// 窗口树
    /// </summary>
    private SubWindowTree m_WindowTree;
    /// <summary>
    /// 工具栏树
    /// </summary>
    private ToolBarTree m_ToolbarTree;
    /// <summary>
    /// 消息界面
    /// </summary>
    private EditorWindowMsgBox m_MsgBox;

    private bool m_IsInitialized;

    /// <summary>
    /// 窗口创建方法
    /// </summary>
    /// <typeparam name="T">窗口类型</typeparam>
    /// <param name="handle">实现绘制的Handle对象</param>
    /// <returns></returns>
    public static T CreateWindow<T>(System.Object handle = null) where T : MDIEditorWindow
    {
        T window = MDIEditorWindow.GetWindow<T>();
        window.handle = handle;
        window.Clear();
        window.Init();
        window.m_IsInitialized = true;
        return window;
    }

    /// <summary>
    /// 窗口创建方法
    /// </summary>
    /// <typeparam name="T">窗口类型</typeparam>
    /// <param name="type">handle对象类型（只要可以通过构造函数创建都可以）</param>
    /// <param name="args">handle对象的构造函数参数</param>
    /// <returns></returns>
    public static T CreateWindow<T>(System.Type type, params System.Object[] args) where T : MDIEditorWindow
    {
        System.Object obj = null;
        if (type != null)
            obj = System.Activator.CreateInstance(type, args);
        return CreateWindow<T>(obj);
    }

    void OnGUI()
    {
        bool guienable = GUI.enabled;
        if (m_MsgBox != null && m_MsgBox.IsShowing)
            GUI.enabled = false;
        else
            GUI.enabled = true;

        GUI.BeginGroup(new Rect(0, 0, position.width, position.height), GUIStyleCache.GetStyle("LODBlackBox"));
        OnDrawGUI();
        GUI.EndGroup();

        GUI.enabled = guienable;
        DrawMsgBox(new Rect(0, 0, position.width, position.height));
    }

    /// <summary>
    /// 显示消息界面
    /// </summary>
    /// <param name="id">界面ID</param>
    /// <param name="obj">消息传递对象</param>
    public void ShowMsgBox(int id, System.Object obj)
    {
        if (m_MsgBox != null)
            m_MsgBox.ShowMsgBox(id, obj);
    }

    /// <summary>
    /// 隐藏消息界面
    /// </summary>
    public void HideMsgBox()
    {
        if (m_MsgBox != null)
            m_MsgBox.HideMsgBox();
    }

    /// <summary>
    /// 打开SubWindow
    /// </summary>
    /// <param name="id"></param>
    public void OpenSubWindow(string id)
    {
        SetSubWindowActive(id, true);
    }

    public void OpenSubWindow(SubWindowAction action)
    {
        SetSubWindowActive(action, true);
    }

    public void OpenSubWindow(SubWindowActionHalf action)
    {
        SetSubWindowActive(action, true);
    }

    public void OpenSubWindow(SubWindowActionFull action)
    {
        SetSubWindowActive(action, true);
    }

    /// <summary>
    /// 关闭SubWindow
    /// </summary>
    /// <param name="id"></param>
    public void CloseSubWindow(string id)
    {
        SetSubWindowActive(id, false);
    }

    public void CloseSubWindow(SubWindowAction action)
    {
        SetSubWindowActive(action, false);
    }

    public void CloseSubWindow(SubWindowActionHalf action)
    {
        SetSubWindowActive(action, false);
    }

    public void CloseSubWindow(SubWindowActionFull action)
    {
        SetSubWindowActive(action, false);
    }

    /// <summary>
    /// 动态添加子窗体
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="icon">图标</param>
    /// <param name="action">方法</param>
    public void AddDynamicSubWindow(string title, string icon, SubWindowAction action)
    {
        this.AddDynamicSubWindowInternal(title, icon, SubWindowToolbarType.None, SubWindowHelpBoxType.None, action);
    }

    /// <summary>
    /// 动态添加子窗体
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="icon">图标</param>
    /// <param name="action">方法</param>
    public void AddDynamicSubWindow(string title, SubWindowIcon icon, SubWindowAction action)
    {
        this.AddDynamicSubWindowInternal(title, icon, SubWindowToolbarType.None, SubWindowHelpBoxType.None, action);
    }

    /// <summary>
    /// 添加带工具栏的子窗体
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="icon">图标</param>
    /// <param name="action">方法</param>
    public void AddDynamicSubWindowWithToolBar(string title, string icon, SubWindowToolbarType toolbar, SubWindowActionHalf action)
    {
        if (toolbar == SubWindowToolbarType.None)
            return;
        this.AddDynamicSubWindowInternal(title, icon, toolbar, SubWindowHelpBoxType.None, action);
    }

    /// <summary>
    /// 添加带工具栏的子窗体
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="icon">图标</param>
    /// <param name="action">方法</param>
    public void AddDynamicSubWindowWithToolBar(string title, SubWindowIcon icon, SubWindowToolbarType toolbar, SubWindowActionHalf action)
    {
        if (toolbar == SubWindowToolbarType.None)
            return;
        this.AddDynamicSubWindowInternal(title, icon, toolbar, SubWindowHelpBoxType.None, action);
    }

    /// <summary>
    /// 添加带帮助栏的子窗体
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="icon">图标</param>
    /// <param name="helpBoxType">帮助栏</param>
    /// <param name="action">方法</param>
    public void AddDynamicSubWindowWithHelpBox(string title, string icon, SubWindowHelpBoxType helpBoxType,
        SubWindowActionHalf action)
    {
        if (helpBoxType == SubWindowHelpBoxType.None)
            return;
        this.AddDynamicSubWindowInternal(title, icon, SubWindowToolbarType.None, helpBoxType, action);
    }

    /// <summary>
    /// 添加带帮助栏的子窗体
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="icon">图标</param>
    /// <param name="helpBoxType">帮助栏</param>
    /// <param name="action">方法</param>
    public void AddDynamicSubWindowWithHelpBox(string title, SubWindowIcon icon, SubWindowHelpBoxType helpBoxType,
        SubWindowActionHalf action)
    {
        if (helpBoxType == SubWindowHelpBoxType.None)
            return;
        this.AddDynamicSubWindowInternal(title, icon, SubWindowToolbarType.None, helpBoxType, action);
    }

    /// <summary>
    /// 添加完整窗口
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="icon">图标</param>
    /// <param name="helpBoxType">帮助栏</param>
    /// <param name="action">方法</param>
    public void AddDynamicFullSubWindow(string title, string icon, SubWindowToolbarType toolbar, SubWindowHelpBoxType helpBoxType,
        SubWindowActionFull action)
    {
        if (helpBoxType == SubWindowHelpBoxType.None)
            return;
        if (toolbar == SubWindowToolbarType.None)
            return;
        this.AddDynamicSubWindowInternal(title, icon, toolbar, helpBoxType, action);
    }

    /// <summary>
    /// 添加完整窗口
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="icon">图标</param>
    /// <param name="helpBoxType">帮助栏</param>
    /// <param name="action">方法</param>
    public void AddDynamicFullSubWindow(string title, SubWindowIcon icon, SubWindowToolbarType toolbar, SubWindowHelpBoxType helpBoxType,
        SubWindowActionFull action)
    {
        if (helpBoxType == SubWindowHelpBoxType.None)
            return;
        if (toolbar == SubWindowToolbarType.None)
            return;
        this.AddDynamicSubWindowInternal(title, icon, toolbar, helpBoxType, action);
    }

    /// <summary>
    /// 移除动态窗口
    /// </summary>
    /// <param name="action"></param>
    public void RemoveDynamicSubWindow(SubWindowAction action)
    {
        this.RemoveDynamicSubWindowInternal(action);
    }

    /// <summary>
    /// 移除动态窗口
    /// </summary>
    /// <param name="action"></param>
    public void RemoveDynamicSubWindow(SubWindowActionHalf action)
    {
        this.RemoveDynamicSubWindowInternal(action);
    }

    /// <summary>
    /// 移除动态窗口
    /// </summary>
    /// <param name="action"></param>
    public void RemoveDynamicSubWindow(SubWindowActionFull action)
    {
        this.RemoveDynamicSubWindowInternal(action);
    }

    /// <summary>
    /// 移除所有动态窗口
    /// </summary>
    public void RemoveAllDynamicSubWindow()
    {
        if (m_WindowTree != null)
        {
            m_WindowTree.RemoveAllDynamicWindow();
        }
    }

    protected virtual void OnEnable()
    {
        if (m_IsInitialized)
            Init();
    }

    protected virtual void OnProjectChange()
    {
        Init();
    }

    protected virtual void OnDisable()
    {
    }

    protected virtual void OnDestroy()
    {
        if (m_WindowTree != null)
        {
            m_WindowTree.Destroy();
            m_WindowTree = null;
        }
        if (m_ToolbarTree != null)
        {
            m_ToolbarTree.Destroy();
            m_ToolbarTree = null;
        }
        if (m_MsgBox != null)
        {
            m_MsgBox.Destroy();
            m_MsgBox = null;
        }
    }

    protected virtual void Clear()
    {
        if (m_WindowTree != null)
        {
            m_WindowTree.Destroy();
            m_WindowTree = null;
        }
        if (m_ToolbarTree != null)
        {
            m_ToolbarTree.Destroy();
            m_ToolbarTree = null;
        }
        if (m_MsgBox != null)
        {
            m_MsgBox.Destroy();
            m_MsgBox = null;
        }
    }

    protected virtual void Init()
    {
        if (m_WindowTree == null)
        {
            if (handle != null)
                m_WindowTree = new SubWindowTree(Repaint, GetType().Name, handle.GetType().Name);
            else
                m_WindowTree = new SubWindowTree(Repaint, GetType().Name, null);
        }
        if (m_ToolbarTree == null)
        {
            m_ToolbarTree = new ToolBarTree();
        }
        if (m_MsgBox == null)
        {
            m_MsgBox = new EditorWindowMsgBox();
        }
        Type[] handleTypes = null;
        System.Object[] handles = null;
        if (handle != null)
        {
            handleTypes = new Type[] {handle.GetType(), GetType()};
            handles = new object[] {handle, this};
        }
        else
        {
            handleTypes = new Type[] {GetType()};
            handles = new object[] {this};
        }
        EditorWindowToolsInitializer.InitTools(this, handleTypes, handles, m_WindowTree, m_ToolbarTree, m_MsgBox);
    }

    protected virtual void OnDrawGUI()
    {
        DrawWindowTree(new Rect(0, 17, position.width, position.height - 17));//绘制子窗口树
        DrawToolbar(new Rect(0, 0, position.width, 17));//绘制工具栏树
    }


    /// <summary>
    /// 绘制窗口树
    /// </summary>
    /// <param name="rect"></param>
    protected void DrawWindowTree(Rect rect)
    {
        if (m_WindowTree != null)
        {
            m_WindowTree.DrawWindowTree(rect);
        }
    }

    /// <summary>
    /// 绘制工具栏
    /// </summary>
    /// <param name="rect"></param>
    protected void DrawToolbar(Rect rect)
    {
        GUI.Box(rect, string.Empty, GUIStyleCache.GetStyle("Toolbar"));
        if (m_WindowTree != null)
        {
            m_WindowTree.DrawLayoutButton(new Rect(rect.x + rect.width - 80, rect.y, 70, rect.height));
        }
        GUILayout.BeginArea(new Rect(rect.x + 10, rect.y, rect.width - 100, rect.height));
        GUILayout.BeginHorizontal();
        if (m_ToolbarTree != null)
        {
            m_ToolbarTree.DrawToolbar();
        }
        if (m_WindowTree != null)
        {
            m_WindowTree.DrawViewButton();
        }
        OnDrawToolBar();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    protected void DrawMsgBox(Rect rect)
    {
        if (m_MsgBox == null)
            return;
        m_MsgBox.DrawMsgBox(rect);
    }

    /// <summary>
    /// 绘制工具栏-用于子类扩展工具栏控件的绘制
    /// </summary>
    protected virtual void OnDrawToolBar()
    {

    }

    private void SetSubWindowActive(string windowId, bool active)
    {
        if (m_WindowTree != null && !string.IsNullOrEmpty(windowId))
        {
            m_WindowTree.SetSubWindowActive(windowId, active);
        }
    }

    private void SetSubWindowActive(Delegate action, bool active)
    {
        if (m_WindowTree != null && action != null)
        {
            m_WindowTree.SetSubWindowActive(action.Method, action.Target, active);
        }
    }

    private void AddDynamicSubWindowInternal(string title, SubWindowIcon icon, SubWindowToolbarType toolbar, SubWindowHelpBoxType helpbox,
        Delegate action)
    {
        this.AddDynamicSubWindowInternal(title, GUIEx.GetIconPath(icon), toolbar, helpbox, action);
    }

    private void AddDynamicSubWindowInternal(string title, string icon, SubWindowToolbarType toolbar, SubWindowHelpBoxType helpbox,
        Delegate action)
    {
        if (m_WindowTree != null)
        {
            string id = action.Target.GetType().FullName + "." + action.Method.Name;
            if (m_WindowTree.ContainWindowID(id))
                return;
            SubWindow window = new SubWindow(title, icon, true, action.Method, action.Target, toolbar, helpbox);
            window.isDynamic = true;
            this.m_WindowTree.AddWindow(window, true);
        }
    }

    private void RemoveDynamicSubWindowInternal(Delegate action)
    {
        if (m_WindowTree != null)
        {
            string id = action.Target.GetType().FullName + "." + action.Method.Name;
            this.m_WindowTree.RemoveWindowByID(id);
        }
    }

}
