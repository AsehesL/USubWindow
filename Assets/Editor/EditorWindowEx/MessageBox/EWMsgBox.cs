using System;
using UnityEngine;
using System.Collections;
using System.Reflection;

namespace EditorWinEx.Internal
{
    internal class EWMsgBox
    {
        private EWMsgBoxDrawer m_Drawer;

        public EWMsgBox(MethodInfo method, System.Object target, float x, float y, float width, float height)
        {
            m_Drawer = new EWMsgBoxMethodDrawer(method, target, x, y, width, height);
        }

        public EWMsgBox(EWMsgBoxCustomObjectDrawer drawer)
        {
            m_Drawer = new EWMsgBoxObjectDrawer(drawer);
        }

        public void DrawMsgBox(Rect rect, System.Object obj)
        {
            m_Drawer.DrawMsgBox(rect, obj);
        }
    }
}