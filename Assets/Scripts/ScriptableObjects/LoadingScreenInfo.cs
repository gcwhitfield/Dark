using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenInfo : ScriptableObject
{
    [TextArea(1, 25)]
    public string text;

    public Sprite image;
}
