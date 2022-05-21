using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/New Dialogue")]
public class Dialogue : ScriptableObject
{
    public string title; // the title of the dialogue
    [TextArea(1, 100)]
    public string text; // the text to display for the dialogue
}
