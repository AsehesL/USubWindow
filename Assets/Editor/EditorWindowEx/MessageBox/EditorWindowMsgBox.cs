using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;
using EditorWinEx;
using EditorWinEx.Internal;

public delegate void DrawActionUseObj(Rect rect, System.Object obj);

/// <summary>
/// MsgBox组件
/// </summary>
public class EditorWindowMsgBox : EditorWindowComponentBase
{
    private Dictionary<int, EWMsgBoxDrawer> m_MsgBoxs = new Dictionary<int, EWMsgBoxDrawer>();

    public bool IsShowing
    {
        get { return m_IsShowing; }
    }

    private bool m_IsShowing;

    private int m_CurrentShowId = -1;

    private System.Object m_Obj;

    /// <summary>
    /// 添加MsgBox
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="method">绘制方法</param>
    /// <param name="target">绘制方法对象</param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void AddMsgBox(int id, MethodInfo method, System.Object target, EWRectangle rectangle)
    {
        if (m_MsgBoxs.ContainsKey(id))
        {
            Debug.LogError("错误,已经包含该ID的MsgBox方法:" + id);
            return;
        }
        EWMsgBoxMethodDrawer msgbox = new EWMsgBoxMethodDrawer(method, target, rectangle);
        msgbox.Init();
        m_MsgBoxs.Add(id, msgbox);
    }

    /// <summary>
    /// 添加MsgBox
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="drawer">自定义绘制器对象</param>
    public void AddMsgBox(int id, EWMsgBoxCustomDrawer drawer)
    {
        if (m_MsgBoxs.ContainsKey(id))
        {
            Debug.LogError("错误,已经包含该ID的MsgBox方法:" + id);
            return;
        }
        EWMsgBoxObjectDrawer msgbox = new EWMsgBoxObjectDrawer(drawer);
        msgbox.Init();
        m_MsgBoxs.Add(id, msgbox);
    }

    public void DrawMsgBox(Rect rect)
    {
        if (!m_IsShowing)
            return;
        if (!m_MsgBoxs.ContainsKey(m_CurrentShowId))
            return;
        var msgBox = m_MsgBoxs[m_CurrentShowId];
        msgBox.DrawMsgBox(rect, m_Obj);
        return;
    }

    public void ShowMsgBox(int id, System.Object obj)
    {
        if (m_MsgBoxs.ContainsKey(id))
        {
            m_Obj = obj;
            m_CurrentShowId = id;
            m_IsShowing = true;
            m_MsgBoxs[id].Enable();
        }
    }

    public void HideMsgBox()
    {
        m_IsShowing = false;
        if (m_MsgBoxs.ContainsKey(m_CurrentShowId))
        {
            m_MsgBoxs[m_CurrentShowId].Disable();
        }
    }

    protected override void OnRegisterMethod(System.Object container, MethodInfo method, System.Object target)
    {
        System.Object[] atts = method.GetCustomAttributes(typeof(EWMsgBoxAttribute), false);
        ParameterInfo[] parameters = method.GetParameters();
        if (atts != null && parameters.Length == 2 && parameters[0].ParameterType == typeof(Rect) && parameters[1].ParameterType == typeof(System.Object))
        {
            for (int j = 0; j < atts.Length; j++)
            {
                EWMsgBoxAttribute att = (EWMsgBoxAttribute)atts[j];
                AddMsgBox(att.id, method, target, att.Rectangle);
            }
        }
    }

    protected override void OnRegisterClass(System.Object container, Type type)
    {
        if (container == null)
            return;
        if (!type.IsSubclassOf(typeof(EWMsgBoxCustomDrawer)))
            return;
        System.Object[] atts = type.GetCustomAttributes(typeof(EWMsgBoxHandleAttribute), false);
        for (int i = 0; i < atts.Length; i++)
        {
            EWMsgBoxHandleAttribute att = (EWMsgBoxHandleAttribute)atts[i];
            if (att == null)
                continue;
            if (att.targetType != container.GetType())
                continue;
            EWMsgBoxCustomDrawer drawer = (EWMsgBoxCustomDrawer) System.Activator.CreateInstance(type);
            if (drawer == null)
                continue;
            drawer.SetContainer(container);
            drawer.closeAction = HideMsgBox;
            AddMsgBox(att.id, drawer);
        }
    }

    protected override void OnInit()
    {
    }

    protected override void OnDestroy()
    {
        foreach(var kvp in m_MsgBoxs)
        {
            if (kvp.Value != null)
            {
                kvp.Value.Destroy();
            }
        }
    }
}
