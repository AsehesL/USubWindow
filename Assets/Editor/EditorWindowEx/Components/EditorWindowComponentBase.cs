using System;
using UnityEngine;
using System.Reflection;
using System.Collections;

namespace EditorWinEx
{
    /// <summary>
    /// EditorWindow组件基类
    /// </summary>
    public abstract class EditorWindowComponentBase
    {
        /// <summary>
        /// 是否初始化完成
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// 为组件注册方法-该方法由组件初始化器在初始化时调用，一般不需要手动调用
        /// </summary>
        /// <param name="container">组件容器</param>
        /// <param name="method">方法对象</param>
        /// <param name="target">方法目标</param>
        public void RegisterMethod(System.Object container, MethodInfo method, System.Object target)
        {
            if (IsInitialized)
                return;
            OnRegisterMethod(container, method, target);
        }

        /// <summary>
        /// 为组件注册对象-该方法由组件初始化器在初始化时调用，一般不需要手动调用
        /// </summary>
        /// <param name="container">组件容器</param>
        /// <param name="type">对象类型</param>
        public void RegisterClass(System.Object container, Type type)
        {
            if (IsInitialized)
                return;
            OnRegisterClass(container, type);
        }

        /// <summary>
        /// 组件初始化
        /// </summary>
        public void Init()
        {
            if (IsInitialized)
                return;
            OnInit();
            IsInitialized = true;
        }

        /// <summary>
        /// 组件销毁
        /// </summary>
        public void Destroy()
        {
            if (!IsInitialized)
                return;
            OnDestroy();
            IsInitialized = false;
        }

        public void Disable()
        {
            if (!IsInitialized)
                return;
            OnDisable();
        }

        protected abstract void OnRegisterMethod(System.Object container, MethodInfo method, System.Object target);

        protected abstract void OnRegisterClass(System.Object container, Type type);

        protected virtual void OnInit()
        {
        }

        protected virtual void OnDestroy()
        {
        }

        protected virtual void OnDisable() { }
    }
}