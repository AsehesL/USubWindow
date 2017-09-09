using UnityEngine;
using UnityEditor;
using System.Collections;

public struct GUITweenParam
{
    public float tweenTime;

    public bool isTweening;

    private Rect m_Rect;

    public GUITweenParam(bool isTweening = true)
    {
        this.isTweening = isTweening;
        this.tweenTime = 0;
        this.m_Rect = default(Rect);
    }

    public bool CheckRect(Rect rect)
    {
        if (m_Rect != rect)
        {
            m_Rect = rect;
            return false;
        }
        return true;
    }
}

public class GUIEx
{

    public static bool ToolbarButton(Rect rect, string text)
    {
        return GUI.Button(rect, text, "toolbarbutton");
    }

    public static GUITweenParam ScaleTweenBox(Rect rect, GUITweenParam param, string text, GUIStyle style = null)
    {
        param = ScaleTweenInternal(ref rect, param);
        if (style != null)
            GUI.Box(rect, text, style);
        else
            GUI.Box(rect, text);
        return param;
    }

    public static GUITweenParam ScaleTweenBox(Rect rect, GUITweenParam param, GUIContent content, GUIStyle style = null)
    {
        param = ScaleTweenInternal(ref rect, param);
        if (style != null)
            GUI.Box(rect, content, style);
        else
            GUI.Box(rect, content);
        return param;
    }

    private static GUITweenParam ScaleTweenInternal(ref Rect rect, GUITweenParam param)
    {
        if (!param.CheckRect(rect))
        {
            param.tweenTime = 0;
        }

        param.tweenTime += 0.03f;
        //float scaleTweenTime = ((float)(EditorApplication.timeSinceStartup - param.tweenTime) / 0.1f);
        if (param.tweenTime > 1)
        {
            param.tweenTime = 1;
            param.isTweening = false;
        }
        else
        {
            param.isTweening = true;
        }

        float x = Mathf.Lerp(rect.x + rect.width / 2, rect.x, param.tweenTime);
        float y = Mathf.Lerp(rect.y + rect.height / 2, rect.y, param.tweenTime);
        float w = Mathf.Lerp(0, rect.width, param.tweenTime);
        float h = Mathf.Lerp(0, rect.height, param.tweenTime);
        rect = new Rect(x, y, w, h);
        return param;
    }

    public static string GetIconPath(SubWindowIcon icon)
    {
        switch (icon)
        {
            case SubWindowIcon.None:
                return null;
            case SubWindowIcon.Animation:
                return "d_UnityEditor.AnimationWindow";
            case SubWindowIcon.Animator:
                return "UnityEditor.Graphs.AnimatorControllerTool";
            case SubWindowIcon.AssetStore:
                return "Asset Store";
            case SubWindowIcon.AudioMixer:
                return "Audio Mixer";
            case SubWindowIcon.Web:
                return "BuildSettings.Web.Small";
            case SubWindowIcon.Console:
                return "d_UnityEditor.ConsoleWindow";
            case SubWindowIcon.Game:
                return "d_UnityEditor.GameView";
            case SubWindowIcon.Hierarchy:
                return "UnityEditor.HierarchyWindow";
            case SubWindowIcon.Inspector:
                return "d_UnityEditor.InspectorWindow";
            case SubWindowIcon.Lighting:
                return "Lighting";
            case SubWindowIcon.Navigation:
                return "Navigation";
            case SubWindowIcon.Occlusion:
                return "Occlusion";
            case SubWindowIcon.Profiler:
                return "d_UnityEditor.ProfilerWindow";
            case SubWindowIcon.Project:
                return "Project";
            case SubWindowIcon.Scene:
                return "d_UnityEditor.SceneView";
            case SubWindowIcon.BuildSetting:
                return "BuildSettings.SelectedIcon";
            case SubWindowIcon.Shader:
                return "Shader Icon";
            case SubWindowIcon.Avator:
                return "Avatar Icon";
            case SubWindowIcon.GameObject:
                return "GameObject Icon";
            case SubWindowIcon.Camera:
                return "Camera Icon";
            case SubWindowIcon.JavaScript:
                return "js Script Icon";
            case SubWindowIcon.CSharp:
                return "cs Script Icon";
            case SubWindowIcon.Sprite:
                return "Sprite Icon";
            case SubWindowIcon.Text:
                return "AnimatorController Icon";
            case SubWindowIcon.AnimatorController:
                return "cs Script Icon";
            case SubWindowIcon.MeshRenderer:
                return "MeshRenderer Icon";
            case SubWindowIcon.Terrain:
                return "Terrain Icon";
            case SubWindowIcon.Audio:
                return "SceneviewAudio";
            case SubWindowIcon.IPhone:
                return "BuildSettings.iPhone.small";
            case SubWindowIcon.Font:
                return "Font Icon";
            case SubWindowIcon.Material:
                return "Material Icon";
            case SubWindowIcon.GameManager:
                return "GameManager Icon";
            case SubWindowIcon.Player:
                return "Animation Icon";
            case SubWindowIcon.Texture:
                return "Texture Icon";
            case SubWindowIcon.Scriptable:
                return "ScriptableObject Icon";
            case SubWindowIcon.Movie:
                return "MovieTexture Icon";
            case SubWindowIcon.CGProgram:
                return "CGProgram Icon";
            case SubWindowIcon.Search:
                return "Search Icon";
            case SubWindowIcon.Favorite:
                return "Favorite Icon";
            case SubWindowIcon.Android:
                return "BuildSettings.Android.small";
            case SubWindowIcon.Setting:
                return "SettingsIcon";
            default:
                return null;
        }
    }
}
