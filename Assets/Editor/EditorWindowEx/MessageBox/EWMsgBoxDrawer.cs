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
}