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
            if (m_Drawer == null)
                return;
            string id = GetID();
            if (EditorPrefsEx.HasKey(id))
            {
                var obj = EditorPrefsEx.GetObject(id, drawer.GetType());
                if (obj != null)
                {
                    drawer = (EWMsgBoxCustomDrawer)obj;
                    drawer.SetContainer(this.m_Drawer.Container);
                    drawer.closeAction = this.m_Drawer.closeAction;
                    this.m_Drawer = drawer;
                }
            }
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
            string id = GetID();
            if (EditorPrefsEx.HasKey(id))
                EditorPrefsEx.DeleteKey(id);
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

        protected override void OnSerialize()
        {
            base.OnSerialize();
            if (m_Drawer == null)
                return;
            string id = GetID();
            EditorPrefsEx.SetObject(id, m_Drawer);
        }

        private string GetID()
        {
            return GetType().FullName + "." + m_Drawer.GetType().FullName + "." + m_Drawer.Container.GetType().FullName;
        }
    }
}