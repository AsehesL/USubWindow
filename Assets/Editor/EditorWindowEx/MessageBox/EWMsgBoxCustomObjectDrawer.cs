using UnityEngine;
using System.Collections;
using System;

public abstract class EWMsgBoxCustomObjectDrawer : CustomObjectDrawerBase
{

    public abstract float X { get; }

    public abstract float Y { get; }

    public abstract float Width { get; }

    public abstract float Height { get; }

    public System.Action closeAction;

    public void CloseMsgBox()
    {
        if (closeAction != null)
            closeAction();
    }

    public override void OnDestroy()
    {
    }

    public override void OnDisable()
    {
    }

    public override void OnEnable()
    {
    }

    public override void Init()
    {
    }

    public virtual void DrawMsgBox(Rect rect, System.Object obj)
    {
        
    }
}
