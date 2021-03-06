using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gliding : MonoBehaviour
{
    private InputActionMap glidingActionMap;
    Vector2 input;

    bool isGliding;

    private void Awake()
    {
        //this.OnDisable();
    }
    private void OnEnable()
    {
        if (PlayerStatus.Instance.status == PlayerStatus.Status.IsGliding)
        {
            Debug.Log("i got Enable when enter");
            glidingActionMap = GetComponent<ActionMapManager>().SwitchActionMap("GlidingControl");
            //switch action map for control from player to car

            //Registering Function to listen on keyboard input
            glidingActionMap["Move"].performed += GlidingMovePerformed;
            glidingActionMap["Move"].canceled += GlidingMoveCanceled;
            glidingActionMap["Exit"].performed += GlidingExitPerformed;
            isGliding = true;
        }

    }
    float speed = 0;
    public void init()
    {
        // enter this player to the parachute
        //parachuteController.PlayerEnterParachute(this);
    }

    private void GlidingMovePerformed(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    private void GlidingMoveCanceled(InputAction.CallbackContext context)
    {
        input = Vector2.zero;
    }

    private void GlidingExitPerformed(InputAction.CallbackContext context)
    {
        //parachuteController.currentParachuteEnvStatus = PlayerStatus.Status.IsOnLand;
        //ExitDependsOnEnvironment("Land", parachuteController.currentParachuteEnvStatus);
    }

    public void ExitDependsOnEnvironment(string area, PlayerStatus.Status environementStatus)
    {
        DisableMovementActionMap();
        //parachuteController.PlayerExitParachute(this);
        // area is the action map here
        GetComponent<ActionMapManager>().SwitchActionMap(area);
        // environemtn depends upon parachute is in land or is in water
        PlayerStatus.Instance.status = environementStatus;
    }

    void DisableMovementActionMap()
    {
        ////UnRegistering Function to listen on keyboard input
        if (PlayerStatus.Instance.status == PlayerStatus.Status.IsGliding)
        {
            glidingActionMap["Move"].performed -= GlidingMovePerformed;
            glidingActionMap["Move"].canceled -= GlidingMoveCanceled;
            glidingActionMap.Disable();
            isGliding = false;

        }
    }


    private void OnDisable()
    {
        DisableMovementActionMap();

    }

    //Rigidbody rb;
    public float FlyForwardSpeed = 5;
    public float FlyDownwardSpeed = 1;
    //Vector3 flySpeedVector = new Vector3(0, 0, 5);
    public float YawAmount = 120;
    public float Yaw;

    private void Update()
    {
        if (!isGliding)
            return;
        //Inputs
        input = glidingActionMap["Move"].ReadValue<Vector2>();
        if (input.y > 0)
        {
            FlyForwardSpeed += 5 * Time.deltaTime;
            FlyForwardSpeed = FlyForwardSpeed < 20 ? FlyForwardSpeed : 20;

            FlyDownwardSpeed += 2 * Time.deltaTime;
            FlyDownwardSpeed = FlyDownwardSpeed < 10 ? FlyDownwardSpeed : 10;

            //flySpeedVector = new  Vector3(0, 0, FlyForwardSpeed);
        }
        // down the value
        if (input == Vector2.zero)
        {
            FlyForwardSpeed -= 0.2f * Time.deltaTime;
            FlyForwardSpeed = FlyForwardSpeed < 5 ? 5 : FlyForwardSpeed;

            FlyDownwardSpeed -= 0.2f * Time.deltaTime;
            FlyDownwardSpeed = FlyDownwardSpeed < 1 ? 1 : FlyDownwardSpeed;
        }

        //Move Forward
        transform.position += (transform.forward * FlyForwardSpeed + transform.up * (-FlyDownwardSpeed)) * Time.deltaTime;

        //Yaw.Pitch,roll
        Yaw += input.x * YawAmount * Time.deltaTime;
        float pitch = Mathf.Lerp(0, 20, Mathf.Abs(input.y)) * Mathf.Sign(input.y);
        float roll = Mathf.Lerp(0, 30, Mathf.Abs(input.x)) * Mathf.Sign(input.x);

        //apply rotation
        //parachuteController.transform.localRotation = Quaternion.Euler((Vector3.up * Yaw + Vector3.right * pitch + Vector3.forward * roll));
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler((Vector3.up * Yaw + Vector3.right * pitch + Vector3.forward * (-roll))), 7f * Time.deltaTime);



    }
}
