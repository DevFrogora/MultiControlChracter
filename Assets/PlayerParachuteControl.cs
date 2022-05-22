using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParachuteControl : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;

    public GameObject driverPosition;
    public bool isInParachute = false;

    public Vector3 cameraOffset = new Vector3(0.3795f, 0.43f, -4.739f);
    GameObject driverSeat;
    public PlayerStatus.Status currentParachuteEnvStatus = PlayerStatus.Status.IsInParachute;

    //Rigidbody rb;
    public float FlyForwardSpeed = 5;
    public float FlyDownwardSpeed = 1;
    //Vector3 flySpeedVector = new Vector3(0, 0, 5);
    public float YawAmount = 120;
    public float Yaw;

    private void Awake()
    {
        //playerStatus = GetComponent<PlayerStatus>();

    }

    public bool Interact(Interactor interactor)
    {
        isInParachute = !isInParachute;
        if (isInParachute)
        {
            // starting point to enter parachute
            activePlayerParachuteLink(interactor);
        }
        return true;
    }

    public void activePlayerParachuteLink(Interactor playerInteractor)
    {
        PlayerStatus.Instance.status = PlayerStatus.Status.IsInParachute;
        isInParachute = true;
        playerInteractor.GetComponent<PlayerParachuteLink>().enabled = true;
        playerInteractor.GetComponent<PlayerParachuteLink>().parachuteController = this;
        playerInteractor.GetComponent<PlayerParachuteLink>().init();
    }


    public void PlayerEnterParachute(PlayerParachuteLink player)
    {
        player.GetComponent<Interactor>().enabled = false;
        PlayerAndParachuteTightCoupled(player);
        currentParachuteEnvStatus = PlayerStatus.Status.IsInParachute;
        PlayerStatus.Instance.status = PlayerStatus.Status.IsInParachute;
    }

    void PlayerAndParachuteTightCoupled(PlayerParachuteLink player)
    {
        // Turn on /off that this need to run when player in parachute
        //player.GetComponent<Animator>().enabled = false;
        player.GetComponent<Animator>().applyRootMotion = false;
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<CapsuleCollider>().enabled = false;

        //player Camera Positon when in parachute
        player.GetComponentInChildren<CinemachineCameraOffset>().m_Offset = cameraOffset;

        // player position and rotation when player is attached with parachute
        player.transform.position = driverPosition.transform.position;
        player.transform.rotation = transform.localRotation;
        Debug.Log("Parachute Player Rotation : " + player.transform.rotation);
        Debug.Log("Parachute  Rotation : " + transform.rotation);

        // setting player as child of parachute
        player.transform.parent = WrapSeatToPlayer(driverPosition.transform);

    }

    Transform WrapSeatToPlayer(Transform _driverPosition)
    {
        driverSeat = new GameObject();
        driverSeat.transform.parent = _driverPosition;
        return driverSeat.transform;
    }

    public void PlayerExitParachute(PlayerParachuteLink player)
    {
        PlayerAndParachuteDecoupled(player);

    }

    void PlayerAndParachuteDecoupled(PlayerParachuteLink player)
    {
        if (PlayerStatus.Instance.status == PlayerStatus.Status.IsInParachute)
        {
            player.GetComponent<Animator>().enabled = true;
            player.GetComponent<PlayerParachuteLink>().enabled = false;
            player.GetComponent<Rigidbody>().useGravity = true;
            player.GetComponent<Rigidbody>().isKinematic = false;
            player.GetComponent<CapsuleCollider>().enabled = true;

            //removing player from parachute
            player.transform.localScale = new Vector3(1, 1, 1);
            player.transform.SetParent(null);

            //player.GetComponent<CharacterLocomotion>().landActionMap.Enable();

            //disabling the playerParachute link ( for disabling the update() of parachute link)
            player.enabled = false;

            //player Camera Positon exit from camera
            player.GetComponentInChildren<CinemachineCameraOffset>().m_Offset = player.GetComponent<CharacterLocomotion>().defaultCameraPos;
            StartCoroutine(waiter(player));

        }
    }

    IEnumerator waiter(PlayerParachuteLink player)
    {
        isInParachute = false;
        Destroy(driverSeat);
        //Wait for 4 seconds
        yield return new WaitForSeconds(1);
        player.GetComponent<Interactor>().enabled = true;
        Debug.Log("Exited and Interactor Enabled");

    }

    public int speed = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if(PlayerStatus.Instance.status == PlayerStatus.Status.IsInParachute)
        {
            if (collision.collider.gameObject.tag == "Water" || collision.collider.gameObject.tag == "Land")
            {
                currentParachuteEnvStatus = PlayerStatus.Status.IsOnLand;
                GetComponentInChildren<Animator>().applyRootMotion = true;
                GetComponentInChildren<Animator>().SetLayerWeight(4,0);
                GetComponentInChildren<PlayerParachuteLink>().ExitDependsOnEnvironment("Land", currentParachuteEnvStatus);

                Debug.Log("Player Collide with land fired , In Parachute");
            }
        }
    }

}
