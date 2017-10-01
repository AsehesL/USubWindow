using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

namespace EditorWinEx.Internal.Serialization
{
    [Serializable]
    internal class EWSerializationObject
    {
        public System.Object Obj { get { return m_Obj; } }

        System.Object m_Obj;
        [SerializeField]
        string m_ObjectClassName;
        [SerializeField]
        string m_ObjectAssemblyName;

        private EWSerializationObject(System.Object obj)
        {
            this.m_Obj = obj;
            this.m_ObjectAssemblyName = obj.GetType().Assembly.FullName;
            this.m_ObjectClassName = obj.GetType().FullName;
        }

        public static EWSerializationObject CreateInstance(System.Object handle)
        {
            if (handle == null)
                return null;
            return new EWSerializationObject(handle);
        }

        public void SaveObject(string windowID)
        {
            if (this.m_Obj == null)
                return;
            string id = windowID + "." + m_ObjectAssemblyName + "." + m_ObjectClassName;
            EditorPrefsEx.SetObject(id, this.m_Obj);
        }

        public void LoadObject(string windowID)
        {
            if (this.m_Obj != null)
                return;
            string id = windowID + "." + m_ObjectAssemblyName + "." + m_ObjectClassName;
            if (!EditorPrefsEx.HasKey(id))
                return;
            Assembly assembly = Assembly.Load(m_ObjectAssemblyName);
            if (assembly != null)
            {
                Type type = assembly.GetType(m_ObjectClassName);
                if (type != null)
                {
                    this.m_Obj = EditorPrefsEx.GetObject(id, type);
                }
            }
        }

        public void ClearObject(string windowID)
        {
            string id = windowID + "." + m_ObjectAssemblyName + "." + m_ObjectClassName;
            EditorPrefsEx.DeleteKey(id);
        }
    }
}