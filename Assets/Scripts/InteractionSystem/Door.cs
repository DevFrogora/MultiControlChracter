using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    //Animator animator;
     Animation doorAnim;
    public bool isDoorOpen = false;
    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;

    private void Start()
    {
        //animator = GetComponent<Animator>();
        //animator.SetBool("isDoorOpen",isDoorOpen);

        doorAnim = GetComponentInParent<Animation>();
    }

    public bool Interact(Interactor interactor)
    {
        isDoorOpen = !isDoorOpen;
        //animator.SetBool("isDoorOpen", isDoorOpen);
        if (isDoorOpen) { doorAnim.Play("Door_Open");  }
        else { doorAnim.Play("Door_Close"); }
        
        return true;
    }


}
