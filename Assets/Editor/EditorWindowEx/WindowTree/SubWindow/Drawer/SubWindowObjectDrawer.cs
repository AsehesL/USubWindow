using System;
using UnityEngine;
using System.Collections;
using UnityEditor;

namespace EditorWinEx.Internal
{
    internal class SubWindowObjectDrawer : SubWindowDrawerBase
    {
        protected override SubWindowHelpBox helpBox
        {
            get { return m_ObjDrawer.helpBox; }
        }

        public override GUIContent Title
        {
            get { return m_ObjDrawer.Title; }
        }

        protected override EWSubWindowToolbarType toolBar
        {
            get { return m_ObjDrawer.toolBar; }
        }

        private SubWindowCustomDrawer m_ObjDrawer;

        private bool m_IsLock = false;

        public SubWindowObjectDrawer(SubWindowCustomDrawer drawer)
        {
            this.m_ObjDrawer = drawer;
            if (m_ObjDrawer == null)
                return;
            string id = GetID(false);
            if (EditorPrefsEx.HasKey(id))
            {
                var obj = EditorPrefsEx.GetObject(id, drawer.GetType());
                if (obj != null)
                {
                    drawer = (SubWindowCustomDrawer)obj;
                    drawer.SetContainer(this.m_ObjDrawer.Container);
                    this.m_ObjDrawer = drawer;
                }
            }
        }

        internal static string GetDrawerID(SubWindowCustomDrawer drawer, bool dynamic)
        {
            string result = null;
            if (drawer == null)
                return result;
            result = "__CLASS__" + drawer.GetType().FullName;
            if (dynamic)
                if (drawer.Title != null && !string.IsNullOrEmpty(drawer.Title.text))
                    result += "." + drawer.GetHashCode();
                else
                    result += ".UnKnown";
            return result;
        }

        internal static string GetDrawerIDByType(Type type, string title, bool dynamic)
        {
            string result = null;
            if (type == null)
                return null;
            if(type.IsSubclassOf(typeof(SubWindowCustomDrawer)))
                result = "__CLASS__" + type.FullName;
            if(dynamic)
                if (!string.IsNullOrEmpty(title))
                    result += "." + title;
                else
                    result += ".UnKnownTitle";
            return result;
        }

        public override string GetID(bool dynamic)
        {
            return GetDrawerID(m_ObjDrawer, dynamic);
        }

        public override void DrawWindow(Rect mainRect, Rect toolbarRect, Rect helpboxRect)
        {
            this.m_ObjDrawer.DrawMainWindow(mainRect);
            if (toolbarRect.width > 0 && toolbarRect.height > 0)
                this.m_ObjDrawer.DrawToolBar(toolbarRect);
            if (helpboxRect.width > 0 && helpboxRect.height > 0)
                this.m_ObjDrawer.DrawHelpBox(helpboxRect);
        }

        public override void DrawLeafToolBar(Rect rect)
        {
            base.DrawLeafToolBar(rect);
            if (m_ObjDrawer is ISubWinCustomMenu)
            {
                Rect popRect = new Rect(rect.x + rect.width - 12, rect.y + 7, 13, 11);
                if (GUI.Button(popRect, string.Empty,
                    GUIStyleCache.GetStyle("PaneOptions")))
                {
                    GenericMenu menu = new GenericMenu();
                    ((ISubWinCustomMenu)m_ObjDrawer).AddCustomMenu(menu);
                    if (menu.GetItemCount() > 0)
                        menu.DropDown(popRect);
                }
                rect = new Rect(rect.x + rect.width - 40, rect.y, rect.width - 40, rect.height);
            }
            if (m_ObjDrawer is ISubWinLock)
            {
                EditorGUI.BeginChangeCheck();
                m_IsLock = GUI.Toggle(new Rect(rect.x + rect.width - 20, rect.y + 3, 13, 11), m_IsLock, string.Empty,
                    GUIStyleCache.GetStyle("IN LockButton"));
                if (EditorGUI.EndChangeCheck())
                {
                    ((ISubWinLock)m_ObjDrawer).SetLockActive(m_IsLock);
                }
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_ObjDrawer.OnDisable();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            m_ObjDrawer.OnDestroy();
            string id = GetID(false);
            if (EditorPrefsEx.HasKey(id))
                EditorPrefsEx.DeleteKey(id);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            m_ObjDrawer.OnEnable();
        }

        protected override bool OnInit()
        {
            m_ObjDrawer.Init();
            return true;
        }

        protected override void OnSerialize(bool dynamic)
        {
            base.OnSerialize(dynamic);
            if (!dynamic)
            {
                string id = GetID(false);
                EditorPrefsEx.SetObject(id, m_ObjDrawer);
            }
        }
    }
}