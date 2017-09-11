using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// 子窗口样式
/// </summary>
public enum SubWindowStyle
{
    Default,//默认
    Preview,//预览窗口样式
    Grid,//网格窗口样式
}

namespace EditorWinEx
{
    /// <summary>
    /// 子窗口样式Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SubWindowStyleAttribute : Attribute
    {
        public SubWindowStyle subWindowStyle;

        public SubWindowStyleAttribute(SubWindowStyle type)
        {
            this.subWindowStyle = type;
        }
    }
}