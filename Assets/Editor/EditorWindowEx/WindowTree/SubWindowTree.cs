using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;

namespace EditorWinEx.Internal
{
    /// <summary>
    /// 子窗体树
    /// </summary>
    internal class SubWindowTree : EditorWindowComponentBase
    {
        /// <summary>
        /// 当前使用中的布局-null为默认布局
        /// </summary>
        //public string CurrentLayout { get { return m_CurrentLayout; } }
        /// <summary>
        /// 根节点
        /// </summary>
        private SubWindowNode m_Root;

        /// <summary>
        /// 重绘方法句柄
        /// </summary>
        private System.Action m_Repaint;

        /// <summary>
        /// 当前拖拽的窗口
        /// </summary>
        private SubWindow m_CurrentDrag;

        private bool m_IsDraging;

        /// <summary>
        /// 放置窗口的预处理函数句柄
        /// </summary>
        private System.Action<SubWindow> m_PreAction;

        /// <summary>
        /// 放置窗口的后处理函数局部
        /// </summary>
        private System.Action m_PostAction;

        /// <summary>
        /// 子窗口的关闭响应事件
        /// </summary>
        private System.Action<SubWindow> m_OnSubWindowClose;

        /// <summary>
        /// 树中注册的子窗体列表
        /// </summary>
        private List<SubWindow> m_SubWindowList = new List<SubWindow>();

        private SubWindowLayout m_Layout;

        public SubWindowTree(System.Action repaint, string windowName, string handleName)
        {
            m_Repaint = repaint;
            m_PreAction = this.PreDropAction;
            m_PostAction = this.PostDropAction;
            m_OnSubWindowClose = this.OnSubWindowClosw;
            this.m_Layout = new SubWindowLayout(windowName, handleName);
        }

        protected override void OnRegisterMethod(System.Object container, MethodInfo method, System.Object target)
        {
            System.Object[] atts = method.GetCustomAttributes(typeof (EWSubWindowAttribute), false);

            for (int j = 0; j < atts.Length; j++)
            {
                EWSubWindowAttribute att = (EWSubWindowAttribute) atts[j];
                System.Object obj = SubWindowFactory.CreateSubWindow(att.windowStyle, att.title, att.iconPath,
                    att.active,
                    method, target, att.toolbar, att.helpBox);

                if (obj != null)
                {
                    m_SubWindowList.Add((SubWindow)obj);
                }
            }
        }

        protected override void OnRegisterClass(System.Object container, Type type)
        {
            if (container == null)
                return;
            if (!type.IsSubclassOf(typeof (SubWindowCustomDrawer)))
                return;
            System.Object[] atts = type.GetCustomAttributes(typeof (EWSubWindowHandleAttribute), false);
            for (int i = 0; i < atts.Length; i++)
            {
                EWSubWindowHandleAttribute att = (EWSubWindowHandleAttribute) atts[i];
                if (att == null)
                    continue;
                if (att.containerType != container.GetType())
                    continue;
                System.Object obj = SubWindowFactory.CreateSubWindow(container, att.active, att.windowStyle, type);
                if (obj != null)
                {
                    m_SubWindowList.Add((SubWindow)obj);
                }
            }
        }

        protected override void OnInit()
        {
            base.OnInit();
            if (!UseLayout("Default"))
                UseDefaultLayout();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            RemoveAllDynamicWindow();
            SaveCurrentLayout();
            DestroyAllWindow();
        }

        public void AddDynamicWindow(string title, string icon, EWSubWindowToolbarType toolbar, SubWindowHelpBoxType helpbox,
        Delegate method)
        {
            if (method == null)
                return;
            string id = SubWindowMethodDrawer.GetMethodID(method.Method, method.Target);
            SubWindow window = new SubWindow(title, icon, true, method.Method, method.Target, toolbar, helpbox);
            window.isDynamic = true;
            m_SubWindowList.Add(window);
            window.Open();
            this.InsertWindow(window);
        }

        public void AddDynamicWindow<T>(T drawer) where T : SubWindowCustomDrawer
        {
            if (drawer == null)
                return;
            string id = SubWindowObjectDrawer.GetDrawerID(drawer, true);
            SubWindow window = new SubWindow(true, drawer);
            window.isDynamic = true;
            m_SubWindowList.Add(window);
            window.Open();
            this.InsertWindow(window);
        }

        public bool RemoveDynamicWindow(Delegate method)
        {
            string id = null;
            if (method == null)
                id = SubWindowMethodDrawer.GetMethodID(null, null);
            else
                id = SubWindowMethodDrawer.GetMethodID(method.Method, method.Target);
            return RemoveWindowByID(id);
        }

        public bool RemoveDynamicWindow<T>(T drawer) where T : SubWindowCustomDrawer
        {
            string id = null;
            if (drawer == null)
                id = SubWindowObjectDrawer.GetDrawerID(null, true);
            else
                id = SubWindowObjectDrawer.GetDrawerID(drawer, true);
            return RemoveWindowByID(id);
        }

        /// <summary>
        /// 根据ID移除窗口
        /// </summary>
        /// <param name="windowId">窗口ID</param>
        private bool RemoveWindowByID(string windowId)
        {
            if (string.IsNullOrEmpty(windowId))
                return false;
            if (this.m_SubWindowList != null)
            {
                for (int i = 0; i < m_SubWindowList.Count; i++)
                {
                    if (this.m_SubWindowList[i].GetIndentifier() == windowId)
                    {
                        var win = this.m_SubWindowList[i];
                        if (win.IsOpen)
                        {
                            if (this.m_Root != null)
                            {
                                this.m_Root.RemoveWindow(win);
                                this.m_Root.ClearEmptyNode();
                                this.m_Root.Recalculate(0, true);
                            }
                        }
                        this.m_SubWindowList.RemoveAt(i);
                        return true;
                    }
                }
            }
            return false;
        }

        public void RemoveAllDynamicWindow()
        {
            if (this.m_SubWindowList != null)
            {
                bool hasRemoved = false;
                for (int i = 0; i < m_SubWindowList.Count; i++)
                {
                    if (this.m_SubWindowList[i].isDynamic)
                    {
                        var win = this.m_SubWindowList[i];
                        if (win.IsOpen)
                        {
                            if (this.m_Root != null)
                            {
                                this.m_Root.RemoveWindow(win);
                                hasRemoved = true;
                            }
                        }
                        win.Destroy();
                        this.m_SubWindowList.RemoveAt(i);
                    }
                }
                if (hasRemoved)
                {
                    if (this.m_Root != null)
                    {
                        this.m_Root.ClearEmptyNode();
                        this.m_Root.Recalculate(0, true);
                    }
                }
            }
        }

        private void DestroyAllWindow()
        {
            if (this.m_SubWindowList != null)
            {
                for (int i = 0; i < m_SubWindowList.Count; i++)
                {
                    if (m_SubWindowList[i] != null)
                    {
                        m_SubWindowList[i].Destroy();
                    }
                }
                m_SubWindowList.Clear();
            }
        }

        /// <summary>
        /// 是否包含指定ID的窗口
        /// </summary>
        /// <param name="windowId"></param>
        public bool ContainWindowID(string windowId)
        {
            if (this.m_SubWindowList != null)
            {
                for (int i = 0; i < m_SubWindowList.Count; i++)
                {
                    if (this.m_SubWindowList[i].GetIndentifier() == windowId)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 绘制树
        /// </summary>
        /// <param name="rect">区域</param>
        public void DrawWindowTree(Rect rect)
        {
            if (this.m_Root != null)
                this.m_Root.DrawGUI(rect, m_Repaint);
            ListenEvent();
        }

        /// <summary>
        /// 绘制工具栏-视图按钮
        /// </summary>
        public void DrawViewButton()
        {
            Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(70), GUILayout.Height(17));
            if (GUIEx.ToolbarButton(rect, "视图"))
            {
                if (m_Root != null)
                {
                    GenericMenu menu = new GenericMenu();
                    for (int i = 0; i < m_SubWindowList.Count; i++)
                    {
                        menu.AddItem(new GUIContent(m_SubWindowList[i].Title), m_SubWindowList[i].IsOpen,
                            OnSetSubWindowActive, m_SubWindowList[i]);
                    }
                    menu.DropDown(rect);
                }
            }
        }

        /// <summary>
        /// 绘制工具栏-布局按钮
        /// </summary>
        /// <param name="rect"></param>
        public void DrawLayoutButton(Rect rect)
        {
            if (GUI.Button(rect, "Layout", GUIStyleCache.GetStyle("ToolbarDropDown")))
            {
                if (m_Layout != null && m_Layout.Layouts != null && m_Layout.Layouts.Count > 0)
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Default"), false, this.UseDefaultLayout);
                    for (int i = 0; i < m_Layout.Layouts.Count; i++)
                    {
                        menu.AddItem(new GUIContent(m_Layout.Layouts[i]), false, this.OnUseLayout, m_Layout.Layouts[i]);
                    }
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Save Layout..."), false, this.OnSaveLayout);
                    menu.AddItem(new GUIContent("Delete Layout..."), false, this.OnDeleteLayout);
                    menu.AddItem(new GUIContent("Clear All Layout..."), false, this.OnRevertLayout);
                    menu.DropDown(rect);
                }
            }
        }

        public void SaveCurrentLayout()
        {
            SaveLayoutCfgs("Default");
        }

        /// <summary>
        /// 插入子窗口
        /// </summary>
        /// <param name="window"></param>
        private void InsertWindow(SubWindow window)
        {
            window.AddCloseEventListener(this.m_OnSubWindowClose);
            if (this.m_Root == null)
            {
                this.m_Root = new SubWindowNode(true, 0);
            }
            this.m_Root.AddWindow(window, 0);

        }

        private void ListenEvent()
        {
            if (m_Root == null)
                return;
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                m_CurrentDrag = m_Root.DragWindow(Event.current.mousePosition);
                if (m_CurrentDrag != null)
                {
                    m_IsDraging = true;
                    Event.current.Use();
                }
            }
            if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
            {
                if (m_IsDraging)
                {
                    m_IsDraging = false;
                    if (m_CurrentDrag != null)
                    {
                        m_Root.DropWindow(Event.current.mousePosition, 0, m_CurrentDrag, m_PreAction, m_PostAction);
                        Event.current.Use();
                    }
                    m_CurrentDrag = null;
                }
            }
            if (m_IsDraging)
            {
                m_Root.DrawAnchorArea(Event.current.mousePosition, 0, m_CurrentDrag);
                DrawFloatingWindow(Event.current.mousePosition);
                if (m_Repaint != null)
                {
                    m_Repaint();
                }
            }
        }

        private void DrawFloatingWindow(Vector2 position)
        {
            if (m_CurrentDrag == null)
                return;
            GUI.Box(new Rect(position.x - 50, position.y - 10, 100, 40), m_CurrentDrag.Title,
                GUIStyleCache.GetStyle("window"));
        }

        /// <summary>
        /// 子窗口关闭事件
        /// </summary>
        /// <param name="subWindow"></param>
        private void OnSubWindowClosw(SubWindow subWindow)
        {
            this.PreDropAction(subWindow);
            this.PostDropAction();
        }

        /// <summary>
        /// 放置子窗口的预处理
        /// </summary>
        /// <param name="subWindow"></param>
        private void PreDropAction(SubWindow subWindow)
        {
            if (this.m_Root != null)
            {
                if (subWindow != null)
                    this.m_Root.RemoveWindow(subWindow); //从树中删除该子窗口
            }
        }

        /// <summary>
        /// 放置子窗口的后处理
        /// </summary>
        private void PostDropAction()
        {
            if (this.m_Root != null)
            {
                this.m_Root.ClearEmptyNode(); //清除空节点
                this.m_Root.Recalculate(0, true); //重新计算树的各节点参数
            }
        }

        private void OnSetSubWindowActive(System.Object subwindow)
        {
            if (subwindow == null)
                return;
            SubWindow window = (SubWindow) subwindow;
            if (window.IsOpen)
            {
                window.Close();
            }
            else
            {
                this.InsertWindow(window);
            }
        }

        private bool UseLayout(string layoutName)
        {
            if (string.IsNullOrEmpty(layoutName))
                return false;
            string treeId = GetTreeIndentifier();
            if (string.IsNullOrEmpty(treeId))
                return false;
            if (m_Layout == null)
                return false;
            if (m_Root == null)
                m_Root = new SubWindowNode(true, 0);
            var element = m_Layout.UseLayout(layoutName, treeId);
            if (element == null)
                return false;
            for (int i = 0; i < m_SubWindowList.Count; i++)
            {
                if (m_SubWindowList[i].IsOpen)
                {
                    m_SubWindowList[i].Close();
                }
            }

            m_Root.CreateFromLayoutCfg(element, m_SubWindowList, m_OnSubWindowClose);
            m_Root.ClearEmptyNode();
            m_Root.Recalculate(0, true);

            return true;
        }

        private void SaveLayoutCfgs(string layoutName)
        {
            if (string.IsNullOrEmpty(layoutName))
                return;
            string treeId = GetTreeIndentifier();
            if (string.IsNullOrEmpty(treeId))
                return;
            if (m_Layout == null)
                return;
            m_Layout.SaveLayout(layoutName, treeId, m_Root);
        }

        /// <summary>
        /// 使用布局
        /// </summary>
        /// <param name="layout"></param>
        private void OnUseLayout(System.Object layout)
        {
            if (layout == null)
                return;

            string layoutName = (string) layout;
            UseLayout(layoutName);
        }

        /// <summary>
        /// 使用默认布局
        /// </summary>
        private void UseDefaultLayout()
        {
            List<SubWindow> alreadyOpen = new List<SubWindow>();
            for (int i = 0; i < m_SubWindowList.Count; i++)
            {
                if (m_SubWindowList[i].DefaultOpen)
                {
                    alreadyOpen.Add(m_SubWindowList[i]);
                    m_SubWindowList[i].Close();
                }
            }
            for (int i = 0; i < alreadyOpen.Count; i++)
            {
                this.InsertWindow(alreadyOpen[i]);
            }
        }

        private void OnSaveLayout()
        {
            SubWindowTreeLayoutWizard.CreateWizard(m_Layout, GetTreeIndentifier(), m_Root);
        }

        private void OnDeleteLayout()
        {
            if (m_Layout != null)
                SubWindowTreeDeleteLayoutWizard.CreateWizard(m_Layout);
        }

        private void OnRevertLayout()
        {
            if (m_Layout != null)
                m_Layout.RevertLayout();
        }

        private string GetTreeIndentifier()
        {
            if (m_SubWindowList != null)
            {
                string text = "";
                List<string> idlist = new List<string>();
                foreach (var window in m_SubWindowList)
                {
                    idlist.Add(window.GetIndentifier());
                }
                idlist.Sort();
                for (int i = 0; i < idlist.Count; i++)
                {
                    if (i != idlist.Count - 1)
                    {
                        text = text + idlist[i] + "_";
                    }
                    else
                    {
                        text = text + idlist[i];
                    }
                }
                return text;
            }
            return null;
        }
    }
}