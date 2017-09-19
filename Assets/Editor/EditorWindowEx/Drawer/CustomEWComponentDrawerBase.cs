using UnityEngine;
using System.Collections;
using EditorWinEx;
using EditorWinEx.Internal;
using System;

/// <summary>
/// 自定义组件绘制器
/// </summary>
[System.Serializable]
public abstract class CustomEWComponentDrawerBase : IMessageDispatcher
{
    public System.Object Container { get { return container; } }
    /// <summary>
    /// 组件容器
    /// </summary>
    [NonSerialized]
    protected System.Object container;

    /// <summary>
    /// 设置组件容器-只允许初始化时设置，一般不需要手动调用
    /// </summary>
    /// <param name="container"></param>
    public void SetContainer(System.Object container)
    {
        if (this.container != null)
        {
            Debug.LogError("错误，Container只允许在初始化时设置！");
            return;
        }
        this.container = container;
    }

    public abstract void Init();

    public abstract void OnEnable();

    public abstract void OnDisable();

    public abstract void OnDestroy();

    /// <summary>
    /// 返回消息派遣器的容器类型
    /// </summary>
    /// <returns></returns>
    public Type GetContainerType()
    {
        if (container == null)
            return null;
        return container.GetType();
    }
}
