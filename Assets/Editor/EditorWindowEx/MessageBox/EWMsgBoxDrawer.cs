using System;
using UnityEngine;
using System.Collections;
using System.Reflection;

namespace EditorWinEx.Internal
{
    internal abstract class EWMsgBoxDrawer : CustomObjectDrawerWarpperBase
    {

        protected abstract float X { get; }
        protected abstract float Y { get; }
        protected abstract float Width { get; }
        protected abstract float Height { get; }

        public void DrawMsgBox(Rect rect, System.Object obj)
        {
            Rect main = new Rect(rect.x + rect.width * X, rect.y + rect.height * Y, rect.width * Width,
               rect.height * Height);

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
        protected override float Height
        {
            get { return m_Height; }
        }

        protected override float Width
        {
            get { return m_Width; }
        }

        protected override float X
        {
            get { return m_X; }
        }

        protected override float Y
        {
            get { return m_Y; }
        }

        private readonly float m_X;
        private readonly float m_Y;
        private readonly float m_Width;
        private readonly float m_Height;

        private DrawActionUseObj m_DrawAction;

        public EWMsgBoxMethodDrawer(MethodInfo method, System.Object target, float x, float y, float width, float height)
        {
            if (method != null && target != null)
                m_DrawAction = Delegate.CreateDelegate(typeof (DrawActionUseObj), target, method) as DrawActionUseObj;
            this.m_X = x;
            this.m_Y = y;
            this.m_Width = width;
            this.m_Height = height;
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
        protected override float X
        {
            get { return m_Drawer.X;  }
        }

        protected override float Y
        {
            get { return m_Drawer.Y; }
        }

        protected override float Width
        {
            get { return m_Drawer.Width; }
        }

        protected override float Height
        {
            get { return m_Drawer.Height; }
        }

        private EWMsgBoxCustomObjectDrawer m_Drawer;

        public EWMsgBoxObjectDrawer(EWMsgBoxCustomObjectDrawer drawer)
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