using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonPersistant<GameManager>
{
    public SceneReference loadingScene;

    public void QuitGame()
    {
        Application.Quit();
    }
}
