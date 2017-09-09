using UnityEngine;
using System.Reflection;
using System.Collections;

public abstract class EditorWindowTool
{

    public bool IsInitialized { get; private set; }

    public void RegisterMethod(MethodInfo method, System.Object target)
    {
        if (IsInitialized)
            return;
        OnRegisterMethod(method, target, false);
    }

    public void RegisterGlobalMethod(MethodInfo method)
    {
        if (IsInitialized)
            return;
        OnRegisterMethod(method, null, true);
    }

    public void Init()
    {
        if (IsInitialized)
            return;
        OnInit();
        IsInitialized = true;
    }

    public void Destroy()
    {
        if (!IsInitialized)
            return;
        OnDestroy();
        IsInitialized = false;
    }

    protected abstract void OnRegisterMethod(MethodInfo method, System.Object target, bool isStatic);

    protected virtual void OnInit() { }

    protected virtual void OnDestroy() { }
}
