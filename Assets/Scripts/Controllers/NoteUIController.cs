using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoteUIController
{
 
    public GameObject noteUI;

    public void Open()
    {
        noteUI.SetActive(true);
        PlayerController.Instance.disableControls = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // advances to the next page. if there are no pages left,
    // then it closes the note UI
    public void GoToNextPage()
    {

    }

    public void GoToPreviousPage()
    {

    }

    public void Close()
    {
        noteUI.SetActive(false);
        PlayerController.Instance.disableControls = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
