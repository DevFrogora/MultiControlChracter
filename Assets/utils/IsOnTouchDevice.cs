using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsOnTouchDevice : MonoBehaviour
{
    public GameObject mobileControlCanvas;
    // Start is called before the first frame update
    void Start()
    {
        if(SystemInfo.deviceType == DeviceType.Handheld)
        {
            //this is a touch device
            mobileControlCanvas.SetActive(true);
        }
        else
        {
            // this is a non touch device
            mobileControlCanvas.SetActive(false);
        }
    }

}
