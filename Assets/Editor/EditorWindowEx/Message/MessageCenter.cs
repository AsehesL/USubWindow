using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace EditorWinEx.Internal
{
    public delegate void MessageHandle();

    public delegate void MessageHandle<T>(T arg);

    public delegate void MessageHandle<T0, T1>(T0 arg0, T1 arg1);

    public delegate void MessageHandle<T0, T1, T2>(T0 arg0, T1 arg1, T2 arg2);

    public delegate void MessageHandle<T0, T1, T2, T3>(T0 arg0, T1 arg1, T2 arg2, T3 arg3);

    /// <summary>
    /// 消息中心
    /// 消息中心扩展消息派遣器的方法，实现消息监听、移除、广播
    /// </summary>
    internal static class MessageCenter
    {
        /// <summary>
        /// 根据容器类型分组消息，只有同一个容器中的不同组件之间可以监听、广播消息
        /// </summary>
        private static Dictionary<string, Dictionary<int, Delegate>> m_Messages;

        public static void AddListener(this IMessageDispatcher dispatcher, int messageId, MessageHandle handle)
        {
            Type containterType = dispatcher.GetContainerType();
            if (handle == null)
                return;
            var messages = GetMessagesOfType(containterType);
            if (messages == null)
                return;
            if (CombineMethod(messageId, messages, handle))
            {
                messages[messageId] = (MessageHandle)Delegate.Combine(messages[messageId], handle);
            }
        }

        public static void AddListener<T>(this IMessageDispatcher dispatcher, int messageId, MessageHandle<T> handle)
        {
            Type containterType = dispatcher.GetContainerType();
            if (handle == null)
                return;
            var messages = GetMessagesOfType(containterType);
            if (messages == null)
                return;
            if (CombineMethod(messageId, messages, handle))
            {
                messages[messageId] = (MessageHandle<T>)Delegate.Combine(messages[messageId], handle);
            }
        }

        public static void AddListener<T0, T1>(this IMessageDispatcher dispatcher, int messageId, MessageHandle<T0, T1> handle)
        {
            Type containterType = dispatcher.GetContainerType();
            if (handle == null)
                return;
            var messages = GetMessagesOfType(containterType);
            if (messages == null)
                return;
            if (CombineMethod(messageId, messages, handle))
            {
                messages[messageId] = (MessageHandle<T0, T1>)Delegate.Combine(messages[messageId], handle);
            }
        }

        public static void AddListener<T0, T1, T2>(this IMessageDispatcher dispatcher, int messageId, MessageHandle<T0, T1, T2> handle)
        {
            Type containterType = dispatcher.GetContainerType();
            if (handle == null)
                return;
            var messages = GetMessagesOfType(containterType);
            if (messages == null)
                return;
            if (CombineMethod(messageId, messages, handle))
            {
                messages[messageId] = (MessageHandle<T0, T1, T2>)Delegate.Combine(messages[messageId], handle);
            }
        }

        public static void AddListener<T0, T1, T2, T3>(this IMessageDispatcher dispatcher, int messageId, MessageHandle<T0, T1, T2, T3> handle)
        {
            Type containterType = dispatcher.GetContainerType();
            if (handle == null)
                return;
            var messages = GetMessagesOfType(containterType);
            if (messages == null)
                return;
            if (CombineMethod(messageId, messages, handle))
            {
                messages[messageId] = (MessageHandle<T0, T1, T2, T3>)Delegate.Combine(messages[messageId], handle);
            }
        }

        public static void RemoveListener(this IMessageDispatcher dispatcher, int messageId, MessageHandle handle)
        {
            Type containterType = dispatcher.GetContainerType();
            if (handle == null)
                return;
            var messages = GetMessagesOfType(containterType, false);
            if (messages != null)
            {
                if(RemoveMethod(messageId, messages, handle))
                {
                    messages[messageId] = (MessageHandle) Delegate.Remove(messages[messageId], handle);
                    RemoveEmptyMessage(messageId, containterType);
                }
            }
        }

        public static void RemoveListener<T>(this IMessageDispatcher dispatcher, int messageId, MessageHandle<T> handle)
        {
            Type containterType = dispatcher.GetContainerType();
            if (handle == null)
                return;
            var messages = GetMessagesOfType(containterType, false);
            if (messages != null)
            {
                if (RemoveMethod(messageId, messages, handle))
                {
                    messages[messageId] = (MessageHandle<T>)Delegate.Remove(messages[messageId], handle);
                    RemoveEmptyMessage(messageId, containterType);
                }
            }
        }

        public static void RemoveListener<T0, T1>(this IMessageDispatcher dispatcher, int messageId, MessageHandle<T0, T1> handle)
        {
            Type containterType = dispatcher.GetContainerType();
            if (handle == null)
                return;
            var messages = GetMessagesOfType(containterType, false);
            if (messages != null)
            {
                if (RemoveMethod(messageId, messages, handle))
                {
                    messages[messageId] = (MessageHandle<T0, T1>)Delegate.Remove(messages[messageId], handle);
                    RemoveEmptyMessage(messageId, containterType);
                }
            }
        }

        public static void RemoveListener<T0, T1, T2>(this IMessageDispatcher dispatcher, int messageId, MessageHandle<T0, T1, T2> handle)
        {
            Type containterType = dispatcher.GetContainerType();
            if (handle == null)
                return;
            var messages = GetMessagesOfType(containterType, false);
            if (messages != null)
            {
                if (RemoveMethod(messageId, messages, handle))
                {
                    messages[messageId] = (MessageHandle<T0, T1, T2>)Delegate.Remove(messages[messageId], handle);
                    RemoveEmptyMessage(messageId, containterType);
                }
            }
        }

        public static void RemoveListener<T0, T1, T2, T3>(this IMessageDispatcher dispatcher, int messageId, MessageHandle<T0, T1, T2, T3> handle)
        {
            Type containterType = dispatcher.GetContainerType();
            if (handle == null)
                return;
            var messages = GetMessagesOfType(containterType, false);
            if (messages != null)
            {
                if (RemoveMethod(messageId, messages, handle))
                {
                    messages[messageId] = (MessageHandle<T0, T1, T2, T3>)Delegate.Remove(messages[messageId], handle);
                    RemoveEmptyMessage(messageId, containterType);
                }
            }
        }

        public static void Broadcast(this IMessageDispatcher dispatcher, int messageId)
        {
            Type containterType = dispatcher.GetContainerType();
            var method = GetMethod(containterType, messageId);
            if (method != null)
            {
                MessageHandle handle = (MessageHandle) method;
                if (handle != null)
                    handle.Invoke();
            }
        }

        public static void Broadcast<T>(this IMessageDispatcher dispatcher, int messageId, T arg)
        {
            Type containterType = dispatcher.GetContainerType();
            var method = GetMethod(containterType, messageId);
            if (method != null)
            {
                MessageHandle<T> handle = (MessageHandle<T>)method;
                if (handle != null)
                    handle.Invoke(arg);
            }
        }

        public static void Broadcast<T0, T1>(this IMessageDispatcher dispatcher, int messageId, T0 arg0, T1 arg1)
        {
            Type containterType = dispatcher.GetContainerType();
            var method = GetMethod(containterType, messageId);
            if (method != null)
            {
                MessageHandle<T0, T1> handle = (MessageHandle<T0, T1>)method;
                if (handle != null)
                    handle.Invoke(arg0, arg1);
            }
        }

        public static void Broadcast<T0, T1, T2>(this IMessageDispatcher dispatcher, int messageId, T0 arg0, T1 arg1, T2 arg2)
        {
            Type containterType = dispatcher.GetContainerType();
            var method = GetMethod(containterType, messageId);
            if (method != null)
            {
                MessageHandle<T0, T1, T2> handle = (MessageHandle<T0, T1, T2>)method;
                if (handle != null)
                    handle.Invoke(arg0, arg1, arg2);
            }
        }

        public static void Broadcast<T0, T1, T2, T3>(this IMessageDispatcher dispatcher, int messageId, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            Type containterType = dispatcher.GetContainerType();
            var method = GetMethod(containterType, messageId);
            if (method != null)
            {
                MessageHandle<T0, T1, T2, T3> handle = (MessageHandle<T0, T1, T2, T3>)method;
                if (handle != null)
                    handle.Invoke(arg0, arg1, arg2, arg3);
            }
        }

        private static Dictionary<int, Delegate> GetMessagesOfType(Type containterType, bool createIfNotExists = true)
        {
            if (containterType == null)
                return null;
            string typeName = containterType.FullName;
            if (string.IsNullOrEmpty(typeName))
                return null;
            if (m_Messages == null)
            {
                if (createIfNotExists)
                    m_Messages = new Dictionary<string, Dictionary<int, Delegate>>();
                else
                    return null;
            }
            Dictionary<int, Delegate> messages = null;
            if (m_Messages.ContainsKey(typeName))
                messages = m_Messages[typeName];
            else if(createIfNotExists)
            {
                messages = new Dictionary<int, Delegate>();
                m_Messages.Add(typeName, messages);
            }
            return messages;
        }

        private static bool CombineMethod(int messageId, Dictionary<int, Delegate> messages, Delegate handle)
        {
            Delegate del = null;
            if (messages.ContainsKey(messageId))
            {
                del = messages[messageId];
                if (del.GetType() == handle.GetType())
                {
                    return true;
                }
            }
            else
            {
                messages.Add(messageId, handle);
            }
            return false;
        }

        private static bool RemoveMethod(int messageId, Dictionary<int, Delegate> messages, Delegate handle)
        {
            if (messages == null)
                return false;
            Delegate message = null;
            if (messages.ContainsKey(messageId))
                message = messages[messageId];
            else
                return false;
            if (message.GetType() == handle.GetType())
            {
                return true;
            }
            return false;
        }

        private static Delegate GetMethod(Type containterType, int messageId)
        {
            if (containterType == null)
                return null;
            string typeName = containterType.FullName;
            if (string.IsNullOrEmpty(typeName))
                return null;
            if (m_Messages.ContainsKey(typeName))
            {
                var messages = m_Messages[typeName];
                if (messages != null && messages.ContainsKey(messageId))
                {
                    return messages[messageId];
                }
            }
            return null;
        }

        private static void RemoveEmptyMessage(int messageId, Type containterType)
        {
            if (containterType == null)
                return;
            string typeName = containterType.FullName;
            if (string.IsNullOrEmpty(typeName))
                return;
            
            if (m_Messages.ContainsKey(typeName))
            {
                Dictionary<int, Delegate> messages = m_Messages[typeName];
                if (messages.ContainsKey(messageId) && messages[messageId] == null)
                {
                    messages.Remove(messageId);
                }
                if (messages.Count == 0)
                    m_Messages.Remove(typeName);
            }

        }
    }
}