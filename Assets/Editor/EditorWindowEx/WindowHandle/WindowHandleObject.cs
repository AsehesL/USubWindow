using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

namespace EditorWinEx.Internal
{
    [Serializable]
    internal class WindowHandleObject
    {
        public System.Object Handle { get { return m_Handle; } }

        System.Object m_Handle;
        [SerializeField]
        string m_HandleClassName;
        [SerializeField]
        string m_HandleAssemblyName;

        private WindowHandleObject(System.Object handle)
        {
            this.m_Handle = handle;
            this.m_HandleAssemblyName = handle.GetType().Assembly.FullName;
            this.m_HandleClassName = handle.GetType().FullName;
        }

        public static WindowHandleObject CreateInstance(System.Object handle)
        {
            if (handle == null)
                return null;
            return new WindowHandleObject(handle);
        }

        public void SaveHandle(string windowID)
        {
            if (this.m_Handle == null)
                return;
            string id = windowID + "." + m_HandleAssemblyName + "." + m_HandleClassName;
            EditorPrefsEx.SetObject(id, this.m_Handle);
        }

        public void LoadHandle(string windowID)
        {
            if (this.m_Handle != null)
                return;
            string id = windowID + "." + m_HandleAssemblyName + "." + m_HandleClassName;
            if (!EditorPrefsEx.HasKey(id))
                return;
            Assembly assembly = Assembly.Load(m_HandleAssemblyName);
            if (assembly != null)
            {
                Type type = assembly.GetType(m_HandleClassName);
                if (type != null)
                {
                    this.m_Handle = EditorPrefsEx.GetObject(id, type);
                }
            }
        }

        public void ClearHandle(string windowID)
        {
            string id = windowID + "." + m_HandleAssemblyName + "." + m_HandleClassName;
            EditorPrefsEx.DeleteKey(id);
        }
    }
}