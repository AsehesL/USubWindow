using UnityEngine;
using System;
using System.Collections;
using EditorWinEx.Internal;

/// <summary>
/// 子窗体消息框-该特性允许自定义消息框绘制函数
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class EWMsgBoxAttribute : Attribute
{
    /// <summary>
    /// 消息框ID
    /// </summary>
    public int id;

    public EWRectangle Rectangle { get; private set; }

    /// <summary>
    /// 消息框窗口
    /// </summary>
    /// <param name="id">窗口ID</param>
    /// <param name="x">x(0~1)</param>
    /// <param name="y">y(0~1)</param>
    /// <param name="width">width(0~1)</param>
    /// <param name="height">height(0~1)</param>
    public EWMsgBoxAttribute(int id, float x = 0.2f, float y = 0.2f, float width = 0.6f, float height = 0.6f)
    {
        this.id = id;
        this.Rectangle = new EWRectangle(x, y, width, height);
    }

    /// <summary>
    /// 消息框窗口
    /// </summary>
    /// <param name="id">窗口ID</param>
    /// <param name="x">x</param>
    /// <param name="y">y</param>
    /// <param name="z">z</param>
    /// <param name="w">w</param>
    /// <param name="anchorLeft">左侧约束</param>
    /// <param name="anchorRight">右侧约束</param>
    /// <param name="anchorTop">顶部约束</param>
    /// <param name="anchorBottom">底部约束</param>
    public EWMsgBoxAttribute(int id, float x, float y, float z, float w, bool anchorLeft, bool anchorRight, bool anchorTop, bool anchorBottom)
    {
        this.id = id;
        this.Rectangle = new EWRectangle(x, y, z, w, anchorLeft, anchorRight, anchorTop, anchorBottom);
    }
}

/// <summary>
/// 子窗体消息框-该特性允许自定义消息框绘制对象
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class EWMsgBoxHandleAttribute : Attribute
{
    public Type targetType;
    /// <summary>
    /// 消息框ID
    /// </summary>
    public int id;

    /// <summary>
    /// 消息框
    /// </summary>
    /// <param name="targetType">消息框绘制目标的类型</param>
    /// <param name="id">消息框ID</param>
    public EWMsgBoxHandleAttribute(Type targetType, int id)
    {
        this.id = id;
        this.targetType = targetType;
    }
}
