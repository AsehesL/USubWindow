using System;
using UnityEditor;
using UnityEngine;
using System.Collections;


/// <summary>
/// 工具栏标签
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class ToolBarAttribute : Attribute
{
    /// <summary>
    /// 菜单项
    /// </summary>
    public string menuItem;

    /// <summary>
    /// 优先级
    /// </summary>
    public int priority;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="menuItem">菜单项</param>
    /// <param name="priority">优先级-每1000分为一组</param>
    public ToolBarAttribute(string menuItem, int priority = 1000)
    {
        this.menuItem = menuItem;
        this.priority = priority;
    }
}