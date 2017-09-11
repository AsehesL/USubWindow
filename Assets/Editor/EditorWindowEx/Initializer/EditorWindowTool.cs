using System;
using UnityEngine;
using System.Reflection;
using System.Collections;

namespace EditorWinEx
{
    public abstract class EditorWindowTool
    {

        public bool IsInitialized { get; private set; }

        public void RegisterMethod(System.Object container, MethodInfo method, System.Object target)
        {
            if (IsInitialized)
                return;
            OnRegisterMethod(container, method, target);
        }

        public void RegisterClass(System.Object container, Type type)
        {
            if (IsInitialized)
                return;
            OnRegisterClass(container, type);
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

        protected abstract void OnRegisterMethod(System.Object container, MethodInfo method, System.Object target);

        protected abstract void OnRegisterClass(System.Object container, Type type);

        protected virtual void OnInit()
        {
        }

        protected virtual void OnDestroy()
        {
        }
    }
}