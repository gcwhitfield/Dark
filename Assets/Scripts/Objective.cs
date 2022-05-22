using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// An Objective contains a list of 'tasks' (ObjectiveItem) that the player must complete.
// At Start, Objective begins to check every frame whether all of the tasks have been
// completed. Once the tasks have been completed, the 'objectiveCompletedFunction' will
// be executed.
public class Objective : MonoBehaviour
{
    public List<ObjectiveItem> tasks;

    // This function will get executed when the objective has been completed.
    // This is set in the Unity Inspector
    public UnityEvent objectiveCompletedFunction;

    private void Start()
    {
        StartCoroutine("WaitForTasksCompleted");
    }

    // Returns true if there are any tasks remaining to do 
    bool TasksRemaining()
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            if (!tasks[i].completed)
            {
                return true;
            }
        }
        return false;
    }

    // Waits for all of the tasks to be completed. Then, executes
    // the ObjectiveCompleted function
    IEnumerator WaitForTasksCompleted()
    {
        while (TasksRemaining())
        {
            yield return null;
        }
        OnObjectiveCompleted();
    }

    // This function is called when the objective is completed
    public void OnObjectiveCompleted()
    {
        if (objectiveCompletedFunction != null)
        {
            objectiveCompletedFunction.Invoke();
        }
    }
}
