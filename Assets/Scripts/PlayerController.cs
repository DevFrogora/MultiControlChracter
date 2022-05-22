using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    //private PlayerControls playerControls;
    //public float playerSpeed;
    //Vector2 move;

    //// Start is called before the first frame update
    //private void Awake()
    //{
    //    playerControls = new PlayerControls();
    //}

    //private void OnEnable()
    //{
    //    playerControls.Enable();
    //}

    //private void OnDisable()
    //{
    //    playerControls.Land.Move.performed -= Walk_performed;
    //    playerControls.Disable();
    //}

    //void Start()
    //{
    //    playerControls.Land.Move.performed += Walk_performed;
    //    playerControls.Land.Move.canceled += Walk_canceled;
    //}

    //private void Walk_performed(InputAction.CallbackContext context)
    //{
    //    move = context.ReadValue<Vector2>();
 
    //}

    //private void Walk_canceled(InputAction.CallbackContext context)
    //{
    //    move = Vector2.zero;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    transform.position += new Vector3 (move.x ,0,move.y) * playerSpeed * Time.deltaTime;
    //}
}
