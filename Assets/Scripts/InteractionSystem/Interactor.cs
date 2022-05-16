using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius =  0.5f;
    [SerializeField] private LayerMask _interactionMask;

    private readonly Collider[] _colliders = new Collider[3]; // 3 collider (item ) we are checking
    [SerializeField] private int _numFound; // number of collider(item found)

    PlayerStatus playerStatus;
    private void Awake()
    {
        playerStatus = GetComponent<PlayerStatus>();

    }
    //int i = 0;
    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius,
            _colliders, _interactionMask);

        if(_numFound > 0)
        {
            var interactable = _colliders[0].GetComponent<IInteractable>();
            if(interactable != null && Keyboard.current.fKey.wasPressedThisFrame)
            {
                if (PlayerStatus.Instance.status == PlayerStatus.Status.IsInCar)
                {
                    return;
                }
                interactable.Interact(this);
                Debug.Log("From Interactor");

            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
