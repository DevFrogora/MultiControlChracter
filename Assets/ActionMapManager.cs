using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionMapManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public InputActionMap currentActionMap;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>() ;
        currentActionMap = playerInput.actions.FindActionMap("Land");
    }

    void Start()
    {

    }
     public InputActionMap SwitchActionMap( string switchName)
    {
        currentActionMap.Disable();
        currentActionMap = playerInput.actions.FindActionMap(switchName);
        playerInput.SwitchCurrentActionMap(switchName);
        currentActionMap.Enable();
        return currentActionMap;

    }
}
