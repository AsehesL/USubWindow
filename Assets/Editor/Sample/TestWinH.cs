using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

/// <summary>
/// SubWindow动态窗口范例
/// </summary>
public class TestWinH : MDIEditorWindow
{
    private DynamicWin m_DynamicWin1;
    private DynamicWin m_DynamicWin2;


    [MenuItem("SubWindow范例/8.动态窗口范例")]
    static void InitWin()
    {
        TestWinA.CreateWindow<TestWinH>();
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

    [EWToolBar("工具/创建动态窗口C")]
    private void Test5()
    {
        if (m_DynamicWin1 == null)
            m_DynamicWin1 = new DynamicWin("动态窗口C", "XXXXXXX");
        AddDynamicSubWindow(m_DynamicWin1);
    }


    [EWToolBar("工具/移除动态窗口C")]
    private void Test6()
    {
        if (m_DynamicWin1 != null)
            RemoveDynamicSubWindow<DynamicWin>(m_DynamicWin1);
    }

    [EWToolBar("工具/创建动态窗口D")]
    private void Test7()
    {
        if (m_DynamicWin2 == null)
            m_DynamicWin2 = new DynamicWin("动态窗口D", "YYYYYYY");
        AddDynamicSubWindow(m_DynamicWin2);
    }


    [EWToolBar("工具/移除动态窗口D")]
    private void Test8()
    {
        if (m_DynamicWin2 != null)
            RemoveDynamicSubWindow<DynamicWin>(m_DynamicWin2);
    }

    private class DynamicWin : SubWindowCustomDrawer
    {
        public override GUIContent Title
        {
            get { return m_Title; }
        }

        public override EWSubWindowToolbarType toolBar
        {
            get { return EWSubWindowToolbarType.Normal; }
        }

        private GUIContent m_Title;

        private string m_Arg;

        public DynamicWin(string title, string arg)
        {
            m_Title = new GUIContent(title);
            if (arg == null)
                m_Arg = "Null";
            else
                m_Arg = arg;
        }

        public override void DrawMainWindow(Rect mainRect)
        {
            base.DrawMainWindow(mainRect);
            GUI.Label(mainRect, "Arg:" + m_Arg);
        }
    }
}
