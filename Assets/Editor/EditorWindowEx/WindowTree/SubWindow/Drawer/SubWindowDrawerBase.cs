using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using UnityEditor;

namespace EditorWinEx.Internal
{
    /// <summary>
    /// SubWindow组件绘制器
    /// </summary>
    internal abstract class SubWindowDrawerBase : EWComponentDrawerBase
    {
        /// <summary>
        /// 标题
        /// </summary>
        public abstract GUIContent Title { get; }

        public abstract string Id { get; }

        protected abstract EWSubWindowToolbarType toolBar { get; }

        /// <summary>
        /// 帮助栏
        /// </summary>
        protected abstract SubWindowHelpBox helpBox { get; }

        public Rect DrawToolBar(ref Rect rect)
        {
            if (toolBar == EWSubWindowToolbarType.Normal)
            {
                Rect h = new Rect(rect.x, rect.y, rect.width, 18);
                rect = new Rect(rect.x, rect.y + 18, rect.width, rect.height - 18);
                GUI.Box(h, string.Empty, GUIStyleCache.GetStyle("Toolbar"));
                return h;
            }
            else if (toolBar == EWSubWindowToolbarType.Mini)
            {
                Rect h = new Rect(rect.x, rect.y, rect.width, 15);
                rect = new Rect(rect.x, rect.y + 15, rect.width, rect.height - 15);
                GUI.Box(h, string.Empty, GUIStyleCache.GetStyle("MiniToolbarButton"));
                return h;
            }
            return new Rect(rect.x, rect.y, 0, 0);
        }

        public Rect DrawHelpBox(ref Rect rect)
        {
            if (helpBox != null)
            {
                Rect h = helpBox.DrawHelpBox(ref rect);
                return h;
            }
            return new Rect(rect.x, rect.y, 0, 0);
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

        public abstract void DrawWindow(Rect mainRect, Rect toolbarRect, Rect helpboxRect);

    }

    internal class SubWindowMethodDrawer : SubWindowDrawerBase
    {
        protected override SubWindowHelpBox helpBox
        {
            get { return m_HelpBox; }
        }

        public override string Id
        {
            get { return m_ID; }
        }

        public override GUIContent Title
        {
            get { return m_Title; }
        }

        protected override EWSubWindowToolbarType toolBar
        {
            get { return m_ToolBar; }
        }


        private GUIContent m_Title;

        /// <summary>
        /// 绘制函数参数列表
        /// </summary>
        private System.Object[] m_Params;

        /// <summary>
        /// 绘制函数
        /// </summary>
        private MethodInfo m_Method;

        /// <summary>
        /// 绘制函数响应目标
        /// </summary>
        private System.Object m_Target;

        private EWSubWindowToolbarType m_ToolBar;

        private string m_ID;

        /// <summary>
        /// 帮助栏
        /// </summary>
        private SubWindowHelpBox m_HelpBox = null;

        public SubWindowMethodDrawer(string title, string icon, MethodInfo method, System.Object target,
            EWSubWindowToolbarType toolbar, SubWindowHelpBoxType helpbox)
        {
            this.m_Title = CreateTitle(title, icon);
            this.m_Method = method;
            this.m_ToolBar = toolbar;
            this.m_HelpBox = SubWindowHelpBox.CreateHelpBox(helpbox);
            this.m_Target = target;
            if (target == null && method == null)
                this.m_ID = "UnKnownClass.UnKnownMethod";
            else if (target == null)
                this.m_ID = "UnKnownClass." + method.Name;
            else if (method == null)
                this.m_ID = target.GetType().FullName + ".UnKnownMethod";
            else
                this.m_ID = target.GetType().FullName + "." + method.Name;
            if (this.m_Method != null)
            {
                ParameterInfo[] p = this.m_Method.GetParameters();
                m_Params = new System.Object[p.Length];
            }
        }

        protected override bool OnInit()
        {
            return true;
        }

        public override void DrawWindow(Rect mainRect, Rect toolbarRect, Rect helpboxRect)
        {
            if (m_Method != null)
            {
                if (m_Params.Length > 0)
                    m_Params[0] = mainRect;
                if (m_Params.Length > 1)
                    if (m_ToolBar == EWSubWindowToolbarType.None)
                        m_Params[1] = helpboxRect;
                    else
                        m_Params[1] = toolbarRect;
                if (m_Params.Length > 2)
                    m_Params[2] = helpboxRect;
                m_Method.Invoke(m_Target, m_Params);
            }
        }

        private GUIContent CreateTitle(string title, string icon)
        {
            if (string.IsNullOrEmpty(icon))
                return new GUIContent(title);
            Texture2D tex = EditorGUIUtility.FindTexture(icon);
            if (tex == null)
                return new GUIContent(title);
            return new GUIContent(title, tex);
        }
    }

    internal class SubWindowObjectDrawer : SubWindowDrawerBase
    {
        protected override SubWindowHelpBox helpBox
        {
            get { return m_ObjDrawer.helpBox; }
        }

        public override string Id
        {
            get { return m_Id; }
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

        private string m_Id;

        public SubWindowObjectDrawer(SubWindowCustomDrawer drawer)
        {
            this.m_ObjDrawer = drawer;
            this.m_Id = drawer.GetType().FullName;
        }

        public override void DrawWindow(Rect mainRect, Rect toolbarRect, Rect helpboxRect)
        {
            this.m_ObjDrawer.DrawMainWindow(mainRect);
            if (toolbarRect.width > 0 && toolbarRect.height > 0)
                this.m_ObjDrawer.DrawToolBar(toolbarRect);
            if (helpboxRect.width > 0 && helpboxRect.height > 0)
                this.m_ObjDrawer.DrawHelpBox(helpboxRect);
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
    }
}