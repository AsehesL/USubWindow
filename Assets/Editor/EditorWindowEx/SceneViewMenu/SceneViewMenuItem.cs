using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections;

public delegate void DrawAction(Rect rect);

public delegate void DrawActionUseObj(Rect rect, System.Object obj);
public class SceneViewMenuItem
{

    public string Menu
    {
        get { return m_Menu; }
    }

    private string m_Menu;

    private DrawAction m_DrawAction;

    private float m_Weight;

    private bool m_IsLock;

    public SceneViewMenuItem(string menu, MethodInfo method, System.Object target)
    {
        this.m_Menu = menu;
        this.m_Weight = 0.3f;
        if (method != null)
            this.m_DrawAction = Delegate.CreateDelegate(typeof (DrawAction), target, method) as DrawAction;
    }

    public void Draw(Rect rect)
    {
        if (m_IsLock)
        {
            Rect lockerRect = new Rect(rect.x, rect.y + rect.height - 34, rect.width, 18);
            DrawTitleBar(lockerRect);
            EditorGUIUtility.AddCursorRect(lockerRect, MouseCursor.ResizeVertical);
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                if (lockerRect.Contains(Event.current.mousePosition))
                {
                    Event.current.Use();
                    m_IsLock = false;
                }
            }
        }
        else
        {
            Rect drawRect = new Rect(rect.x, rect.y + rect.height * (1 - m_Weight), rect.width, rect.height * m_Weight);
            Rect lockerRect = new Rect(rect.x, rect.y + rect.height * (1 - m_Weight) - 9, rect.width, 18);
            GUI.Box(drawRect, string.Empty, GUIStyleCache.GetStyle("WindowBackground"));
            DrawTitleBar(lockerRect);

            GUI.BeginGroup(new Rect(drawRect.x + 5, drawRect.y + 5, drawRect.width - 10, drawRect.height - 10));
            if (m_DrawAction != null)
                m_DrawAction(new Rect(0, 0, drawRect.width - 10, drawRect.height - 10));
            GUI.EndGroup();

            EditorGUIUtility.AddCursorRect(lockerRect, MouseCursor.ResizeVertical);
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                if (lockerRect.Contains(Event.current.mousePosition))
                {
                    m_IsLock = true;
                    Event.current.Use();
                }
                else if (drawRect.Contains(Event.current.mousePosition))
                {
                    Event.current.Use();
                }
            }
        }
    }

    private void DrawTitleBar(Rect rect)
    {
        GUI.Box(rect, string.Empty, GUIStyleCache.GetStyle("Toolbar"));
        GUI.Box(new Rect(rect.x + 90, rect.y + 6, rect.width - 100, rect.height - 6), string.Empty,
            GUIStyleCache.GetStyle("WindowBottomResize"));
        GUI.Label(new Rect(rect.x + 5, rect.y, 80, rect.height), m_Menu,
            GUIStyleCache.GetStyle("toolbarbutton"));
    }
}
