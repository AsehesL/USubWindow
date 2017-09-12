using System;
using UnityEngine;
using System.Collections;

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
