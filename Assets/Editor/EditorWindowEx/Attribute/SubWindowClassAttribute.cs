using UnityEngine;
using System;
using System.Collections;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class SubWindowClassAttribute : Attribute
{
    /// <summary>
    /// 目标窗口类型
    /// </summary>
    public Type targetWinType;

    /// <summary>
    /// 是否激活
    /// </summary>
    public bool active;

    /// <summary>
    /// 窗口样式类型
    /// </summary>
    public SubWindowStyle windowStyle;

    public SubWindowClassAttribute(Type targetWinType, SubWindowStyle windowStyle = SubWindowStyle.Default, bool active = true)
    {
        this.targetWinType = targetWinType;
        this.active = active;
        this.windowStyle = windowStyle;
    }
}
