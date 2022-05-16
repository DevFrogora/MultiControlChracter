using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AeroPlaneController : MonoBehaviour
{
    public PlayerStatus.Status currentAeroPlaneEnvStatus = PlayerStatus.Status.IsInParachute;

    public GameObject sitterPosition;
    public Vector3 cameraOffset = new Vector3(0.3795f, -1.23f, -4.739f);
    GameObject aeroplaneSeat;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    public void activePlayerAeroplaneLink(PlayerAeroPlaneLink playerInteractor)
    {
        PlayerStatus.Instance.status = PlayerStatus.Status.IsInPlane;

        playerInteractor.GetComponent<PlayerAeroPlaneLink>().enabled = true;
        playerInteractor.GetComponent<PlayerAeroPlaneLink>().aeroplaneController = this;
        playerInteractor.GetComponent<PlayerAeroPlaneLink>().init();
    }

    public void PlayerEnterAeroplane(PlayerAeroPlaneLink player)
    {
        player.GetComponent<Interactor>().enabled = false;
        PlayerAndAeroplaneTightCoupled(player);
        PlayerStatus.Instance.status = PlayerStatus.Status.IsInPlane;
    }

    void PlayerAndAeroplaneTightCoupled(PlayerAeroPlaneLink player)
    {
        // Turn on /off that this need to run when player in parachute
        player.GetComponent<Animator>().enabled = false;
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<CapsuleCollider>().enabled = false;

        //player Camera Positon when in parachute
        player.GetComponentInChildren<CinemachineCameraOffset>().m_Offset = cameraOffset;

        // player position and rotation when player is attached with parachute
        player.transform.position = sitterPosition.transform.position;
        player.transform.rotation = transform.localRotation;

        // setting player as child of parachute
        player.transform.parent = WrapSeatToPlayer(sitterPosition.transform);

    }

    Transform WrapSeatToPlayer(Transform _driverPosition)
    {
        aeroplaneSeat = new GameObject();
        aeroplaneSeat.transform.parent = _driverPosition;
        return aeroplaneSeat.transform;
    }


    public void PlayerExitAeroplane(PlayerAeroPlaneLink player)
    {
        PlayerAndAeroplaneDecoupled(player);

    }

    void PlayerAndAeroplaneDecoupled(PlayerAeroPlaneLink player)
    {
        if (PlayerStatus.Instance.status == PlayerStatus.Status.IsInPlane)
        {
            player.GetComponent<Animator>().enabled = true;
            player.GetComponent<PlayerAeroPlaneLink>().enabled = false;
            player.GetComponent<Rigidbody>().useGravity = true;
            player.GetComponent<Rigidbody>().isKinematic = false;
            player.GetComponent<CapsuleCollider>().enabled = true;

            //removing player from parachute
            player.transform.localScale = new Vector3(1, 1, 1);
            player.transform.SetParent(null);
            player.transform.position += new Vector3(-11, 0, 0);

            //player.GetComponent<CharacterLocomotion>().landActionMap.Enable();

            //disabling the playerParachute link ( for disabling the update() of parachute link)
            player.enabled = false;

            //player Camera Positon exit from camera
            player.GetComponentInChildren<CinemachineCameraOffset>().m_Offset = player.GetComponent<CharacterLocomotion>().defaultCameraPos;
            StartCoroutine(waiter(player));

        }
    }

    IEnumerator waiter(PlayerAeroPlaneLink player)
    {
        //isInParachute = false;
        Destroy(aeroplaneSeat);
        //Wait for 4 seconds
        yield return new WaitForSeconds(1);
        //player.GetComponent<Interactor>().enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
