using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCarLink : MonoBehaviour
{
    private InputActionMap carActionMap;
    public CarController carController;
    Vector2 input;

    private void Awake()
    {
        //this.OnDisable();
    }
    private void OnEnable()
    {
        if (PlayerStatus.Instance.status == PlayerStatus.Status.IsInCar)
        {
            Debug.Log("i got Enable when enter");
            carActionMap = GetComponent<ActionMapManager>().SwitchActionMap("CarDriving");
            //switch action map for control from player to car
            carActionMap["Move"].performed += CarMovePerformed;
            carActionMap["Move"].canceled += CarMoveCanceled;
            carActionMap["Exit"].performed += CarExitPerformed;
        }

    }

    public void init()
    {
        carController.PlayerEnterCar(this);
    }

    private void CarMovePerformed(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        carController.CarMovePerf(input);

    }

    private void CarMoveCanceled(InputAction.CallbackContext context)
    {
        input = Vector2.zero;
        carController.CarMoveCancel(input);
    }

    private void OnDisable()
    {
        if(PlayerStatus.Instance.status == PlayerStatus.Status.IsInCar)
        {
            carActionMap["Move"].performed -= CarMovePerformed;
            carActionMap["Move"].canceled -= CarMoveCanceled;

            carActionMap.Disable();
            Debug.Log("I got Diabled because i am exiting the car");
        }

    }


    private void CarExitPerformed(InputAction.CallbackContext context)
    {
        DisableMovementActionMap();
        carController.PlayerExitCar(this);

    }
    void DisableMovementActionMap()
    {
        if (PlayerStatus.Instance.status == PlayerStatus.Status.IsInCar)
        {
            carActionMap["Move"].performed -= CarMovePerformed;
            carActionMap["Move"].canceled -= CarMoveCanceled;

            carActionMap.Disable();
            Debug.Log("Exiting the Car");

        }
    }

    private void Update()
    {
        transform.position = carController.driverPosition.transform.position;
    }

}
