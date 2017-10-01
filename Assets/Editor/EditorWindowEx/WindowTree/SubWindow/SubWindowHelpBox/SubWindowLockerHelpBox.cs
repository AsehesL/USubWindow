using UnityEngine;
using System.Collections;
using UnityEditor;

namespace EditorWinEx
{
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
}