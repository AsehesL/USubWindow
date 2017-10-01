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

        protected abstract EWSubWindowToolbarType toolBar { get; }

        /// <summary>
        /// 帮助栏
        /// </summary>
        protected abstract SubWindowHelpBox helpBox { get; }

        public virtual void DrawLeafToolBar(Rect rect) { }

        public abstract string GetID(bool dynamic);

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

        public void Serialize(bool dynamic)
        {
            if (IsInitialized)
                OnSerialize(dynamic);
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

        protected virtual void OnSerialize(bool dynamic) { }

        public abstract void DrawWindow(Rect mainRect, Rect toolbarRect, Rect helpboxRect);

    }
}