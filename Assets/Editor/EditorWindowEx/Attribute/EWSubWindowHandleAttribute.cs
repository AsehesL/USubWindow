using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// 子窗口-该特性允许自定义子窗口绘制对象，只能用于SubWindowCustomDrawer的子类
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class EWSubWindowHandleAttribute : Attribute
{
    /// <summary>
    /// 子窗口容器类型
    /// </summary>
    public Type containerType;

    /// <summary>
    /// 是否激活
    /// </summary>
    public bool active;

    /// <summary>
    /// 窗口样式类型
    /// </summary>
    public SubWindowStyle windowStyle;

    /// <summary>
    /// 子窗口标签
    /// </summary>
    /// <param name="containerType">子窗口容器类型</param>
    /// <param name="windowStyle">窗口风格</param>
    /// <param name="active">是否激活</param>
    public EWSubWindowHandleAttribute(Type containerType, SubWindowStyle windowStyle = SubWindowStyle.Default, bool active = true)
    {
        this.containerType = containerType;
        this.active = active;
        this.windowStyle = windowStyle;
    }
}
