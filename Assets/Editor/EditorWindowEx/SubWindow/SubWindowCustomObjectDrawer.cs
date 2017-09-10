using UnityEngine;
using System.Collections;

public abstract class SubWindowCustomObjectDrawer
{
    public System.Object container;

    public abstract SubWindowHelpBox helpBox { get; }

    public abstract GUIContent Title { get; }

    public abstract SubWindowToolbarType toolBar { get; }

    public virtual void DrawMainWindow(Rect mainRect) { }

    public virtual void DrawToolBar(Rect toolbar) { }

    public virtual void DrawHelpBox(Rect helpBox) { }
}
