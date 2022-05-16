using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParachuteLink : MonoBehaviour
{
    private InputActionMap parachuteActionMap;
    public PlayerParachuteControl parachuteController;
    Vector2 input;

    private void Awake()
    {
        //this.OnDisable();
    }
    private void OnEnable()
    {
        if (PlayerStatus.Instance.status == PlayerStatus.Status.IsInParachute)
        {
            Debug.Log("i got Enable when enter");
            parachuteActionMap = GetComponent<ActionMapManager>().SwitchActionMap("Parachute");
            //switch action map for control from player to car

            //Registering Function to listen on keyboard input
            parachuteActionMap["Move"].performed += ParachuteMovePerformed;
            parachuteActionMap["Move"].canceled += ParachuteMoveCanceled;
            parachuteActionMap["Exit"].performed += ParachuteExitPerformed;

        }

    }
    float speed = 0;
    public void init()
    {
        // enter this player to the parachute
        parachuteController.PlayerEnterParachute(this);
    }

    private void ParachuteMovePerformed(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    private void ParachuteMoveCanceled(InputAction.CallbackContext context)
    {
        input = Vector2.zero;
    }

    private void ParachuteExitPerformed(InputAction.CallbackContext context)
    {
        parachuteController.currentParachuteEnvStatus = PlayerStatus.Status.IsOnLand;
        ExitDependsOnEnvironment("Land",parachuteController.currentParachuteEnvStatus);
    }

    public void ExitDependsOnEnvironment(string area, PlayerStatus.Status environementStatus)
    {
        DisableMovementActionMap();
        parachuteController.PlayerExitParachute(this);
        // area is the action map here
        GetComponent<ActionMapManager>().SwitchActionMap(area);
        // environemtn depends upon parachute is in land or is in water
        PlayerStatus.Instance.status = environementStatus;
    }

    void DisableMovementActionMap()
    {
        ////UnRegistering Function to listen on keyboard input
        if (PlayerStatus.Instance.status == PlayerStatus.Status.IsInParachute)
        {
            parachuteActionMap["Move"].performed -= ParachuteMovePerformed;
            parachuteActionMap["Move"].canceled -= ParachuteMoveCanceled;
            parachuteActionMap.Disable();

        }
    }


    private void OnDisable()
    {
        DisableMovementActionMap();

    }

    private void Update()
    {
        if (!parachuteController.isInParachute)
            return;
        //Inputs
        input = parachuteActionMap["Move"].ReadValue<Vector2>();
        if(input.y>0)
        {
            parachuteController.FlyForwardSpeed += 5 * Time.deltaTime;
            parachuteController.FlyForwardSpeed = parachuteController.FlyForwardSpeed < 20 ? parachuteController.FlyForwardSpeed : 20;

            parachuteController.FlyDownwardSpeed += 2 * Time.deltaTime;
            parachuteController.FlyDownwardSpeed = parachuteController.FlyDownwardSpeed < 10 ? parachuteController.FlyDownwardSpeed : 10;

            //flySpeedVector = new  Vector3(0, 0, FlyForwardSpeed);
        }
        // down the value
        if(input == Vector2.zero)
        {
            parachuteController.FlyForwardSpeed -= 0.2f * Time.deltaTime;
            parachuteController.FlyForwardSpeed = parachuteController.FlyForwardSpeed < 5 ? 5 : parachuteController.FlyForwardSpeed;

            parachuteController.FlyDownwardSpeed -= 0.2f * Time.deltaTime;
            parachuteController.FlyDownwardSpeed = parachuteController.FlyDownwardSpeed < 1 ? 1 : parachuteController.FlyDownwardSpeed;
        }

        //Move Forward
        parachuteController.transform.position +=( parachuteController.transform.forward * parachuteController.FlyForwardSpeed + parachuteController.transform.up * (-parachuteController.FlyDownwardSpeed))* Time.deltaTime;

        //Yaw.Pitch,roll
        parachuteController.Yaw += input.x * parachuteController.YawAmount * Time.deltaTime;
        float pitch = Mathf.Lerp(0, 20, Mathf.Abs(input.y)) * Mathf.Sign(input.y);
        float roll = Mathf.Lerp(0, 30, Mathf.Abs(input.x)) * Mathf.Sign(input.x);

        //apply rotation
        //parachuteController.transform.localRotation = Quaternion.Euler((Vector3.up * Yaw + Vector3.right * pitch + Vector3.forward * roll));
        parachuteController.transform.localRotation = Quaternion.Lerp(parachuteController.transform.localRotation, Quaternion.Euler((Vector3.up * parachuteController.Yaw + Vector3.right * pitch + Vector3.forward * (-roll))),7f * Time.deltaTime);



    }




}
