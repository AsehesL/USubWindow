using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// SubWindow动态窗口范例
/// </summary>
public class TestWinH : MDIEditorWindow {



    [MenuItem("SubWindow范例/8.动态窗口范例")]
    static void Init()
    {
        TestWinH win = TestWinA.CreateWindow<TestWinH>();
    }
    
    private void SubWinA(Rect main)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "这是动态窗口A");
    }
    
    private void SubWinB(Rect main, Rect toolbar)
    {
        GUI.Label(new Rect(main.x, main.y, main.width, 20), "这是动态窗口B");
    }

    [EWToolBar("工具/创建动态窗口A")]
    private void Test1()
    {
        AddDynamicSubWindow("动态窗口A", EWSubWindowIcon.Navigation, SubWinA);
    }

    [EWToolBar("工具/移除动态窗口A")]
    private void Test2()
    {
        RemoveDynamicSubWindow(SubWinA);
    }

    [EWToolBar("工具/创建动态窗口B")]
    private void Test3()
    {
        AddDynamicSubWindowWithToolBar("动态窗口B", EWSubWindowIcon.Movie, EWSubWindowToolbarType.Mini, SubWinB);
    }


    [EWToolBar("工具/移除动态窗口B")]
    private void Test4()
    {
        RemoveDynamicSubWindow(SubWinB);
    }
}
