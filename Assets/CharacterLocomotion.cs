using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterLocomotion : MonoBehaviour
{
    Animator animator;
    Vector2 input;


    public InputActionMap landActionMap;


    //Camera Position on Car and plane or land or water
    //public CinemachineCameraOffset camera;
    //default Vector3(0.38,-0.15,-0.84)
    public Vector3 defaultCameraPos = new Vector3(0.38f, -0.15f, -0.84f);

    // Start is called before the first frame update
    private void Awake()
    {

    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        landActionMap["Move"].performed -= Walk_performed;
        landActionMap.Disable();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        landActionMap = GetComponent<ActionMapManager>().SwitchActionMap("Land");
        landActionMap["Move"].performed += Walk_performed;
        landActionMap["Move"].canceled += Walk_canceled;
    }

    private void Walk_performed(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        Walk_animation(input);
        //Debug.Log(input);
    }

    private void Walk_canceled(InputAction.CallbackContext context)
    {
        input = Vector2.zero;
        Walk_animation(input);
    }

    void Walk_animation(Vector2 input)
    {
        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
    }
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {

    }

}
