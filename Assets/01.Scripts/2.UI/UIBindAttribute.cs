using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class UIBindAttribute : Attribute
{
    public string Path;
    public UIBindAttribute(string path) => Path = path;
}
