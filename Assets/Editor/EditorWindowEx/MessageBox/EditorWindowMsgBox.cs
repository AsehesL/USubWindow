using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;

public delegate void DrawActionUseObj(Rect rect, System.Object obj);

public class EditorWindowMsgBox : EditorWindowTool
{
    private Dictionary<int, MsgBox> m_MsgBoxs = new Dictionary<int, MsgBox>();

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
        MsgBox msgbox = new MsgBox(method, target, this, x, y, width, height);
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
        }
    }

    public void HideMsgBox()
    {
        m_IsShowing = false;
    }

    protected override void OnRegisterMethod(System.Object container, MethodInfo method, System.Object target, bool isStatic)
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
    }

    protected override void OnInit()
    {
    }

    protected override void OnDestroy()
    {
    }

    private class MsgBox
    {

        private readonly float m_X;
        private readonly float m_Y;
        private readonly float m_Width;
        private readonly float m_Height;

        private DrawActionUseObj m_DrawAction;

        private EditorWindowMsgBox m_Root;

        private bool m_IsStatic;

        public MsgBox(MethodInfo method, System.Object target, EditorWindowMsgBox root, float x, float y, float width, float height)
        {
            if (method != null)
                m_DrawAction = Delegate.CreateDelegate(typeof (DrawActionUseObj), target, method) as DrawActionUseObj;
            this.m_X = x;
            this.m_Y = y;
            this.m_Width = width;
            this.m_Height = height;
            this.m_IsStatic = target == null;
            this.m_Root = root;
        }

        public void DrawMsgBox(Rect rect, System.Object obj)
        {
            DrawMask(rect);
            Rect main = new Rect(rect.x + rect.width * m_X, rect.y + rect.height * m_Y, rect.width * m_Width, rect.height * m_Height);
            if (m_IsStatic)
                DrawGlobalBox(main, obj);
            else
                DrawBox(main, obj);
        }

        private void DrawBox(Rect main, System.Object obj)
        {
            GUI.Box(main, "", GUIStyleCache.GetStyle("WindowBackground"));
            if (m_DrawAction != null)
                m_DrawAction(main, obj);
        }

        private void DrawGlobalBox(Rect main, System.Object obj)
        {
            GUI.Box(main, string.Empty, GUIStyleCache.GetStyle("WindowBackground"));
            GUI.Box(new Rect(main.x, main.y, main.width, 18), string.Empty, GUIStyleCache.GetStyle("Toolbar"));
            if (GUI.Button(new Rect(main.x + main.width - 21, main.y + 4, 13, 11), string.Empty, GUIStyleCache.GetStyle("WinBtnClose")))
            {
                if (m_Root != null)
                    m_Root.HideMsgBox();
            }
            if (m_DrawAction != null)
                m_DrawAction(new Rect(main.x, main.y + 18, main.width, main.height - 18), obj);
        }

        private void DrawMask(Rect rect)
        {
            Color col = GUI.color;
            GUI.color = Color.black;
            GUI.Box(rect, "", GUIStyleCache.GetStyle("SelectionRect"));
            GUI.color = col;
        }
    }
}
