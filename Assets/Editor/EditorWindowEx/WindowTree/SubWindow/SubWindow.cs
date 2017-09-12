using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using EditorWinEx;
using EditorWinEx.Internal;

/// <summary>
/// 真正的子窗口类型-用于实际绘制
/// </summary>
[SubWindowStyle(SubWindowStyle.Default)]
public class SubWindow
{
    /// <summary>
    /// 标题
    /// </summary>
    public GUIContent Title
    {
        get { return this.m_Drawer.Title; }
    }

    /// <summary>
    /// 默认开启状态
    /// </summary>
    public bool DefaultOpen { get; private set; }

    public bool IsOpen { get; private set; }

    public bool isDynamic;

    /// <summary>
    /// 关闭事件
    /// </summary>
    private System.Action<SubWindow> m_OnClose;

    private SubWindowDrawerBase m_Drawer;

    public SubWindow(string title, string icon, bool defaultOpen, MethodInfo method, System.Object target,
        EWSubWindowToolbarType toolbar, SubWindowHelpBoxType helpbox)
    {
        this.DefaultOpen = defaultOpen;
        this.m_Drawer = new SubWindowMethodDrawer(title, icon, method, target, toolbar, helpbox);
        this.m_Drawer.Init();
    }

    public SubWindow(bool defaultOpen, SubWindowCustomDrawer drawer)
    {
        this.DefaultOpen = defaultOpen;
        this.m_Drawer = new SubWindowObjectDrawer(drawer);
        this.m_Drawer.Init();
    }

    /// <summary>
    /// 获得窗口的唯一标识，用于导出布局文件后唯一标识该窗口
    /// </summary>
    /// <returns></returns>
    public string GetIndentifier()
    {
        if (m_Drawer != null)
            return m_Drawer.Id;
        return "Unknown.UnknownId";
    }

    /// <summary>
    /// 注册关闭事件
    /// </summary>
    /// <param name="onClose"></param>
    public void AddCloseEventListener(System.Action<SubWindow> onClose)
    {
        this.m_OnClose = onClose;
    }

    /// <summary>
    /// 绘制窗口
    /// </summary>
    /// <param name="rect"></param>
    public void DrawSubWindow(Rect rect)
    {
        Rect tb = m_Drawer.DrawToolBar(ref rect);
        Rect hb = m_Drawer.DrawHelpBox(ref rect);
        Rect mb = DrawMainArea(rect);
        m_Drawer.DrawWindow(mb, tb, hb);
    }

    /// <summary>
    /// 关闭窗口
    /// </summary>
    public void Close()
    {
        if (!IsOpen)
            return;
        IsOpen = false;
        if (m_OnClose != null)
        {
            m_OnClose(this);
        }
        m_Drawer.Disable();
    }

    public void Open()
    {
        if (IsOpen)
            return;
        IsOpen = true;
        m_Drawer.Enable();
    }

    public void Destroy()
    {
        IsOpen = false;
        m_Drawer.Destroy();
    }

    protected virtual Rect DrawMainArea(Rect rect)
    {
        return rect;
    }
    
}
