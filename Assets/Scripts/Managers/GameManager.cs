using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonPersistant<GameManager>
{
    public void QuitGame()
    {
        Application.Quit();
    }
}
