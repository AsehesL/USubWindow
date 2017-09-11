using System;
using UnityEngine;
using System.Collections;
using System.Reflection;

/// <summary>
/// 网格风格子窗体
/// </summary>
[SubWindowStyle(SubWindowStyle.Grid)]
public class GridSubWindow : SubWindow
{

    private Texture2D m_PanelBackground;

    private Vector2 m_SceneViewPosition;

    private bool m_IsDragging;

    private const int kTileSize = 100;

    private int m_TileCountX = 0;
    private int m_TileCountY = 0;

    public GridSubWindow(string title, string icon, bool defaultOpen, MethodInfo method, System.Object target, SubWindowToolbarType toolbar, SubWindowHelpBoxType helpbox) : base(title, icon, defaultOpen, method, target, toolbar, helpbox)
    {
    }

    protected override Rect DrawMainArea(Rect rect)
    {
        GUI.BeginGroup(rect, GUIStyleCache.GetStyle("GameViewBackground"));
        if (m_PanelBackground == null)
            CreatePanelBackground();

        int tileCountX = Mathf.CeilToInt(rect.width / kTileSize);
        int tileCountY = Mathf.CeilToInt(rect.width / kTileSize);

        if (m_TileCountX != tileCountX || m_TileCountY != tileCountY)
            CheckBoard(rect, tileCountX, tileCountY);

        m_TileCountX = tileCountX;
        m_TileCountY = tileCountY;

        for (int i = -tileCountX; i < 2 * tileCountX; i++)
        {
            for (int j = -tileCountY; j < 2 * tileCountY; j++)
            {
                GUI.DrawTexture(new Rect(rect.x + m_SceneViewPosition.x + i * kTileSize, rect.y + m_SceneViewPosition.y + j * kTileSize, kTileSize, kTileSize), m_PanelBackground);
            }
        }

        GUI.EndGroup();

        ListenDrawMainPanel(rect);

        return new Rect(rect.x + m_SceneViewPosition.x, rect.y + m_SceneViewPosition.y, m_TileCountX * 2 * kTileSize,
            m_TileCountY * 2 * kTileSize);
    }

    void CreatePanelBackground()
    {
        m_PanelBackground = new Texture2D(kTileSize, kTileSize, TextureFormat.RGBA32, false);
        for (int i = 0; i < kTileSize; i++)
        {
            for (int j = 0; j < kTileSize; j++)
            {
                if (i == 0 || j == 0)
                    m_PanelBackground.SetPixel(i, j, new Color(0.05f, 0.05f, 0.05f));
                else if (i % 10 == 0 || j % 10 == 0)
                    m_PanelBackground.SetPixel(i, j, new Color(0.133f, 0.133f, 0.133f));
                else
                    m_PanelBackground.SetPixel(i, j, new Color(0, 0, 0, 0));
            }
        }

        m_PanelBackground.Apply();
    }

    void ListenDrawMainPanel(Rect rect)
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 2)
        {
            if (rect.Contains(Event.current.mousePosition))
            {
                m_IsDragging = true;
                Event.current.Use();
            }
        }
        if (m_IsDragging && Event.current.type == EventType.MouseUp && Event.current.button == 2)
        {
            Event.current.Use();
            m_IsDragging = false;
        }
        if (m_IsDragging && Event.current.type == EventType.MouseDrag && Event.current.button == 2)
        {
            m_SceneViewPosition += Event.current.delta;
            CheckBoard(rect, m_TileCountX, m_TileCountY);
            Event.current.Use();
        }
    }

    private void CheckBoard(Rect rect, int tileCountX, int tileCountY)
    {
        if (m_SceneViewPosition.x < rect.width - tileCountX * 2 * kTileSize)
            m_SceneViewPosition.x = rect.width - tileCountX * 2 * kTileSize;
        if (m_SceneViewPosition.x > tileCountX * kTileSize)
            m_SceneViewPosition.x = tileCountX * kTileSize;
        if (m_SceneViewPosition.y < rect.height - tileCountY * 2 * kTileSize - 20)
            m_SceneViewPosition.y = rect.height - tileCountY * 2 * kTileSize - 20;
        if (m_SceneViewPosition.y > tileCountY * kTileSize - 20)
            m_SceneViewPosition.y = tileCountY * kTileSize - 20;
    }
}
