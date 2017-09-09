using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 布局存储辅助窗口
/// </summary>
internal class SubWindowTreeLayoutWizard : ScriptableWizard
{

    public string layoutname = "";

    private SubWindowLayout m_Layout;
    private string m_TreeId;
    private SubWindowNode m_RootNode;

    public static void CreateWizard(SubWindowLayout layout, string treeId, SubWindowNode rootNode)
    {
        SubWindowTreeLayoutWizard wizard = ScriptableWizard.DisplayWizard<SubWindowTreeLayoutWizard>("Save SubWindow Layout", "Save");
        wizard.maxSize = new Vector2(300, 150);
        wizard.minSize = new Vector2(300, 150);
        wizard.m_Layout = layout;
        wizard.m_TreeId = treeId;
        wizard.m_RootNode = rootNode;
    }

    void OnWizardCreate()
    {
        if (layoutname == "Default")
            return;
        if (m_Layout != null)
            m_Layout.SaveLayout(layoutname, m_TreeId, m_RootNode);
        m_Layout = null;
        m_RootNode = null;
        m_TreeId = null;
    }

    void OnWizardUpdate()
    {
        helpString = "输入Layout名称";
    }
}

/// <summary>
/// 布局删除辅助窗口
/// </summary>
internal class SubWindowTreeDeleteLayoutWizard : ScriptableWizard
{
    private SubWindowLayout m_Layout;
    //private List<string> m_Layouts;

    public static void CreateWizard(SubWindowLayout layout)
    {
        SubWindowTreeDeleteLayoutWizard wizard = ScriptableWizard.DisplayWizard<SubWindowTreeDeleteLayoutWizard>("Delete SubWindow Layout");
        wizard.m_Layout = layout;
        //wizard.m_Layouts = layout;
    }

    void OnGUI()
    {
        if (m_Layout != null && m_Layout.Layouts != null)
        {
            for (int i = 0; i < m_Layout.Layouts.Count; i++)
            {
                if (GUILayout.Button(m_Layout.Layouts[i]))
                {
                    //if (m_Tree != null)
                    //{
                        m_Layout.DeleteLayout(m_Layout.Layouts[i]);
                        break;
                    //}
                }
            }
        }
    }
}
