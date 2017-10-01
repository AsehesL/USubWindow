using UnityEngine;
using System.Collections;
using System.Reflection;
using UnityEditor;
using System;

namespace EditorWinEx.Internal
{
    internal class SubWindowMethodDrawer : SubWindowDrawerBase
    {
        protected override SubWindowHelpBox helpBox
        {
            get { return m_HelpBox; }
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

        /// <summary>
        /// 帮助栏
        /// </summary>
        private SubWindowHelpBox m_HelpBox = null;

        private string m_Id;

        internal static string GetMethodID(MethodInfo method, System.Object target)
        {
            string result = null;
            if (target == null && method == null)
                result= "__METHOD__UnKnownClass.UnKnownMethod";
            else if (target == null)
                result= "__METHOD__UnKnownClass." + method.Name;
            else if (method == null)
                result= "__METHOD__" + target.GetType().FullName + ".UnKnownMethod";
            else
                result= "__METHOD__" + target.GetType().FullName + "." + method.Name;
            return result;
        }

        public SubWindowMethodDrawer(string title, string icon, MethodInfo method, System.Object target,
            EWSubWindowToolbarType toolbar, SubWindowHelpBoxType helpbox)
        {
            this.m_Title = CreateTitle(title, icon);
            this.m_Method = method;
            this.m_ToolBar = toolbar;
            this.m_HelpBox = SubWindowHelpBox.CreateHelpBox(helpbox);
            this.m_Target = target;
            this.m_Id = GetMethodID(method, target);
            if (this.m_Method != null)
            {
                ParameterInfo[] p = this.m_Method.GetParameters();
                m_Params = new System.Object[p.Length];
            }
        }

        public override string GetID(bool dynamic)
        {
            return m_Id;
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
}