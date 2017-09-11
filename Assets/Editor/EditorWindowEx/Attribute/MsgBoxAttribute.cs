using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// 子窗体消息框
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class MsgBoxAttribute : Attribute
{
    /// <summary>
    /// 消息框ID
    /// </summary>
    public int id;

    public float x;
    public float y;
    public float width;
    public float height;


    public MsgBoxAttribute(int id, float x, float y, float width, float height)
    {
        this.id = id;
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class MsgBoxHandleAttribute : Attribute
{
    public Type targetType;
    /// <summary>
    /// 消息框ID
    /// </summary>
    public int id;

    public MsgBoxHandleAttribute(Type targetType, int id)
    {
        this.id = id;
        this.targetType = targetType;
    }
}
