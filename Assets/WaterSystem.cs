using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSystem : MonoBehaviour
{
    public GameObject Water;
    private void OnTriggerEnter(Collider other)
    {
        //other.gameObject.GetComponentInChildren<BodyParts>().Head.SetActive(true);
        other.gameObject.GetComponentInChildren<HeadSwimmingLink>().enabled = true;
        other.gameObject.GetComponentInChildren<HeadSwimmingLink>()._waterSurfacePosY = Water.transform.position.y; // adjusted  +1.6f

    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponentInChildren<HeadSwimmingLink>().enabled = false;



    }

}
