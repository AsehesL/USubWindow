using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;
using EditorWinEx;
using EditorWinEx.Internal;

public delegate void DrawActionUseObj(Rect rect, System.Object obj);

public class EditorWindowMsgBox : EditorWindowTool
{
    private Dictionary<int, EWMsgBoxDrawer> m_MsgBoxs = new Dictionary<int, EWMsgBoxDrawer>();

    public bool IsShowing
    {
        get { return m_IsShowing; }
    }

    private bool m_IsShowing;

    private int m_CurrentShowId = -1;

    private System.Object m_Obj;

    public void AddMsgBox(int id, MethodInfo method, System.Object target, float x, float y, float width,
        float height)
    {
        if (m_MsgBoxs.ContainsKey(id))
        {
            Debug.LogError("错误,已经包含该ID的MsgBox方法:" + id);
            return;
        }
        EWMsgBoxMethodDrawer msgbox = new EWMsgBoxMethodDrawer(method, target, x, y, width, height);
        msgbox.Init();
        m_MsgBoxs.Add(id, msgbox);
    }

    public void AddMsgBox(int id, EWMsgBoxCustomObjectDrawer drawer)
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
        System.Object[] atts = method.GetCustomAttributes(typeof(MsgBoxAttribute), false);
        ParameterInfo[] parameters = method.GetParameters();
        if (atts != null && parameters.Length == 2 && parameters[0].ParameterType == typeof(Rect) && parameters[1].ParameterType == typeof(System.Object))
        {
            for (int j = 0; j < atts.Length; j++)
            {
                MsgBoxAttribute att = (MsgBoxAttribute)atts[j];
                AddMsgBox(att.id, method, target, att.x, att.y, att.width, att.height);
            }
        }
    }

    protected override void OnRegisterClass(System.Object container, Type type)
    {
        if (container == null)
            return;
        if (!type.IsSubclassOf(typeof(EWMsgBoxCustomObjectDrawer)))
            return;
        System.Object[] atts = type.GetCustomAttributes(typeof(MsgBoxHandleAttribute), false);
        for (int i = 0; i < atts.Length; i++)
        {
            MsgBoxHandleAttribute att = (MsgBoxHandleAttribute)atts[i];
            if (att == null)
                continue;
            if (att.targetType != container.GetType())
                continue;
            EWMsgBoxCustomObjectDrawer drawer = (EWMsgBoxCustomObjectDrawer) System.Activator.CreateInstance(type);
            if (drawer == null)
                continue;
            drawer.container = container;
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
