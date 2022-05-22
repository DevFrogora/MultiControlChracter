using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSystem : MonoBehaviour
{
    public GameObject Water;
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<SwimmingLink>().enabled = true;
        other.gameObject.GetComponent<SwimmingLink>()._waterSurfacePosY = Water.transform.position.y; // adjusted  +1.6f
        other.gameObject.GetComponent<SwimmingLink>()._isInWater = true;

    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<SwimmingLink>()._isInWater = false;
        other.gameObject.GetComponent<SwimmingLink>().enabled = false;

    }

}
