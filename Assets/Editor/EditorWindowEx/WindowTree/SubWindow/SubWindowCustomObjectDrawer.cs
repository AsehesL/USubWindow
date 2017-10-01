using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// SubWindow自定义绘制器
/// </summary>
[System.Serializable]
public abstract class SubWindowCustomDrawer : CustomEWComponentDrawerBase
{

    public SubWindowHelpBox helpBox
    {
        get { return m_HelpBox; }
    }

    [NonSerialized]
    private SubWindowHelpBox m_HelpBox;

    [NonSerialized]
    private SubWindowHelpBoxType m_HelpBoxType;

    public void SetSubWindowHelpBoxType(SubWindowHelpBoxType helpBoxType)
    {
        if (m_HelpBoxType == helpBoxType)
            return;
        m_HelpBoxType = helpBoxType;
        m_HelpBox = SubWindowHelpBox.CreateHelpBox(helpBoxType);
    }

    public override void Init()
    {
    }

    public override void OnEnable()
    {
    }

    public override void OnDisable()
    {
    }

    public override void OnDestroy()
    {
    }

    public abstract GUIContent Title { get; }

    public abstract EWSubWindowToolbarType toolBar { get; }

    public virtual void DrawMainWindow(Rect mainRect) { }

    public virtual void DrawToolBar(Rect toolbar) { }

    public virtual void DrawHelpBox(Rect helpBox) { }
}
