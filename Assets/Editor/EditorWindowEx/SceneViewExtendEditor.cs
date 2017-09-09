using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;
using System.Reflection;
using System.Collections;

public class SceneViewExtendEditor
{
    private static SceneViewExtendEditor instance;

    protected UnityEngine.SceneManagement.Scene currentScene;
    protected ToolBarTree toolBarTree;
    protected SceneViewMenu sceneViewMenu;
    protected EditorWindowMsgBox msgBox;

    private bool m_UseGlobalMethod;

    public SceneViewExtendEditor()
    {
        currentScene = EditorSceneManager.GetActiveScene();
    }

    public static T CreateToolBar<T>(bool useGlobalMethod = false, params System.Object[] args) where T : SceneViewExtendEditor
    {
        if (instance != null)
        {
            if (instance.GetType() == typeof (T))
                return (T)instance;
            else
            {
                CloseToolBar();
                instance = null;
            }
        }
        instance = (T) System.Activator.CreateInstance(typeof (T), args);
        instance.m_UseGlobalMethod = useGlobalMethod;
        instance.Init();
        var func = new SceneView.OnSceneFunc(instance.OnSceneGUI);
        if (SceneView.onSceneGUIDelegate != null)
        {
            var delegatelist = SceneView.onSceneGUIDelegate.GetInvocationList();
            if (!delegatelist.Contains(func))
            {
                SceneView.onSceneGUIDelegate += func;
            }
        }
        else
        {
            SceneView.onSceneGUIDelegate += func;
        }
        return (T)instance;
    }

    protected virtual void Init()
    {
        toolBarTree = new ToolBarTree();
        sceneViewMenu = new SceneViewMenu();
        msgBox = new EditorWindowMsgBox();
        EditorWindowToolsInitializer.InitTools(new System.Type[] { GetType() }, new object[] { this }, m_UseGlobalMethod, toolBarTree, sceneViewMenu, msgBox);
    }

    public static void CloseToolBar()
    {
        if (instance == null)
            return;
        instance.ClosePanel();
        var func = new SceneView.OnSceneFunc(instance.OnSceneGUI);
        if (SceneView.onSceneGUIDelegate == null)
            return;
        var delegatelist = SceneView.onSceneGUIDelegate.GetInvocationList();
        if (delegatelist.Contains(func))
        {
            SceneView.onSceneGUIDelegate -= func;
        }

        instance = null;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (CheckSceneChange())
        {
            CloseToolBar();
            return;
        }
        PreDrawScene(sceneView);

        Handles.BeginGUI();
        Rect rect = sceneView.position;
        bool guienable = GUI.enabled;
        if (instance != null && instance.msgBox != null && instance.msgBox.IsShowing)
            GUI.enabled = false;
        else
            GUI.enabled = true;
        DrawToolBarGUI(new Rect(0, 0, rect.width, 18));
        DrawMenuGUI(new Rect(0, 0, rect.width, rect.height));
        GUI.enabled = guienable;
        DrawMsgBoxGUI(new Rect(0, 0, rect.width, rect.height));

        DrawGUI(new Rect(0, 0, rect.width, rect.height));
        Handles.EndGUI();

        PostDrawScene(sceneView);

        sceneView.Repaint();
    }

    protected virtual void ClosePanel()
    {
        
    }

    private static bool CheckSceneChange()
    {
        if (instance == null)
            return true;
        if (instance.currentScene == null)
            return true;
        var scene = EditorSceneManager.GetActiveScene();
        if (instance.currentScene != scene)
            return true;
        return false;
    }

    #region GUI

    private static void DrawToolBarGUI(Rect rect)
    {
        if (instance == null)
            return;
        GUI.Box(rect, "", GUIStyleCache.GetStyle("Toolbar"));
        GUILayout.BeginArea(new Rect(rect.x + 10, rect.y, rect.width - 100, rect.height));
        GUILayout.BeginHorizontal();
        if (instance.toolBarTree != null)
        {
            instance.toolBarTree.DrawToolbar();
        }
        instance.OnDrawToolBar();
        if (instance.sceneViewMenu != null)
        {
            instance.sceneViewMenu.DrawToolBar();
        }
        if (ToolBarButton(70, "关闭"))
        {
            CloseToolBar();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private static void DrawMenuGUI(Rect rect)
    {
        if (instance == null)
            return;
        if (instance.sceneViewMenu != null)
            instance.sceneViewMenu.DrawMenu(rect);
    }

    private static void DrawMsgBoxGUI(Rect rect)
    {
        if (instance == null)
            return;
        if (instance.msgBox != null)
            instance.msgBox.DrawMsgBox(rect);
    }

    protected static Rect GetToolBarRect(float width)
    {
        return EditorGUILayout.GetControlRect(GUILayout.Width(width), GUILayout.Height(17));
    }

    protected static bool ToolBarButton(float width, string text)
    {
        Rect rect = GetToolBarRect(width);
        return GUI.Button(rect, text, GUIStyleCache.GetStyle("toolbarbutton"));
    }

    protected static bool ToolBarToggle(float width, string text, bool value)
    {
        Rect rect = GetToolBarRect(width);
        return GUI.Toggle(rect, value, text, GUIStyleCache.GetStyle("toolbarbutton"));
    }

    private static void DrawGUI(Rect rect)
    {
        if (instance == null)
            return;
        if (instance.msgBox != null && instance.msgBox.IsShowing)
            return;
        instance.OnDrawGUI(rect);
    }

    protected virtual void OnDrawGUI(Rect rect)
    {
        
    }

    protected virtual void OnDrawToolBar()
    {
        
    }
    #endregion

    #region SceneGUI

    private static void PreDrawScene(SceneView sceneView)
    {
        if (instance == null)
            return;
        if (instance.msgBox != null && instance.msgBox.IsShowing)
            return;
        instance.OnPreDrawSceneGUI(sceneView);
    }

    private static void PostDrawScene(SceneView sceneView)
    {
        if (instance == null)
            return;
        if (instance.msgBox != null && instance.msgBox.IsShowing)
            return;
        instance.OnPostDrawSceneGUI(sceneView);
    }

    protected virtual void OnPreDrawSceneGUI(SceneView sceneView)
    {
        
    }

    protected virtual void OnPostDrawSceneGUI(SceneView sceneView)
    {
        
    }
    #endregion

    public void ShowMsgBox(int id, System.Object obj)
    {
        if (msgBox != null)
            msgBox.ShowMsgBox(id, obj);
    }

    public void HideMsgBox()
    {
        if (msgBox != null)
            msgBox.HideMsgBox();
    }
    
}
