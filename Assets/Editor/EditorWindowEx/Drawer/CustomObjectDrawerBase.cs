using UnityEngine;
using System.Collections;
using EditorWinEx;
using EditorWinEx.Internal;
using System;

public abstract class CustomObjectDrawerBase : IMessageDispatcher
{

    public System.Object container;

    public abstract void Init();

    public abstract void OnEnable();

    public abstract void OnDisable();

    public abstract void OnDestroy();

    public Type GetDispatcherType()
    {
        if (container == null)
            return null;
        return container.GetType();
    }
}
