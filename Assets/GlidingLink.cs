using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlidingLink : MonoBehaviour
{
    private InputActionMap glidingActionMap;
    Vector2 input;
    Animator animator;
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
            glidingActionMap = GetComponent<ActionMapManager>().SwitchActionMap("Gliding");
            //switch action map for control from player to car
            
            //Registering Function to listen on keyboard input
            glidingActionMap["Move"].performed += GlidingMovePerformed;
            glidingActionMap["Move"].canceled += GlidingMoveCanceled;
            glidingActionMap["Open"].performed += GlidingOpenParachute;
            isGliding = true;

            glidingActionMap.Enable();
            init();
        }

    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetLayerWeight(2, 1);
        //animator.SetLayerWeight(1, 0);
        //animator.enabled = true;
    }
    public void init()
    {
        // enter this player to the parachute
        //parachuteController.PlayerEnterParachute(this);
        Debug.Log(GetComponent<ActionMapManager>().currentActionMap.ToString());
    }

    private void GlidingMovePerformed(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        Gliding_animation(input);
    }

    private void GlidingMoveCanceled(InputAction.CallbackContext context)
    {
        input = Vector2.zero;
        Gliding_animation(input);
    }
    void Gliding_animation(Vector2 input)
    {
        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
    }
    private void GlidingOpenParachute(InputAction.CallbackContext context)
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
        Gliding_animation(input);
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

    }
}
