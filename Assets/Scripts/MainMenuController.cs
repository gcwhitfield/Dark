using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : Singleton<MainMenuController>
{
    public SceneReference nextScene;

    public void BeginButtonClicked()
    {
        SceneTransitionManager.Instance.TransitionToScene(nextScene);
    }
}
