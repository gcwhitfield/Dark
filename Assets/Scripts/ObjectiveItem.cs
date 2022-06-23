using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ObjectiveItem : MonoBehaviour
{
    // this will be set to 'true' when the objective has been completed
    [HideInInspector]
    public bool completed;
}
