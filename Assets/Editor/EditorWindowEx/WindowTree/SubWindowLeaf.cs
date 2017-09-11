using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace EditorWinEx.Internal
{
    /// <summary>
    /// 子窗口叶子节点
    /// </summary>
    internal class SubWindowLeaf : SubWindowNode
    {

        /// <summary>
        /// 子窗口数量
        /// </summary>
        public override int Count
        {
            get { return m_SubWindows.Count; }
        }

        /// <summary>
        /// 最大子窗口树
        /// </summary>
        private const int kMaxSubWindowCount = 3;

        /// <summary>
        /// 子窗口列表
        /// </summary>
        private List<SubWindow> m_SubWindows = new List<SubWindow>();

        /// <summary>
        /// 当前选中的子窗口
        /// </summary>
        private int m_SelectSubWindow;

        private GUITweenParam m_TweenParam;


        public SubWindowLeaf(SubWindow window, bool isHorizontal, int depth) : base(isHorizontal, depth)
        {
            if (window != null)
            {
                window.isOpen = true;
                m_SubWindows.Add(window);
            }
            m_TweenParam = new GUITweenParam(true);
        }

        public override void DrawGUI(Rect rect, System.Action repaintAction)
        {
            rect = new Rect(rect.x + 2, rect.y, rect.width - 4, rect.height - 2);


            if (m_TweenParam.isTweening)
            {
                m_TweenParam = GUIEx.ScaleTweenBox(rect, m_TweenParam, m_SubWindows[m_SelectSubWindow].Title,
                    GUIStyleCache.GetStyle("dragtabdropwindow"));
                if (repaintAction != null)
                    repaintAction();
                return;
            }

            GUI.BeginGroup(rect, GUIStyleCache.GetStyle("WindowBackground"));
            GUI.Box(new Rect(0, 0, rect.width, 18), string.Empty, GUIStyleCache.GetStyle("dockarea"));

            if (m_SelectSubWindow >= 0 && m_SelectSubWindow < m_SubWindows.Count)
            {
                GUI.Label(new Rect(m_SelectSubWindow*110, 0, rect.width - m_SelectSubWindow*110, 18),
                    m_SubWindows[m_SelectSubWindow].Title, GUIStyleCache.GetStyle("dragtabdropwindow"));
            }
            for (int i = 0; i < m_SubWindows.Count; i++)
            {
                if (m_SelectSubWindow != i)
                {
                    if (GUI.Button(new Rect(i*110, 0, 110, 17), m_SubWindows[i].Title, GUIStyleCache.GetStyle("dragtab")))
                    {
                        m_SelectSubWindow = i;
                    }
                }
            }
            GUI.Box(new Rect(0, 18, rect.width, rect.height - 18), string.Empty, GUIStyleCache.GetStyle("hostview"));
            if (m_SelectSubWindow >= 0 && m_SelectSubWindow < m_SubWindows.Count)
            {
                GUI.BeginGroup(new Rect(0, 18, rect.width, rect.height - 18));
                m_SubWindows[m_SelectSubWindow].DrawSubWindow(new Rect(0, 0, rect.width, rect.height - 18));
                GUI.EndGroup();
                if (repaintAction != null)
                    repaintAction();
            }

            if (GUI.Button(new Rect(rect.width - 21, 4, 13, 11), string.Empty, GUIStyleCache.GetStyle("WinBtnClose")))
            {
                if (m_SelectSubWindow >= 0 && m_SelectSubWindow < m_SubWindows.Count)
                {
                    m_SubWindows[m_SelectSubWindow].Close();
                }
            }

            GUI.EndGroup();
            this.rect = rect;
        }

        /// <summary>
        /// 是否包含窗口
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public override bool ContainWindow(SubWindow window)
        {
            return m_SubWindows.Contains(window);
        }

        /// <summary>
        /// 添加窗口
        /// </summary>
        /// <param name="window"></param>
        /// <param name="index"></param>
        public override void AddWindow(SubWindow window, int index)
        {
            this.m_SubWindows.Add(window);
            m_SelectSubWindow = this.m_SubWindows.Count - 1;
        }

        public override void Insert(SubWindowNode node, int index)
        {
        }

        /// <summary>
        /// 移除窗口
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public override bool RemoveWindow(SubWindow window)
        {
            for (int i = 0; i < m_SubWindows.Count; i++)
            {
                if (m_SubWindows[i] == window)
                {
                    m_SubWindows.Remove(window);
                    m_SelectSubWindow = 0;
                    return true;
                }
            }
            return false;
        }

        public override void ClearEmptyNode()
        {
        }

        public override void WriteToLayoutCfg(XmlElement element, XmlDocument document, int index)
        {
            XmlElement currentElement = document.CreateElement("SubWindowLeaf");
            currentElement.SetAttribute("Weight", weight.ToString());
            currentElement.SetAttribute("Depth", depth.ToString());
            currentElement.SetAttribute("Horizontal", isHorizontal.ToString());
            currentElement.SetAttribute("Index", index.ToString());
            element.AppendChild(currentElement);
            if (m_SubWindows != null)
            {
                for (int i = 0; i < m_SubWindows.Count; i++)
                {
                    XmlElement windowElement = document.CreateElement("SubWindow");
                    windowElement.SetAttribute("ID", m_SubWindows[i].GetIndentifier());
                    windowElement.SetAttribute("Index", i.ToString());
                    currentElement.AppendChild(windowElement);
                }
            }
        }

        public override void CreateFromLayoutCfg(XmlElement node, List<SubWindow> windowList,
            System.Action<SubWindow> onWindowClose)
        {
            string weightStr = node.GetAttribute("Weight");
            string depthStr = node.GetAttribute("Depth");
            string horizontalStr = node.GetAttribute("Horizontal");
            weight = float.Parse(weightStr);
            depth = int.Parse(depthStr);
            isHorizontal = bool.Parse(horizontalStr);
            XmlNodeList nodes = node.ChildNodes;
            if (nodes.Count == 0)
                return;
            XmlElement[] sortnode = new XmlElement[nodes.Count];
            foreach (var n in nodes)
            {
                XmlElement nd = (XmlElement) n;
                string indexstr = nd.GetAttribute("Index");
                int index = int.Parse(indexstr);
                sortnode[index] = nd;
            }
            foreach (var n in sortnode)
            {
                string id = n.GetAttribute("ID");
                var window = windowList.Find(w => w.GetIndentifier() == id);
                if (window == null)
                    continue;
                window.isOpen = true;
                window.AddCloseEventListener(onWindowClose);
                m_SubWindows.Add(window);
            }
        }

        public override SubWindow DragWindow(Vector2 position)
        {
            if (m_TweenParam.isTweening)
                return null;
            if (m_SelectSubWindow < 0 || m_SelectSubWindow >= m_SubWindows.Count)
                return null;
            Rect rect = new Rect(this.rect.x + m_SelectSubWindow*100, this.rect.y, 100, 17);
            if (rect.Contains(position))
            {
                return m_SubWindows[m_SelectSubWindow];
            }
            return null;
        }

        public override void Recalculate(int depth, bool isHorizontal)
        {
            this.depth = depth;
            this.isHorizontal = isHorizontal;
        }

        protected override bool TriggerAnchorArea(Vector2 position, int depth, SubWindow window,
            System.Action<SubWindow> preDropAction, System.Action postDropAction)
        {
            if (m_TweenParam.isTweening)
                return false;
            if (depth >= kMaxNodeDepth)
                return false;
            if (m_SubWindows.Count >= kMaxSubWindowCount)
                return false;
            if (this.m_SubWindows.Contains(window))
                return false;
            Rect rect = new Rect(this.rect.x, this.rect.y, this.rect.width, 17);
            if (rect.Contains(position))
            {
                if (preDropAction == null)
                {
                    tweenParam = GUIEx.ScaleTweenBox(rect, tweenParam, string.Empty,
                        GUIStyleCache.GetStyle("SelectionRect"));
                }
                else
                {
                    if (preDropAction != null)
                    {
                        preDropAction(window);
                        this.AddWindow(window, 0);
                        postDropAction();
                    }
                }
                return true;
            }
            return false;
        }

    }
}