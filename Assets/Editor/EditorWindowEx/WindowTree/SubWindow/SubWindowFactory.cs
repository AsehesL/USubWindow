using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using EditorWinEx;

/// <summary>
/// SubWindow工厂类
/// </summary>
internal class SubWindowFactory
{
    private static Dictionary<SubWindowStyle, System.Type> subWindowClass;

    public static SubWindow CreateSubWindow(SubWindowStyle style, string title, string iconPath, bool defaultOpen,
        MethodInfo method, System.Object target, SubWindowToolbarType toolbar, SubWindowHelpBoxType helpbox)
    {
        if (!CheckSubWindowParameters(method, toolbar, helpbox))
            return null;
        if (subWindowClass == null)
            GetSubWinodwStyleClasses();
        else if (!subWindowClass.ContainsKey(style))
            GetSubWinodwStyleClasses();
        if (subWindowClass == null || !subWindowClass.ContainsKey(style))
            return null;
        System.Type type = subWindowClass[style];
        return (SubWindow)System.Activator.CreateInstance(type, title, iconPath, defaultOpen, method, target, toolbar,
            helpbox);
    }

    public static SubWindow CreateSubWindow(System.Object container, bool defaultOpen, SubWindowStyle style, System.Type customDrawerType)
    {
        if (customDrawerType == null)
            return null;
        if (subWindowClass == null)
            GetSubWinodwStyleClasses();
        else if (!subWindowClass.ContainsKey(style))
            GetSubWinodwStyleClasses();
        if (subWindowClass == null || !subWindowClass.ContainsKey(style))
            return null;
        System.Type type = subWindowClass[style];
        SubWindowCustomObjectDrawer drawer =
            (SubWindowCustomObjectDrawer) System.Activator.CreateInstance(customDrawerType);
        if (drawer == null)
            return null;
        drawer.container = container;
        return (SubWindow)System.Activator.CreateInstance(type, defaultOpen, drawer);
    }

    /// <summary>
    /// 检查子窗口绘制函数的参数是否合法
    /// </summary>
    /// <param name="infos">绘制方法的参数数组</param>
    /// <returns></returns>
    private static bool CheckSubWindowParameters(MethodInfo method, SubWindowToolbarType toolbar, SubWindowHelpBoxType helpbox)
    {
        ParameterInfo[] infos = method.GetParameters();
        if (toolbar != SubWindowToolbarType.None && helpbox != SubWindowHelpBoxType.None)
        {
            if (infos.Length != 3)
                return false;
        }
        else if ((toolbar != SubWindowToolbarType.None && helpbox == SubWindowHelpBoxType.None) || (toolbar == SubWindowToolbarType.None && helpbox != SubWindowHelpBoxType.None))
        {
            if (infos.Length != 2)
                return false;
        }
        else if (toolbar == SubWindowToolbarType.None && helpbox == SubWindowHelpBoxType.None)
        {
            if (infos.Length != 1)
                return false;
        }
        for (int i = 0; i < infos.Length; i++)
        {
            if (infos[i].ParameterType != typeof(Rect))
                return false;
        }
        return true;
    }

    private static void GetSubWinodwStyleClasses()
    {
        if (subWindowClass == null)
            subWindowClass = new Dictionary<SubWindowStyle, Type>();
        subWindowClass.Clear();
        System.Type sbwindow = typeof(SubWindow);
        Assembly assembly = sbwindow.Assembly;
        System.Type[] tp = assembly.GetTypes();
        for (int i = 0; i < tp.Length; i++)
        {
            if (!tp[i].IsClass)
                continue;
            if (tp[i].IsAbstract)
                continue;
            if (tp[i].IsSubclassOf(sbwindow) || tp[i] == sbwindow)
            {
                System.Object[] atts = tp[i].GetCustomAttributes(typeof(SubWindowStyleAttribute), false);
                for (int j = 0; j < atts.Length; j++)
                {
                    SubWindowStyleAttribute att = (SubWindowStyleAttribute)atts[j];
                    subWindowClass[att.subWindowStyle] = tp[i];
                }
            }
        }
    }
}
