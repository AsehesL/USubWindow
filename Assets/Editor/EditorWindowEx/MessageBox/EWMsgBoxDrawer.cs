using System;
using UnityEngine;
using System.Collections;
using System.Reflection;

namespace EditorWinEx.Internal
{
    /// <summary>
    /// MsgBox组件绘制器
    /// </summary>
    internal abstract class EWMsgBoxDrawer : EWComponentDrawerBase
    {

        protected abstract EWRectangle Rectangle { get; }

        public void DrawMsgBox(Rect rect, System.Object obj)
        {
            Rect main = Rectangle.GetRect(rect);

            GUI.Box(main, "", GUIStyleCache.GetStyle("WindowBackground"));
            OnDrawMsgBox(main, obj);
        }

        protected override void OnDestroy()
        {
        }

        protected override void OnEnable()
        {
        }

        protected override void OnDisable()
        {
        }

        protected abstract void OnDrawMsgBox(Rect rect, System.Object obj);
    }

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
                m_DrawAction = Delegate.CreateDelegate(typeof (DrawActionUseObj), target, method) as DrawActionUseObj;

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

    internal class EWMsgBoxObjectDrawer : EWMsgBoxDrawer
    {
        protected override EWRectangle Rectangle
        {
            get { return m_Drawer.Recttangle; }
        }

        private EWMsgBoxCustomDrawer m_Drawer;

        public EWMsgBoxObjectDrawer(EWMsgBoxCustomDrawer drawer)
        {
            m_Drawer = drawer;
        }

        protected override bool OnInit()
        {
            m_Drawer.Init();
            return true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            m_Drawer.OnDestroy();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_Drawer.OnDisable();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Drawer.OnEnable();
        }

        protected override void OnDrawMsgBox(Rect rect, object obj)
        {
            m_Drawer.DrawMsgBox(rect, obj);
        }
    }
}