using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// 真正的子窗口类型-用于实际绘制
/// </summary>
[SubWindowStyle(SubWindowStyle.Default)]
public class SubWindow
{
    /// <summary>
    /// 标题
    /// </summary>
    public GUIContent Title { get { return this.m_Title; } }

    /// <summary>
    /// 默认开启状态
    /// </summary>
    public bool DefaultOpen { get; private set; }

    public bool isOpen;

    public bool isDynamic;

    private GUIContent m_Title;

    /// <summary>
    /// 关闭事件
    /// </summary>
    private System.Action<SubWindow> m_OnClose;

    /// <summary>
    /// 绘制函数参数列表
    /// </summary>
    private System.Object[] m_Params;

    /// <summary>
    /// 绘制函数
    /// </summary>
    private MethodInfo m_Method;

    /// <summary>
    /// 绘制函数响应目标
    /// </summary>
    private System.Object m_Target;

    private SubWindowToolbarType m_ToolBar;

    private string m_ID;

    /// <summary>
    /// 帮助栏
    /// </summary>
    private SubWindowHelpBox m_HelpBox = null;

    public SubWindow(string title, string icon, bool defaultOpen, MethodInfo method, System.Object target,
        SubWindowToolbarType toolbar, SubWindowHelpBoxType helpbox)
    {
        this.m_Title = CreateTitle(title, icon);
        this.m_Method = method;
        this.m_ToolBar = toolbar;
        this.m_HelpBox = SubWindowHelpBox.CreateHelpBox(helpbox);
        this.m_Target = target;
        if (target == null && method == null)
            this.m_ID = "UnKnownClass.UnKnownMethod";
        else if (target == null)
            this.m_ID = "UnKnownClass." + method.Name;
        else if (method == null)
            this.m_ID = target.GetType().FullName + ".UnKnownMethod";
        else
            this.m_ID = target.GetType().FullName + "." + method.Name;
        this.DefaultOpen = defaultOpen;
        if (this.m_Method != null)
        {
            ParameterInfo[] p = this.m_Method.GetParameters();
            m_Params = new System.Object[p.Length];
        }
    }

    /// <summary>
    /// 获得窗口的唯一标识，用于导出布局文件后唯一标识该窗口
    /// </summary>
    /// <returns></returns>
    public string GetIndentifier()
    {
        //return m_Method.GetHashCode().ToString();
        return m_ID;
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
        Rect tb = DrawToolBar(ref rect);
        Rect hb = DrawHelpBox(ref rect);
        Rect mb = DrawMainArea(rect);
        Invoke(m_Target, mb, tb, hb);
    }

    /// <summary>
    /// 关闭窗口
    /// </summary>
    public void Close()
    {
        if (!isOpen)
            return;
        isOpen = false;
        if (m_OnClose != null)
        {
            m_OnClose(this);
        }
    }

    /// <summary>
    /// 执行绘制方法
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="mainRect"></param>
    /// <param name="toolbarRect"></param>
    /// <param name="helpboxRect"></param>
    private void Invoke(System.Object obj, Rect mainRect, Rect toolbarRect, Rect helpboxRect)
    {
        if (m_Method != null)
        {
            if (m_Params.Length > 0)
                m_Params[0] = mainRect;
            if (m_Params.Length > 1)
                if (m_ToolBar == SubWindowToolbarType.None)
                    m_Params[1] = helpboxRect;
                else
                    m_Params[1] = toolbarRect;
            if (m_Params.Length > 2)
                m_Params[2] = helpboxRect;
            m_Method.Invoke(obj, m_Params);
        }
    }

    protected virtual Rect DrawMainArea(Rect rect)
    {
        return rect;
    }

    private Rect DrawToolBar(ref Rect rect)
    {
        if (m_ToolBar == SubWindowToolbarType.Normal)
        {
            Rect h = new Rect(rect.x, rect.y, rect.width, 18);
            rect = new Rect(rect.x, rect.y + 18, rect.width, rect.height - 18);
            GUI.Box(h, string.Empty, GUIStyleCache.GetStyle("Toolbar"));
            return h;
        }
        else if (m_ToolBar == SubWindowToolbarType.Mini)
        {
            Rect h = new Rect(rect.x, rect.y, rect.width, 15);
            rect = new Rect(rect.x, rect.y + 15, rect.width, rect.height - 15);
            GUI.Box(h, string.Empty, GUIStyleCache.GetStyle("MiniToolbarButton"));
            return h;
        }
        return new Rect(rect.x, rect.y, 0, 0);
    }

    private Rect DrawHelpBox(ref Rect rect)
    {
        if (m_HelpBox != null)
        {
            Rect h = m_HelpBox.DrawHelpBox(ref rect);
            return h;
        }
        return new Rect(rect.x, rect.y, 0, 0);
    }

    private GUIContent CreateTitle(string title, string icon)
    {
        if (string.IsNullOrEmpty(icon))
            return new GUIContent(title);
        Texture2D tex = EditorGUIUtility.FindTexture(icon);
        if (tex == null)
            return new GUIContent(title);
        return new GUIContent(title, tex);
    }
    
}
