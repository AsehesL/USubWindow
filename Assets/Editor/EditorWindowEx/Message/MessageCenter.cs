using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace EditorWinEx.Internal
{
    internal class MessageCenter
    {
        public delegate void MessageHandle();

        public delegate void MessageHandle<T>(T arg);

        public delegate void MessageHandle<T1, T2>(T1 arg0, T2 arg1);

        public delegate void MessageHandle<T1, T2, T3>(T1 arg0, T2 arg1, T3 arg2);

        public delegate void MessageHandle<T1, T2, T3, T4>(T1 arg0, T2 arg1, T3 arg2, T4 arg3);

        private static Dictionary<string, Dictionary<int, Delegate>> m_Messages;

        public static void AddListener(Type containterType, int messageId, MessageHandle handle)
        {
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

        public static void AddListener<T>(Type containterType, int messageId, MessageHandle<T> handle)
        {
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

        public static void AddListener<T1, T2>(Type containterType, int messageId, MessageHandle<T1, T2> handle)
        {
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

        public static void AddListener<T1, T2, T3>(Type containterType, int messageId, MessageHandle<T1, T2, T3> handle)
        {
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

        public static void AddListener<T1, T2, T3, T4>(Type containterType, int messageId, MessageHandle<T1, T2, T3, T4> handle)
        {
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

        public static void RemoveListener(Type containterType, int messageId, MessageHandle handle)
        {
            if (handle == null)
                return;
            if (containterType == null)
                return;
            string typeName = containterType.FullName;
            if (string.IsNullOrEmpty(typeName))
                return;
            if (!m_Messages.ContainsKey(typeName))
                return;
            var messages = m_Messages[typeName];
            if (messages == null)
                return;
            if (!messages.ContainsKey(messageId))
                return;
            var message = messages[messageId];
        }

        private static Dictionary<int, Delegate> GetMessagesOfType(Type containterType)
        {
            if (containterType == null)
                return null;
            string typeName = containterType.FullName;
            if (string.IsNullOrEmpty(typeName))
                return null;
            if (m_Messages == null)
                m_Messages = new Dictionary<string, Dictionary<int, Delegate>>();
            Dictionary<int, Delegate> messages = null;
            if (m_Messages.ContainsKey(typeName))
                messages = m_Messages[typeName];
            else
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
    }
}