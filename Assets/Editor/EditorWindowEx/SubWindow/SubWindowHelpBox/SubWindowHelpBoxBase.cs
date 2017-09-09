using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// 帮助栏样式
/// </summary>
public enum SubWindowHelpBoxType
{
    None,
    Bottom,
    Left,
    Right,
    Top,
    Locker,
}

/// <summary>
/// 帮助栏基类
/// </summary>
public abstract class SubWindowHelpBox
{
    /// <summary>
    /// 帮助栏绘制方法
    /// </summary>
    /// <param name="rect"></param>
    /// <returns></returns>
    public abstract Rect DrawHelpBox(ref Rect rect);
    
    public static SubWindowHelpBox CreateHelpBox(SubWindowHelpBoxType helpBoxType)
    {
        switch (helpBoxType)
        {
            case SubWindowHelpBoxType.Bottom:
                return new SubWindowDockHelpBox(SubWindowDockHelpBox.DockPosition.Bottom);
            case SubWindowHelpBoxType.Left:
                return new SubWindowDockHelpBox(SubWindowDockHelpBox.DockPosition.Left);
            case SubWindowHelpBoxType.Right:
                return new SubWindowDockHelpBox(SubWindowDockHelpBox.DockPosition.Right);
            case SubWindowHelpBoxType.Top:
                return new SubWindowDockHelpBox(SubWindowDockHelpBox.DockPosition.Top);
            case SubWindowHelpBoxType.Locker:
                return new SubWindowLockerHelpBox();
            default:
                return null;
        }
    }
}

public class SubWindowDockHelpBox : SubWindowHelpBox
{
    public enum DockPosition
    {
        Bottom,
        Left,
        Right,
        Top,
    }

    protected DockPosition dockPosition;

    protected float weight = 0.3f;

    private bool m_IsDragging;

    public SubWindowDockHelpBox(DockPosition dockPosition) : base()
    {
        this.dockPosition = dockPosition;
    }

    public override Rect DrawHelpBox(ref Rect rect)
    {
        DoDrag(rect);
        Rect drawRect = default(Rect);
        if (dockPosition == DockPosition.Bottom)
        {
            drawRect = new Rect(rect.x, rect.y + rect.height * (1 - weight), rect.width, rect.height * weight);
            rect = new Rect(rect.x, rect.y, rect.width, rect.height * (1 - weight));
        }
        else if (dockPosition == DockPosition.Top)
        {
            drawRect = new Rect(rect.x, rect.y, rect.width, rect.height * weight);
            rect = new Rect(rect.x, rect.y + rect.height * weight, rect.width, rect.height * (1 - weight));
        }
        else if (dockPosition == DockPosition.Left)
        {
            drawRect = new Rect(rect.x, rect.y, rect.width * weight, rect.height);
            rect = new Rect(rect.x + rect.width * weight, rect.y, rect.width * (1 - weight), rect.height);
        }
        else if (dockPosition == DockPosition.Right)
        {
            drawRect = new Rect(rect.x + rect.width * (1 - weight), rect.y, rect.width * weight, rect.height);
            rect = new Rect(rect.x, rect.y, rect.width * (1 - weight), rect.height);
        }
        GUI.Box(drawRect, string.Empty, GUIStyleCache.GetStyle("WindowBackground"));
        return drawRect;
    }

    protected void DoDrag(Rect rect)
    {
        Rect dragRect = default(Rect);
        MouseCursor cursor = default(MouseCursor);
        if (dockPosition == DockPosition.Bottom)
        {
            dragRect = new Rect(rect.x, rect.y + rect.height * (1 - weight) - 2, rect.width, 4);
            cursor = MouseCursor.ResizeVertical;
        }
        else if (dockPosition == DockPosition.Top)
        {
            dragRect = new Rect(rect.x, rect.y + rect.height * weight - 2, rect.width, 4);
            cursor = MouseCursor.ResizeVertical;
        }
        else if (dockPosition == DockPosition.Left)
        {
            dragRect = new Rect(rect.x + rect.width * weight - 2, rect.y, 4, rect.height);
            cursor = MouseCursor.ResizeHorizontal;
        }
        else if (dockPosition == DockPosition.Right)
        {
            dragRect = new Rect(rect.x + rect.width * (1 - weight) - 2, rect.y, 4, rect.height);
            cursor = MouseCursor.ResizeHorizontal;
        }
        EditorGUIUtility.AddCursorRect(dragRect, cursor);
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            if (dragRect.Contains(Event.current.mousePosition))
            {
                m_IsDragging = true;
                Event.current.Use();
            }
        }
        if (m_IsDragging)
        {
            if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
            {
                m_IsDragging = false;
                Event.current.Use();
            }
            if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
            {
                if (dockPosition == DockPosition.Bottom)
                {
                    weight -= Event.current.delta.y / rect.height;
                }
                else if (dockPosition == DockPosition.Top)
                {
                    weight += Event.current.delta.y / rect.height;
                }
                else if (dockPosition == DockPosition.Left)
                {
                    weight += Event.current.delta.x / rect.width;
                }
                else if (dockPosition == DockPosition.Right)
                {
                    weight -= Event.current.delta.x / rect.width;
                }
                weight = Mathf.Clamp(weight, 0.05f, 0.95f);
                Event.current.Use();
            }
        }
    }
}

public class SubWindowLockerHelpBox : SubWindowDockHelpBox
{

    private bool m_IsLock = true;

    public SubWindowLockerHelpBox() : base(DockPosition.Bottom) { }

    public override Rect DrawHelpBox(ref Rect rect)
    {
        if (m_IsLock)
        {

            Rect lockerRect = new Rect(rect.x, rect.y + rect.height - 18, rect.width, 18);
            rect = new Rect(rect.x, rect.y, rect.width, rect.height - 18);
            GUI.Box(lockerRect, string.Empty, GUIStyleCache.GetStyle("Toolbar"));
            GUI.Box(new Rect(lockerRect.x + 20, lockerRect.y + 6, lockerRect.width - 40, lockerRect.height - 6), string.Empty,
                GUIStyleCache.GetStyle("WindowBottomResize"));
            EditorGUIUtility.AddCursorRect(lockerRect, MouseCursor.ResizeVertical);
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                if (lockerRect.Contains(Event.current.mousePosition))
                {
                    Event.current.Use();
                    m_IsLock = false;
                }
            }
            return new Rect(rect.x, rect.y, 0, 0);
        }
        else
        {
            DoDrag(rect);
            Rect drawRect = new Rect(rect.x, rect.y + rect.height * (1 - weight), rect.width, rect.height * weight);
            Rect lockerRect = new Rect(rect.x, rect.y + rect.height * (1 - weight) - 9, rect.width, 18);
            rect = new Rect(rect.x, rect.y, rect.width, rect.height * (1 - weight) - 9);

            GUI.Box(drawRect, string.Empty, GUIStyleCache.GetStyle("WindowBackground"));
            GUI.Box(lockerRect, string.Empty, GUIStyleCache.GetStyle("Toolbar"));
            GUI.Box(new Rect(lockerRect.x + 20, lockerRect.y + 6, lockerRect.width - 40, lockerRect.height - 6), string.Empty,
                GUIStyleCache.GetStyle("WindowBottomResize"));
            if (weight <= 0.08f)
            {
                weight = 0.1f;
                m_IsLock = true;
            }
            return drawRect;
        }
    }
}