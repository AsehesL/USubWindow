using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

using UObject = UnityEngine.Object;

/// <summary>
/// 编辑器工具初始化器
/// </summary>
public class EditorWindowToolsInitializer
{

    public static void InitTools(Type[] types, System.Object[] targets, bool useGlobalMethod, params EditorWindowTool[] tools)
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
            RegisterInstanceMethod(types[i], targets[i], tools);
        }

        if (useGlobalMethod)
        {
            Assembly assembly = typeof (EditorWindowToolsInitializer).Assembly;
            Type[] globalTypes = assembly.GetTypes();
            for (int i = 0; i < globalTypes.Length; i++)
            {
                if (!globalTypes[i].IsClass)
                    continue;
                RegisterGlobalMethod(globalTypes[i], tools);
            }
        }

        for (int j = 0; j < tools.Length; j++)
        {
            if (!tools[j].IsInitialized)
                tools[j].Init();
        }
    }

    private static void RegisterInstanceMethod(Type type, System.Object target, EditorWindowTool[] tools)
    {
        MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        for (int i = 0; i < methods.Length; i++)
        {
            for (int j = 0; j < tools.Length; j++)
            {
                if (!tools[j].IsInitialized)
                    tools[j].RegisterMethod(methods[i], target);
            }
        }
    }

    private static void RegisterGlobalMethod(Type type, EditorWindowTool[] tools)
    {
        MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        for (int i = 0; i < methods.Length; i++)
        {
            for (int j = 0; j < tools.Length; j++)
            {
                if (!tools[j].IsInitialized)
                    tools[j].RegisterGlobalMethod(methods[i]);
            }
        }
    }
}
