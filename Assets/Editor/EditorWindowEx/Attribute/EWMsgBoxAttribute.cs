using UnityEngine;
using System;
using System.Collections;

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

    public float x;
    public float y;
    public float width;
    public float height;

    /// <summary>
    /// 消息框窗口
    /// </summary>
    /// <param name="id">窗口ID</param>
    /// <param name="x">x(0~1)</param>
    /// <param name="y">y(0~1)</param>
    /// <param name="width">width(0~1)</param>
    /// <param name="height">height(0~1)</param>
    public EWMsgBoxAttribute(int id, float x, float y, float width, float height)
    {
        this.id = id;
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
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
