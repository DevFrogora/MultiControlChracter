using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAiming : MonoBehaviour
{
    public float turnSpeed = 15;
    Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        PlayerStatus.Instance.status = PlayerStatus.Status.IsOnLand;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!(PlayerStatus.Instance.status == PlayerStatus.Status.IsOnLand || PlayerStatus.Instance.status == PlayerStatus.Status.IsGliding || PlayerStatus.Instance.status == PlayerStatus.Status.IsSwimming))
            return;

        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }
}
