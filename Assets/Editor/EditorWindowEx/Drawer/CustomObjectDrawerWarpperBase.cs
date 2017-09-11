using UnityEngine;
using System.Collections;

namespace EditorWinEx.Internal
{
    internal abstract class CustomObjectDrawerWarpperBase
    {
        public bool IsInitialized { get; private set; }

        public bool IsEnabled { get; private set; }

        public void Init()
        {
            if (IsInitialized)
                return;
            if (!OnInit())
                return;
            IsInitialized = true;
        }

        public void Enable()
        {
            if (!IsEnabled)
                OnEnable();
            IsEnabled = true;
        }

        public void Disable()
        {
            if (IsEnabled)
                OnDisable();
            IsEnabled = false;
        }

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