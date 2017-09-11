using UnityEngine;
using System.Collections;

public abstract class CustomObjectDrawerBase
{

    public System.Object container;

    public abstract void Init();

    public abstract void OnEnable();

    public abstract void OnDisable();

    public abstract void OnDestroy();
}
