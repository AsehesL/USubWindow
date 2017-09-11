using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace EditorWinEx
{
    public delegate bool ConditionDelegate(System.Object arg);

}

namespace EditorWinEx.Internal
{
    /// <summary>
    /// 工具栏Item节点
    /// </summary>
    internal class ToolBarTreeNode
    {
        /// <summary>
        /// 子节点数
        /// </summary>
        public int count
        {
            get { return m_NodeList.Count; }
        }

        private string m_Text;
        //    private MethodInfo m_Method;
        //
        //    private System.Object m_Target;
        private Delegate m_Action;
        private ConditionDelegate m_Condition;
        private System.Object m_ArgObj;

        private List<ToolBarTreeNode> m_NodeList;

        private int m_Priority;

        public ToolBarTreeNode(string text, int priority)
        {
            this.m_NodeList = new List<ToolBarTreeNode>();
            this.m_Text = text;
            this.m_Priority = priority;
        }

        /// <summary>
        /// 插入节点
        /// </summary>
        /// <param name="nodetext">节点显示文本</param>
        /// <param name="method">事件方法</param>
        /// <param name="target">事件响应对象</param>
        /// <param name="priority">优先级</param>
        public void InsertNode(string nodetext, MethodInfo method, System.Object target, ConditionDelegate condition,
            System.Object argobj, int priority)
        {
            if (string.IsNullOrEmpty(nodetext))
                return;
            while (nodetext.Length > 0 && nodetext[0] == '/')
            {
                nodetext = nodetext.Substring(1, nodetext.Length - 1);
            }
            if (string.IsNullOrEmpty(nodetext))
                return;
            int first = nodetext.IndexOf('/');
            string lasttext = null;
            bool hasChild = false;
            if (first > 0)
            {
                lasttext = nodetext.Substring(first + 1);
                nodetext = nodetext.Substring(0, first);
                if (!string.IsNullOrEmpty(lasttext))
                    hasChild = true;
            }
            AddNode(nodetext, lasttext, priority, method, target, condition, argobj, hasChild);
        }

        private void AddNode(string nodeText, string lasttext, int priority, MethodInfo method, System.Object target,
            ConditionDelegate condition, System.Object argobj, bool hasChild)
        {
            ToolBarTreeNode node = this.m_NodeList.Find(n => n.m_Text == nodeText);
            if (hasChild)
            {
                if (node == null)
                {
                    node = new ToolBarTreeNode(nodeText, priority);
                    node.InsertNode(lasttext, method, target, condition, argobj, priority);
                    this.m_NodeList.Add(node);
                }
                else
                {
                    node.InsertNode(lasttext, method, target, condition, argobj, priority);
                }
            }
            else
            {
                if (node == null)
                {
                    node = new ToolBarTreeNode(nodeText, priority);
                    node.CombineMethod(method, target, condition, argobj);
                    this.m_NodeList.Add(node);
                }
            }
        }

        /// <summary>
        /// 根据优先级排序
        /// </summary>
        public void Sort()
        {
            if (m_NodeList != null && m_NodeList.Count > 0)
            {
                m_NodeList.Sort((a, b) => { return a.m_Priority - b.m_Priority; });
                for (int i = 0; i < m_NodeList.Count; i++)
                {
                    m_NodeList[i].Sort();
                }
            }
        }

        /// <summary>
        /// 绘制工具栏
        /// </summary>
        public void DrawToolBar()
        {
            for (int i = 0; i < m_NodeList.Count; i++)
            {
                Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(70), GUILayout.Height(17));
                if (GUIEx.ToolbarButton(rect, m_NodeList[i].m_Text))
                {
                    ClickDropDown(rect, m_NodeList[i]);
                }
            }
        }

        /// <summary>
        /// 方法合并
        /// </summary>
        /// <param name="method"></param>
        /// <param name="target"></param>
        /// <param name="condition"></param>
        /// <param name="argobj"></param>
        private void CombineMethod(MethodInfo method, System.Object target, ConditionDelegate condition,
            System.Object argobj)
        {
            m_ArgObj = argobj;
            if (m_ArgObj != null)
                m_Condition = condition;
            else
                m_Condition = null;
            if (m_Action == null)
            {
                if (m_ArgObj == null)
                    m_Action = Delegate.CreateDelegate(typeof (System.Action), target, method);
                else
                    m_Action = Delegate.CreateDelegate(typeof (System.Action<System.Object>), target, method);
            }
        }

        /// <summary>
        /// 点击工具栏
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="node"></param>
        private void ClickDropDown(Rect rect, ToolBarTreeNode node)
        {
            if (node.m_NodeList == null || node.m_NodeList.Count == 0)
            {
                node.Invoke();
                return;
            }
            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < node.m_NodeList.Count; i++)
            {
                node.m_NodeList[i].ShowDropDownMenu(menu, "", false);
            }
            menu.DropDown(rect);
        }

        /// <summary>
        /// 显示工具栏
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="text"></param>
        /// <param name="separator"></param>
        private void ShowDropDownMenu(GenericMenu menu, string text, bool separator)
        {
            string ntext = text + "/" + m_Text;
            if (m_NodeList == null || m_NodeList.Count == 0)
            {
                ntext = ntext.TrimStart('/');
                if (separator)
                {
                    menu.AddSeparator((text + "/").TrimStart('/'));
                }
                if (m_Condition != null && m_ArgObj != null)
                {
                    bool r = m_Condition(m_ArgObj);
                    menu.AddItem(new GUIContent(ntext), r, Invoke);
                }
                else
                {
                    menu.AddItem(new GUIContent(ntext), false, Invoke);
                }

                return;
            }
            int bound = 0;
            if (m_NodeList.Count > 0)
            {
                bound = (m_NodeList[0].m_Priority/1000);
            }
            for (int i = 0; i < m_NodeList.Count; i++)
            {
                int cBound = m_NodeList[i].m_Priority/1000;
                if (cBound > bound)
                {
                    bound = cBound;
                    m_NodeList[i].ShowDropDownMenu(menu, ntext, true);
                }
                else
                    m_NodeList[i].ShowDropDownMenu(menu, ntext, false);
            }
        }

        /// <summary>
        /// 工具栏按钮事件动态执行
        /// </summary>
        private void Invoke()
        {
            if (m_Action != null)
            {
                if (m_ArgObj != null)
                    m_Action.DynamicInvoke(m_ArgObj);
                else
                    m_Action.DynamicInvoke();
            }
        }
    }
}
