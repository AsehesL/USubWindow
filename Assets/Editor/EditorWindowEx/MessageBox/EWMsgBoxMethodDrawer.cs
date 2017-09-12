using System;
using UnityEngine;
using System.Collections;
using System.Reflection;

namespace EditorWinEx.Internal
{
    internal class EWMsgBoxMethodDrawer : EWMsgBoxDrawer
    {
        protected override EWRectangle Rectangle
        {
            get { return m_Rectangle; }
        }

        private DrawActionUseObj m_DrawAction;

        private EWRectangle m_Rectangle;

        public EWMsgBoxMethodDrawer(MethodInfo method, System.Object target, EWRectangle rectangle)
        {
            if (method != null && target != null)
                m_DrawAction = Delegate.CreateDelegate(typeof(DrawActionUseObj), target, method) as DrawActionUseObj;

            this.m_Rectangle = rectangle;
        }

        protected override bool OnInit()
        {
            return true;
        }

        protected override void OnDrawMsgBox(Rect rect, object obj)
        {
            if (m_DrawAction != null)
                m_DrawAction(rect, obj);
        }
    }
}