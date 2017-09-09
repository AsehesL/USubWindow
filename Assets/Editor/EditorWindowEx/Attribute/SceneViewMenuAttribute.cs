using UnityEngine;
using System;
using System.Collections;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class SceneViewMenuAttribute : Attribute
{
    public string menu;

    public SceneViewMenuAttribute(string menu)
    {
        this.menu = menu;
    }
}
