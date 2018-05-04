using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace EditorWinEx.Internal
{
    /// <summary>
    /// SubWindow布局
    /// </summary>
    internal class SubWindowLayout
    {
        public List<string> Layouts
        {
            get { return m_Layouts; }
        }

        private string m_WindowName;
        private string m_HandleName;

        //private string m_LayoutPrefsKey;

        private List<string> m_Layouts = new List<string>();

        public SubWindowLayout(string windowName, string handleName)
        {
            this.m_WindowName = windowName;
            this.m_HandleName = handleName;

            //m_LayoutPrefsKey = MDIEditorWindow.GetIndentifier() + "_" + "SubWindowTree_" + m_WindowName;

            //if (!string.IsNullOrEmpty(m_HandleName))
            //    m_LayoutPrefsKey = m_LayoutPrefsKey + "_" + m_HandleName;
            LoadLayoutCfgs();
        }

        public void SaveLayout(string layoutName, string treeId, SubWindowNode node)
        {
            if (string.IsNullOrEmpty(layoutName))
                return;
            if (string.IsNullOrEmpty(treeId))
                return;
            XmlDocument doc = new XmlDocument();

            XmlElement root = doc.CreateElement("SubWindowTree");
            root.SetAttribute("Layout", layoutName);
            root.SetAttribute("TreeID", treeId.GetHashCode().ToString());

            doc.AppendChild(root);

            if (node != null)
            {
                node.WriteToLayoutCfg(root, doc, 0);
            }
            bool isCurrent = layoutName == "Current";
            string path = GetLayoutCfgsPath(isCurrent);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            doc.Save(path + "/" + layoutName + ".xml");
            LoadLayoutCfgs();
        }

        public void DeleteLayout(string layoutName)
        {
            if (RemoveLayoutCfg(layoutName))
                LoadLayoutCfgs();
        }

        public XmlElement UseLayout(string layoutName, string treeId)
        {
            if (string.IsNullOrEmpty(layoutName))
                return null;
            if (string.IsNullOrEmpty(treeId))
                return null;
            bool isCurrent = layoutName == "Current";
            string path = Path.Combine(GetLayoutCfgsPath(isCurrent), layoutName + ".xml");
            if (File.Exists(path))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNodeList nodes = doc.ChildNodes;
                if (nodes.Count == 1)
                {
                    XmlElement tree = (XmlElement) nodes[0];
                    if (!tree.HasAttribute("TreeID"))
                    {
                        if (EditorUtility.DisplayDialog("错误", "该布局格式错误，是否删除？", "是", "否"))
                        {
                            DeleteLayout(layoutName);
                        }
                        return null;
                    }
                    string layoutTreeId = tree.GetAttribute("TreeID");
                    int layoutTreeIDValue = int.Parse(layoutTreeId);
                    int treeIdValue = treeId.GetHashCode();
                    if (layoutTreeIDValue != treeIdValue)
                    {
                        if (EditorUtility.DisplayDialog("错误", "该布局与当前的窗体结构存在差异，无法继续使用，是否删除？", "是", "否"))
                        {
                            DeleteLayout(layoutName);
                        }
                        return null;
                    }
                    XmlNodeList treenode = tree.ChildNodes;
                    if (treenode.Count == 1)
                    {
                        XmlElement subwindowroot = (XmlElement) treenode[0];

                        return subwindowroot;
                    }
                }
            }
            return null;
        }

        public void RevertLayout()
        {
            if (m_Layouts != null && m_Layouts.Count > 0)
            {
                if (EditorUtility.DisplayDialog("删除所有Layout", "是否删除所有Layout文件", "是", "否"))
                {
                    foreach (var l in m_Layouts)
                    {
                        RemoveLayoutCfg(l);
                    }
                    LoadLayoutCfgs();
                    //SaveCurrentLayout(null);
                }
            }
        }

        private bool RemoveLayoutCfg(string layoutName)
        {
            if (m_Layouts != null && m_Layouts.Contains(layoutName))
            {
                string path = GetLayoutCfgsPath(false) + "/" + layoutName + ".xml";
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
            }
            return false;
        }

        private void LoadLayoutCfgs()
        {
            string path = GetLayoutCfgsPath(false);
            if (Directory.Exists(path))
            {
                m_Layouts.Clear();
                string[] files = Directory.GetFiles(path);
                for (int i = 0; i < files.Length; i++)
                {
                    string filename = Path.GetFileName(files[i]);
                    if (string.IsNullOrEmpty(filename))
                        continue;
                    string ext = Path.GetExtension(files[i]);
                    if (ext != null)
                        filename = filename.Replace(ext, string.Empty);
                    m_Layouts.Add(filename);
                }
            }
        }

        private string GetLayoutCfgsPath(bool isCurrent)
        {
            string rootPath = "SubWindowTree/" + m_WindowName;
            if (!string.IsNullOrEmpty(m_HandleName))
                rootPath = rootPath + "/" + m_HandleName;
            if (isCurrent)
                return Path.Combine(Application.temporaryCachePath, rootPath);
            else
                return rootPath;
        }


    }
}