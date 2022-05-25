using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class NoteUIController
{

    public GameObject noteUI;
    public TMP_Text titleText;
    public TMP_Text text;
    public TMP_Text pageDisplayText;

    // Opens the note UI with a particular set of dialogue
    public void Open(Dialogue d)
    {
        noteUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        text.text = d.text;
        titleText.text = d.title;
        text.pageToDisplay = 1;
        text.ForceMeshUpdate();
        pageDisplayText.text = text.pageToDisplay.ToString() + "/" + text.textInfo.pageCount.ToString();

        AudioManager.Instance.PlayAudio(AudioManager.Instance.pageTurn);
    }

    // Advances to the next page. if there are no pages left,
    // then it closes the note UI
    public void GoToNextPage()
    {
        if (text.pageToDisplay < text.textInfo.pageCount)
        {
            text.pageToDisplay++;
            AudioManager.Instance.PlayAudio(AudioManager.Instance.pageTurn);
            pageDisplayText.text = text.pageToDisplay.ToString() + "/" + text.textInfo.pageCount.ToString();
        } else {
            Close();
        }
    }

    // Go to the previous page. If we are currently at the first page, then do nothing
    public void GoToPreviousPage()
    {
        if (text.pageToDisplay > 1)
        {
            text.pageToDisplay--;
            AudioManager.Instance.PlayAudio(AudioManager.Instance.pageTurn);
            pageDisplayText.text = text.pageToDisplay.ToString() + "/" + text.textInfo.pageCount.ToString();
        }
    }

    public void Close()
    {
        noteUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // change the action map back to Play
        PlayerInputHandler.Instance.playerInput.SwitchCurrentActionMap("Play");
    }
}
