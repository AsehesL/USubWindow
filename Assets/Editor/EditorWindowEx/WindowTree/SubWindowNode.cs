using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Collections.Generic;

namespace EditorWinEx.Internal
{
    /// <summary>
    /// 子窗口节点
    /// </summary>
    internal class SubWindowNode
    {
        /// <summary>
        /// 孩子数量
        /// </summary>
        public virtual int Count
        {
            get { return m_Childs.Count; }
        }

        public int Depth
        {
            get { return depth; }
        }

        public bool IsHorizontal
        {
            get { return isHorizontal; }
        }

        /// <summary>
        /// 最大深度
        /// </summary>
        protected const int kMaxNodeDepth = 4;

        /// <summary>
        /// 孩子节点
        /// </summary>
        private List<SubWindowNode> m_Childs = new List<SubWindowNode>();

        /// <summary>
        /// 权重-（所有兄弟节点的权重和应为1）
        /// </summary>
        public float weight = 1;

        /// <summary>
        /// 深度
        /// </summary>
        protected int depth;

        /// <summary>
        /// 是否水平方向绘制-（父节点与子节点的绘制方向应相反）
        /// </summary>
        protected bool isHorizontal;

        protected Rect rect;

        protected GUITweenParam tweenParam;

        //private Rect m_OriginRect;
        //private float m_RectTweenTime;

        private bool m_IsDragging;
        private int m_CurrentResizeFirstId;
        private int m_CurrentResizeSecondId;

        private const float kMaxWeight = 0.8f;
        private const float kMinWeight = 0.2f;

        public SubWindowNode(bool horizontal, int depth)
        {
            this.isHorizontal = horizontal;
            this.depth = depth;
        }

        /// <summary>
        /// 绘制GUI
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="repaintAction"></param>
        public virtual void DrawGUI(Rect rect, System.Action repaintAction)
        {
            float offset = 0;
            //Rect resizeRect = default(Rect);
            if (isHorizontal)
            {
                for (int i = 0; i < m_Childs.Count; i++)
                {
                    if (i > 0)
                    {
                        Resize(i - 1, i, new Rect(rect.x + offset - 2, rect.y, 4, rect.height));
                    }
                    int w = (int) (rect.width*m_Childs[i].weight);
                    m_Childs[i].DrawGUI(new Rect(rect.x + offset, rect.y, w, rect.height),
                        repaintAction);
                    if (i >= 0 && i < m_Childs.Count)
                        offset += w;
                }
            }
            else
            {
                for (int i = 0; i < m_Childs.Count; i++)
                {
                    if (i > 0)
                    {
                        Resize(i - 1, i, new Rect(rect.x, rect.y + offset - 2, rect.width, 4));
                    }
                    int h = (int) (rect.height*m_Childs[i].weight);
                    m_Childs[i].DrawGUI(new Rect(rect.x, rect.y + offset, rect.width, h),
                        repaintAction);
                    if (i >= 0 && i < m_Childs.Count)
                        offset += h;
                }
            }
            DoResize(repaintAction);
            this.rect = rect;
        }

        /// <summary>
        /// 拖拽
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public virtual SubWindow DragWindow(Vector2 position)
        {
            for (int i = 0; i < m_Childs.Count; i++)
            {
                var r = m_Childs[i].DragWindow(position);
                if (r != null)
                    return r;
            }
            return null;
        }

        /// <summary>
        /// 放置
        /// </summary>
        /// <param name="position"></param>
        /// <param name="depth"></param>
        /// <param name="window"></param>
        /// <param name="preDropAction"></param>
        /// <param name="postDropAction"></param>
        public void DropWindow(Vector2 position, int depth, SubWindow window, System.Action<SubWindow> preDropAction,
            System.Action postDropAction)
        {
            this.TriggerAnchorArea(position, depth, window, preDropAction, postDropAction);
        }

        /// <summary>
        /// 绘制锚点区域
        /// </summary>
        /// <param name="position"></param>
        /// <param name="depth"></param>
        /// <param name="window"></param>
        public void DrawAnchorArea(Vector2 position, int depth, SubWindow window)
        {
            this.TriggerAnchorArea(position, depth, window, null, null);
        }

        /// <summary>
        /// 是否包含子窗口
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public virtual bool ContainWindow(SubWindow window)
        {
            if (m_Childs.Count == 1)
            {
                return m_Childs[0].ContainWindow(window);
            }
            return false;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index"></param>
        public virtual void Insert(SubWindowNode node, int index)
        {
            if (this.m_Childs.Count == 0)
            {
                node.weight = 1;
                node.depth = depth + 1;
                node.isHorizontal = !isHorizontal;
                this.m_Childs.Add(node);
                return;
            }
            float w = 1.0f/(this.m_Childs.Count + 1);
            float ew = w/this.m_Childs.Count;
            node.weight = w;
            node.depth = depth + 1;
            node.isHorizontal = !isHorizontal;
            for (int i = 0; i < this.m_Childs.Count; i++)
            {
                this.m_Childs[i].weight -= ew;
            }
            if (index < 0 || index >= m_Childs.Count)
                m_Childs.Add(node);
            else
                m_Childs.Insert(index, node);
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public virtual bool RemoveWindow(SubWindow window)
        {
            for (int i = 0; i < m_Childs.Count; i++)
            {
                bool result = m_Childs[i].RemoveWindow(window);
                if (result)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 清除空节点
        /// </summary>
        public virtual void ClearEmptyNode()
        {
            for (int i = 0; i < m_Childs.Count; i++)
            {
                m_Childs[i].ClearEmptyNode();
                if (m_Childs[i].Count == 0)
                {
                    float w = 1.0f/this.m_Childs.Count;
                    m_Childs.RemoveAt(i);
                    float ew = w/this.m_Childs.Count;
                    for (int j = 0; j < this.m_Childs.Count; j++)
                    {
                        this.m_Childs[j].weight += ew;
                    }
                }
            }
        }

        /// <summary>
        /// 添加子窗口
        /// </summary>
        /// <param name="window"></param>
        /// <param name="index"></param>
        public virtual void AddWindow(SubWindow window, int index)
        {
            SubWindowLeaf item = new SubWindowLeaf(window, !isHorizontal, depth + 1);
            this.Insert(item, index);
        }

        /// <summary>
        /// 重新计算各参数
        /// </summary>
        /// <param name="depth">重新设置的深度</param>
        /// <param name="isHorizontal">重新设置的方向</param>
        public virtual void Recalculate(int depth, bool isHorizontal)
        {
            this.depth = depth;
            this.isHorizontal = isHorizontal;
            if (m_Childs.Count == 0)
            {
                return;
            }
            float weightSum = 0;
            for (int i = 0; i < m_Childs.Count; i++)
            {
                if (m_Childs[i].weight < kMinWeight)
                {
                    m_Childs[i].weight = kMinWeight;
                }
                else if (m_Childs[i].weight > kMaxWeight)
                {
                    m_Childs[i].weight = kMaxWeight;
                }
                weightSum += m_Childs[i].weight;
                m_Childs[i].Recalculate(depth + 1, !isHorizontal);
            }
            if (weightSum > 1.0f + Mathf.Epsilon || weightSum < 1.0f - Mathf.Epsilon)
            {
                float m = (1 - weightSum)/m_Childs.Count;
                for (int i = 0; i < m_Childs.Count; i++)
                {
                    m_Childs[i].weight += m;
                }
            }
        }

        /// <summary>
        /// 将当前节点信息写入布局
        /// </summary>
        /// <param name="element"></param>
        /// <param name="document"></param>
        /// <param name="index"></param>
        public virtual void WriteToLayoutCfg(XmlElement element, XmlDocument document, int index)
        {
            XmlElement currentElement = document.CreateElement("SubWindowNode");
            currentElement.SetAttribute("Weight", weight.ToString());
            currentElement.SetAttribute("Depth", depth.ToString());
            currentElement.SetAttribute("Horizontal", isHorizontal.ToString());
            currentElement.SetAttribute("Index", index.ToString());
            element.AppendChild(currentElement);
            if (m_Childs != null)
            {
                for (int i = 0; i < m_Childs.Count; i++)
                {
                    m_Childs[i].WriteToLayoutCfg(currentElement, document, i);
                }
            }
        }

        /// <summary>
        /// 从布局信息构造节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="windowList"></param>
        /// <param name="onWindowClose"></param>
        public virtual void CreateFromLayoutCfg(XmlElement node, List<SubWindow> windowList,
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
                if (n.Name == "SubWindowNode")
                {
                    SubWindowNode swnode = new SubWindowNode(true, 0);
                    swnode.CreateFromLayoutCfg(n, windowList, onWindowClose);
                    this.m_Childs.Add(swnode);
                }
                else if (n.Name == "SubWindowLeaf")
                {
                    SubWindowLeaf swleaf = new SubWindowLeaf(null, true, 0);
                    swleaf.CreateFromLayoutCfg(n, windowList, onWindowClose);
                    this.m_Childs.Add(swleaf);
                }
            }
        }

        /// <summary>
        /// 触发锚点区域
        /// </summary>
        /// <param name="position"></param>
        /// <param name="depth"></param>
        /// <param name="window"></param>
        /// <param name="preDropAction"></param>
        /// <param name="postDropAction"></param>
        /// <returns></returns>
        protected virtual bool TriggerAnchorArea(Vector2 position, int depth, SubWindow window,
            System.Action<SubWindow> preDropAction, System.Action postDropAction)
        {
            if (depth >= kMaxNodeDepth)
                return false;
            Rect r = default(Rect);
            float offset = 0;
            for (int i = 0; i < m_Childs.Count; i++)
            {
                if (m_Childs[i].TriggerAnchorArea(position, depth + 1, window, preDropAction, postDropAction))
                    return true;
                if (isHorizontal)
                {
                    r = new Rect(rect.x + offset, rect.y, rect.width*m_Childs[i].weight*0.2f, rect.height);
                    if (r.Contains(position))
                    {
                        if (preDropAction == null)
                            tweenParam = GUIEx.ScaleTweenBox(r, tweenParam, string.Empty,
                                GUIStyleCache.GetStyle("SelectionRect"));
                        else
                        {
                            this.DropWindow(window, preDropAction, postDropAction, true, i);
                        }
                        return true;
                    }
                    r = new Rect(rect.x + offset + rect.width*m_Childs[i].weight*0.8f, rect.y,
                        rect.width*m_Childs[i].weight*0.2f, rect.height);
                    if (r.Contains(position))
                    {
                        if (preDropAction == null)
                            tweenParam = GUIEx.ScaleTweenBox(r, tweenParam, string.Empty,
                                GUIStyleCache.GetStyle("SelectionRect"));
                        else
                        {
                            this.DropWindow(window, preDropAction, postDropAction, true, i + 1);
                        }
                        return true;
                    }
                    r = new Rect(rect.x + offset, rect.y + rect.height*0.8f, rect.width*m_Childs[i].weight,
                        rect.height*0.2f);
                    if (r.Contains(position) && m_Childs.Count > 1)
                    {
                        if (preDropAction == null)
                            tweenParam = GUIEx.ScaleTweenBox(r, tweenParam, string.Empty,
                                GUIStyleCache.GetStyle("SelectionRect"));
                        else
                        {
                            this.DropWindow(window, preDropAction, postDropAction, false, i);
                        }
                        return true;
                    }
                    offset += m_Childs[i].weight*rect.width;
                }
                else
                {
                    r = new Rect(rect.x, rect.y + offset, rect.width, rect.height*m_Childs[i].weight*0.2f);
                    if (r.Contains(position))
                    {
                        if (preDropAction == null)
                            tweenParam = GUIEx.ScaleTweenBox(r, tweenParam, string.Empty,
                                GUIStyleCache.GetStyle("SelectionRect"));
                        else
                        {
                            this.DropWindow(window, preDropAction, postDropAction, true, i);
                        }
                        return true;
                    }
                    r = new Rect(rect.x, rect.y + offset + rect.height*m_Childs[i].weight*0.8f, rect.width,
                        rect.height*m_Childs[i].weight*0.2f);
                    if (r.Contains(position))
                    {
                        if (preDropAction == null)
                            tweenParam = GUIEx.ScaleTweenBox(r, tweenParam, string.Empty,
                                GUIStyleCache.GetStyle("SelectionRect"));
                        else
                        {
                            this.DropWindow(window, preDropAction, postDropAction, true, i + 1);
                        }
                        return true;
                    }
                    r = new Rect(rect.x + rect.width*0.8f, rect.y + offset, rect.width*0.2f,
                        rect.height*m_Childs[i].weight);
                    if (r.Contains(position) && m_Childs.Count > 1)
                    {
                        if (preDropAction == null)
                            tweenParam = GUIEx.ScaleTweenBox(r, tweenParam, string.Empty,
                                GUIStyleCache.GetStyle("SelectionRect"));
                        else
                        {
                            this.DropWindow(window, preDropAction, postDropAction, false, i);
                        }
                        return true;
                    }
                    offset += m_Childs[i].weight*rect.height;
                }
            }
            return false;
        }

        private void DropWindow(SubWindow window, System.Action<SubWindow> preDropAction, System.Action postDropAction,
            bool betweenChilds, int dropIndex)
        {
            if (window == null)
                return;
            if (preDropAction == null)
                return;
            if (betweenChilds)
            {
                if (preDropAction != null)
                {

                    preDropAction(window);
                    this.AddWindow(window, dropIndex);

                    postDropAction();
                }
            }
            else
            {
                if (preDropAction != null && dropIndex >= 0 && dropIndex < m_Childs.Count)
                {
                    SubWindowNode child = m_Childs[dropIndex];
                    if (child.ContainWindow(window))
                        return;
                    preDropAction(window);
                    m_Childs.RemoveAt(dropIndex);
                    SubWindowNode node = new SubWindowNode(!isHorizontal, depth + 1);
                    node.weight = child.weight;
                    child.isHorizontal = isHorizontal;
                    child.depth = node.depth + 1;
                    node.Insert(child, 0);

                    this.Insert(node, dropIndex);
                    node.AddWindow(window, -1);
                    postDropAction();
                }
            }
        }

        private void Resize(int first, int second, Rect rect)
        {
            if (isHorizontal)
                EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeHorizontal);
            else
                EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeVertical);
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                if (rect.Contains(Event.current.mousePosition))
                {
                    Event.current.Use();
                    m_IsDragging = true;
                    m_CurrentResizeFirstId = first;
                    m_CurrentResizeSecondId = second;
                }
            }
        }

        private void DoResize(System.Action repaintAct)
        {
            if (m_IsDragging)
            {
                if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
                {
                    m_IsDragging = false;
                    Event.current.Use();
                }
                if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
                {
                    float delta = 0;
                    if (isHorizontal)
                        delta = (Event.current.delta.x)/this.rect.width;
                    else
                    {
                        delta = (Event.current.delta.y)/this.rect.height;
                    }
                    float addW = m_Childs[m_CurrentResizeFirstId].weight + delta;
                    float musW = m_Childs[m_CurrentResizeSecondId].weight - delta;
                    if (addW >= kMinWeight && addW <= kMaxWeight && musW >= kMinWeight && musW <= kMaxWeight)
                    {
                        m_Childs[m_CurrentResizeFirstId].weight = addW;
                        m_Childs[m_CurrentResizeSecondId].weight = musW;
                    }
                    if (repaintAct != null)
                        repaintAct();
                }
            }
        }
    }
}