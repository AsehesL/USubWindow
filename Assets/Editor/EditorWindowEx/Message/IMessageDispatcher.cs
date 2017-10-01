using System;
using UnityEngine;
using System.Collections;

namespace EditorWinEx
{
    /// <summary>
    /// 消息派遣器
    /// </summary>
    public interface IMessageDispatcher
    {
        /// <summary>
        /// 需要实现返回派遣器的容器类型-相同容器的组件可以相互监听和广播消息，并且不会影响其它容器
        /// </summary>
        /// <returns></returns>
        Type GetContainerType();
    }
}
