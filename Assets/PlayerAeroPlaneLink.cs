using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAeroPlaneLink : MonoBehaviour
{
    private InputActionMap planeActionMap;
    public AeroPlaneController aeroplaneController;
    Vector2 input;
    Animator animator;

    private void Awake()
    {
        //this.OnDisable();
    }
    private void OnEnable()
    {
        if (PlayerStatus.Instance.status == PlayerStatus.Status.IsInPlane)
        {
            Debug.Log("Enable AeroPlaneLink");
            planeActionMap = GetComponent<ActionMapManager>().SwitchActionMap("PlaneControl");
            //switch action map for control from player to car

            //Registering Function to listen on keyboard input
            planeActionMap["Jump"].performed += ParachuteJumpPerformed;


        }

    }

    private void Start()
    {

        //animator.enabled = true;
    }
    public void init()
    {
        // enter this player to the parachute
        animator = GetComponent<Animator>();
        aeroplaneController.PlayerEnterAeroplane(this);
        //animator.SetLayerWeight(1, 0);
    }

    private void ParachuteJumpPerformed(InputAction.CallbackContext context)
    {
        //input = context.ReadValue<Vector2>();
        aeroplaneController.currentAeroPlaneEnvStatus = PlayerStatus.Status.IsGliding;
        StartCoroutine(planeJumpAnimation());
    }
    IEnumerator planeJumpAnimation()
    {

        //Wait for 4 seconds
        ExitDependsOnEnvironment("Gliding", PlayerStatus.Status.IsGliding);
        yield return new WaitForSeconds(0.09f);
        animator.SetBool("planeJump", true);
        yield return new WaitForSeconds(0.5f);
        GetComponent<GlidingLink>().enabled = true;
        animator.SetLayerWeight(1, 0);

    }


    public void ExitDependsOnEnvironment(string actionMapArea, PlayerStatus.Status environementStatus)
    {
        DisableMovementActionMap();
        aeroplaneController.PlayerExitAeroplane(this);
        // area is the action map here
        GetComponent<ActionMapManager>().SwitchActionMap(actionMapArea);
        
        // environemtn depends upon parachute is in land or is in water
        PlayerStatus.Instance.status = environementStatus;
    }

    void DisableMovementActionMap()
    {
        ////UnRegistering Function to listen on keyboard input
        if (PlayerStatus.Instance.status == PlayerStatus.Status.IsInPlane)
        {
            planeActionMap.Disable();

        }
    }


    private void OnDisable()
    {
        DisableMovementActionMap();

    }

 


}
