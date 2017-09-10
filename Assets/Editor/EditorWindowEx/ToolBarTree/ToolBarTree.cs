using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 工具栏树
/// </summary>
public class ToolBarTree : EditorWindowTool
{
    /// <summary>
    /// 选项数量
    /// </summary>
    public int count
    {
        get
        {
            if (m_Root == null)
                return 0;
            return m_Root.count;
        }
    }

    /// <summary>
    /// 根节点
    /// </summary>
    private ToolBarTreeNode m_Root;

    public ToolBarTree()
    {

    }

    /// <summary>
    /// 插入菜单项
    /// </summary>
    /// <param name="text">菜单项</param>
    /// <param name="method">菜单响应方法</param>
    /// <param name="target">响应对象</param>
    /// <param name="priority">优先级</param>
    public void InsertItem(string text, MethodInfo method, System.Object target, int priority)
    {
        if (method == null)
            return;
        if (method.GetParameters().Length != 0)
            return;
        if (string.IsNullOrEmpty(text))
            return;
        if (m_Root == null)
            m_Root = new ToolBarTreeNode("", 0);
        m_Root.InsertNode(text, method, target, null, null, priority);
    }

    public void InsertItem(string text, Delegate method, int priority, ConditionDelegate condition, System.Object obj)
    {
        if (method == null)
            return;
        int parameterslen = method.Method.GetParameters().Length;
        if (parameterslen > 1)
            return;
        if (parameterslen == 1 && obj == null)
            return;
        if (parameterslen == 0 && obj != null)
            return;
        if (string.IsNullOrEmpty(text))
            return;
        if (m_Root == null)
            m_Root = new ToolBarTreeNode("", 0);
        m_Root.InsertNode(text, method.Method, method.Target, condition, obj, priority);
    }

    /// <summary>
    /// 根据优先级排序
    /// </summary>
    public void Sort()
    {
        if (m_Root != null)
            m_Root.Sort();
    }

    /// <summary>
    /// 绘制工具栏
    /// </summary>
    public void DrawToolbar()
    {
        if (m_Root != null)
            m_Root.DrawToolBar();
    }

    protected override void OnRegisterMethod(System.Object container, MethodInfo method, System.Object target, bool isStatic)
    {
        System.Object[] atts = method.GetCustomAttributes(typeof(ToolBarAttribute), false);
        ParameterInfo[] parameters = method.GetParameters();
        if (atts != null && parameters.Length == 0)
        {
            for (int j = 0; j < atts.Length; j++)
            {
                ToolBarAttribute att = (ToolBarAttribute)atts[j];
                InsertItem(att.menuItem, method, target, att.priority);
            }
        }
    }

    protected override void OnRegisterClass(System.Object container, Type type)
    {
    }

    protected override void OnInit()
    {
        Sort();
    }
}
