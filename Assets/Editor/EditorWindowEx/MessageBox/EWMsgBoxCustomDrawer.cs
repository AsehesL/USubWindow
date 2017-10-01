using UnityEngine;
using System.Collections;
using System;
using EditorWinEx.Internal;
using UnityEngine.Events;

/// <summary>
/// 自定义MsgBox绘制器
/// </summary>
[Serializable]
public abstract class EWMsgBoxCustomDrawer : CustomEWComponentDrawerBase
{

    public abstract EWRectangle Recttangle { get; }

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
