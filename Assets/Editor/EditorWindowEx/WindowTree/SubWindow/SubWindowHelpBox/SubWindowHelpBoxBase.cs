using UnityEngine;
using UnityEditor;
using System.Collections;
using EditorWinEx;

/// <summary>
/// 帮助栏样式
/// </summary>
public enum SubWindowHelpBoxType
{
    None,
    Bottom,
    Left,
    Right,
    Top,
    Locker,
}

/// <summary>
/// 帮助栏基类
/// </summary>
public abstract class SubWindowHelpBox
{
    /// <summary>
    /// 帮助栏绘制方法
    /// </summary>
    /// <param name="rect"></param>
    /// <returns></returns>
    public abstract Rect DrawHelpBox(ref Rect rect);
    
    public static SubWindowHelpBox CreateHelpBox(SubWindowHelpBoxType helpBoxType)
    {
        switch (helpBoxType)
        {
            case SubWindowHelpBoxType.Bottom:
                return new SubWindowDockHelpBox(SubWindowDockHelpBox.DockPosition.Bottom);
            case SubWindowHelpBoxType.Left:
                return new SubWindowDockHelpBox(SubWindowDockHelpBox.DockPosition.Left);
            case SubWindowHelpBoxType.Right:
                return new SubWindowDockHelpBox(SubWindowDockHelpBox.DockPosition.Right);
            case SubWindowHelpBoxType.Top:
                return new SubWindowDockHelpBox(SubWindowDockHelpBox.DockPosition.Top);
            case SubWindowHelpBoxType.Locker:
                return new SubWindowLockerHelpBox();
            default:
                return null;
        }
    }
}