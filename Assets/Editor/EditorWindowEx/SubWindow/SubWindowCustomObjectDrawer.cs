using UnityEngine;
using System.Collections;

public abstract class SubWindowCustomObjectDrawer
{
    public System.Object container;

    public SubWindowHelpBox helpBox
    {
        get { return m_HelpBox; }
    }

    public void SetSubWindowHelpBoxType(SubWindowHelpBoxType helpBoxType)
    {
        if (m_HelpBoxType == helpBoxType)
            return;
        m_HelpBoxType = helpBoxType;
        m_HelpBox = SubWindowHelpBox.CreateHelpBox(helpBoxType);
    }

    public abstract GUIContent Title { get; }

    public abstract SubWindowToolbarType toolBar { get; }

    public virtual void DrawMainWindow(Rect mainRect) { }

    public virtual void DrawToolBar(Rect toolbar) { }

    public virtual void DrawHelpBox(Rect helpBox) { }

    private SubWindowHelpBox m_HelpBox;

    private SubWindowHelpBoxType m_HelpBoxType;
}
