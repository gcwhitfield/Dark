using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestLevelController : Singleton<TestLevelController>
{
    public PlayerInputManager playerInputManager;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(InputSystem.devices.Count);
        foreach (InputDevice d in InputSystem.devices)
        {
            print(d.name);
        }
        //playerInputManager.JoinPlayer();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
