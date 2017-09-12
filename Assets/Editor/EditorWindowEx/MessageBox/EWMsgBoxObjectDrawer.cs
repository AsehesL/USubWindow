using UnityEngine;
using System.Collections;

namespace EditorWinEx.Internal
{
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