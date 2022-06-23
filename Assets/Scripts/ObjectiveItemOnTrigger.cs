using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A Collider must be attached to the game GameObject that this
// ObjectiveItemOnTrigger is attached to for this script to work]]
// properly
public class ObjectiveItemOnTrigger : ObjectiveItem
{
    [System.Serializable]
    public enum TriggerType
    {
        ON_TRIGGER_ENTER,
        ON_TRIGGER_EXIT
    };

    public TriggerType triggerType;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerType == TriggerType.ON_TRIGGER_ENTER)
        {
            completed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerType == TriggerType.ON_TRIGGER_EXIT)
        {
            completed = true;
        }
    }
}
