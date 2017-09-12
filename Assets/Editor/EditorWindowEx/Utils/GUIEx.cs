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

    public static string GetIconPath(EWSubWindowIcon icon)
    {
        switch (icon)
        {
            case EWSubWindowIcon.None:
                return null;
            case EWSubWindowIcon.Animation:
                return "d_UnityEditor.AnimationWindow";
            case EWSubWindowIcon.Animator:
                return "UnityEditor.Graphs.AnimatorControllerTool";
            case EWSubWindowIcon.AssetStore:
                return "Asset Store";
            case EWSubWindowIcon.AudioMixer:
                return "Audio Mixer";
            case EWSubWindowIcon.Web:
                return "BuildSettings.Web.Small";
            case EWSubWindowIcon.Console:
                return "d_UnityEditor.ConsoleWindow";
            case EWSubWindowIcon.Game:
                return "d_UnityEditor.GameView";
            case EWSubWindowIcon.Hierarchy:
                return "UnityEditor.HierarchyWindow";
            case EWSubWindowIcon.Inspector:
                return "d_UnityEditor.InspectorWindow";
            case EWSubWindowIcon.Lighting:
                return "Lighting";
            case EWSubWindowIcon.Navigation:
                return "Navigation";
            case EWSubWindowIcon.Occlusion:
                return "Occlusion";
            case EWSubWindowIcon.Profiler:
                return "d_UnityEditor.ProfilerWindow";
            case EWSubWindowIcon.Project:
                return "Project";
            case EWSubWindowIcon.Scene:
                return "d_UnityEditor.SceneView";
            case EWSubWindowIcon.BuildSetting:
                return "BuildSettings.SelectedIcon";
            case EWSubWindowIcon.Shader:
                return "Shader Icon";
            case EWSubWindowIcon.Avator:
                return "Avatar Icon";
            case EWSubWindowIcon.GameObject:
                return "GameObject Icon";
            case EWSubWindowIcon.Camera:
                return "Camera Icon";
            case EWSubWindowIcon.JavaScript:
                return "js Script Icon";
            case EWSubWindowIcon.CSharp:
                return "cs Script Icon";
            case EWSubWindowIcon.Sprite:
                return "Sprite Icon";
            case EWSubWindowIcon.Text:
                return "AnimatorController Icon";
            case EWSubWindowIcon.AnimatorController:
                return "cs Script Icon";
            case EWSubWindowIcon.MeshRenderer:
                return "MeshRenderer Icon";
            case EWSubWindowIcon.Terrain:
                return "Terrain Icon";
            case EWSubWindowIcon.Audio:
                return "SceneviewAudio";
            case EWSubWindowIcon.IPhone:
                return "BuildSettings.iPhone.small";
            case EWSubWindowIcon.Font:
                return "Font Icon";
            case EWSubWindowIcon.Material:
                return "Material Icon";
            case EWSubWindowIcon.GameManager:
                return "GameManager Icon";
            case EWSubWindowIcon.Player:
                return "Animation Icon";
            case EWSubWindowIcon.Texture:
                return "Texture Icon";
            case EWSubWindowIcon.Scriptable:
                return "ScriptableObject Icon";
            case EWSubWindowIcon.Movie:
                return "MovieTexture Icon";
            case EWSubWindowIcon.CGProgram:
                return "CGProgram Icon";
            case EWSubWindowIcon.Search:
                return "Search Icon";
            case EWSubWindowIcon.Favorite:
                return "Favorite Icon";
            case EWSubWindowIcon.Android:
                return "BuildSettings.Android.small";
            case EWSubWindowIcon.Setting:
                return "SettingsIcon";
            default:
                return null;
        }
    }
}
