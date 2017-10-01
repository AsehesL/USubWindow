using UnityEngine;
using System.Collections;

namespace EditorWinEx.Internal
{
    /// <summary>
    /// 组件绘制器基类
    /// </summary>
    internal abstract class EWComponentDrawerBase
    {
        /// <summary>
        /// 是否初始化
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// 初始化绘制器
        /// </summary>
        public void Init()
        {
            if (IsInitialized)
                return;
            if (!OnInit())
                return;
            IsInitialized = true;
        }

        /// <summary>
        /// 激活绘制器
        /// </summary>
        public void Enable()
        {
            if (!IsEnabled)
                OnEnable();
            IsEnabled = true;
        }

        /// <summary>
        /// 关闭绘制器
        /// </summary>
        public void Disable()
        {
            if (IsEnabled)
                OnDisable();
            IsEnabled = false;
        }

        /// <summary>
        /// 销毁绘制器
        /// </summary>
        public void Destroy()
        {
            Disable();
            OnDestroy();
            IsInitialized = false;
        }

        protected abstract bool OnInit();

        protected abstract void OnEnable();

        protected abstract void OnDisable();

        protected abstract void OnDestroy();
    }
}