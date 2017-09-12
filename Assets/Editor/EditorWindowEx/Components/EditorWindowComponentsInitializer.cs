using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

using UObject = UnityEngine.Object;

namespace EditorWinEx.Internal
{
    /// <summary>
    /// EditorWindow组件初始化器
    /// </summary>
    internal class EditorWindowComponentsInitializer
    {
        /// <summary>
        /// 初始化组件
        /// </summary>
        /// <param name="container">组件容器</param>
        /// <param name="types">handle类型列表</param>
        /// <param name="targets">handle列表</param>
        /// <param name="tools">组件列表</param>
        public static void InitComponents(System.Object container, Type[] types, System.Object[] targets,
            params EditorWindowComponentBase[] tools)
        {
            if (targets == null || types == null)
                return;
            if (targets.Length == 0 || types.Length == 0)
                return;
            if (targets.Length != types.Length)
                return;
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i] == null)
                    continue;
                if (targets[i] == null)
                    continue;
                RegisterInstanceMethod(container, types[i], targets[i], tools);
            }

            {
                Assembly assembly = typeof (EditorWindowComponentsInitializer).Assembly;
                Type[] globalTypes = assembly.GetTypes();
                for (int i = 0; i < globalTypes.Length; i++)
                {
                    if (!globalTypes[i].IsClass)
                        continue;
                    RegisterClass(container, globalTypes[i], tools);
                }
            }

            for (int j = 0; j < tools.Length; j++)
            {
                if (!tools[j].IsInitialized)
                    tools[j].Init();
            }
        }

        private static void RegisterInstanceMethod(System.Object container, Type type, System.Object target,
            EditorWindowComponentBase[] tools)
        {
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < methods.Length; i++)
            {
                for (int j = 0; j < tools.Length; j++)
                {
                    if (!tools[j].IsInitialized)
                        tools[j].RegisterMethod(container, methods[i], target);
                }
            }
        }

        private static void RegisterClass(System.Object container, Type type, EditorWindowComponentBase[] tools)
        {
            if (type.IsAbstract)
                return;
            for (int i = 0; i < tools.Length; i++)
            {
                if (!tools[i].IsInitialized)
                    tools[i].RegisterClass(container, type);
            }
        }
    }
}