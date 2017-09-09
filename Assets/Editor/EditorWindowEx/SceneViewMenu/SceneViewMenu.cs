using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

public class SceneViewMenu : EditorWindowTool
{
    private List<SceneViewMenuItem> m_MenuItems = new List<SceneViewMenuItem>();

    private SceneViewMenuItem m_CurrentItem;

    public void DrawToolBar()
    {
        Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(70), GUILayout.Height(17));
        if (GUIEx.ToolbarButton(rect, "视图"))
        {
            ClickDropDown(rect);
        }
    }

    public void AddItem(string menu, MethodInfo method, System.Object target)
    {
        SceneViewMenuItem item = new SceneViewMenuItem(menu, method, target);
        this.m_MenuItems.Add(item);
    }

    public void DrawMenu(Rect rect)
    {
        if (m_CurrentItem != null)
            m_CurrentItem.Draw(rect);
    }

    protected override void OnRegisterMethod(MethodInfo method, System.Object target, bool isStatic)
    {
        var atts = method.GetCustomAttributes(typeof(SceneViewMenuAttribute), false);
        var parameters = method.GetParameters();
        if (atts != null && parameters.Length == 1 && parameters[0].ParameterType == typeof(Rect))
        {
            for (int j = 0; j < atts.Length; j++)
            {
                SceneViewMenuAttribute att = (SceneViewMenuAttribute)atts[j];
                AddItem(att.menu, method, target);
            }
        }
    }

    private void ClickDropDown(Rect rect)
    {
        GenericMenu menu = new GenericMenu();
        for (int i = 0; i < m_MenuItems.Count; i++)
        {
            menu.AddItem(new GUIContent(m_MenuItems[i].Menu), m_CurrentItem == m_MenuItems[i], SelectMenuItem,
                m_MenuItems[i]);
        }
        menu.DropDown(rect);
    }

    private void SelectMenuItem(System.Object item)
    {
        if (item == null)
            return;
        SceneViewMenuItem it = (SceneViewMenuItem)item;
        if (m_CurrentItem == it)
            m_CurrentItem = null;
        else
            m_CurrentItem = it;
    }
}
