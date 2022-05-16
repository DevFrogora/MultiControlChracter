using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPlane : MonoBehaviour
{
    public Transform planeTransformPos;
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = planeTransformPos.position;
    }

}
