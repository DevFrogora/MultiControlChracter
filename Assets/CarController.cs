using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour ,IInteractable
{

    #region InteractionWithCar
    public bool isInCar = false;
    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;


    public GameObject driverPosition;

    public Vector3 cameraOffset = new Vector3(0.3795f, -1.23f, -4.739f);

    GameObject driverSeat;

    private void Awake()
    {
        //playerStatus = GetComponent<PlayerStatus>();

    }

    public bool Interact(Interactor interactor)
    {
 
        isInCar = !isInCar;
        if(isInCar)
        {
            PlayerStatus.Instance.status = PlayerStatus.Status.IsInCar;

            interactor.GetComponent<PlayerCarLink>().enabled = true;
            interactor.GetComponent<PlayerCarLink>().carController = this;
            interactor.GetComponent<PlayerCarLink>().init();

        }
        //drop the player inside the car or outside
        //position the cinemachine little bit far from opposite direction of player 


        return true;
    }


    public void PlayerEnterCar(PlayerCarLink player)
    {
        player.transform.position = driverPosition.transform.position;

        //interactor.gameObject.transform.SetParent(driverPosition, true);
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponent<Rigidbody>().isKinematic = true;


        player.transform.parent = WrapSeatToPlayer(driverPosition.transform);

        //interactor.transform.localScale = new Vector3(1, 1, 0.3333333f);
        //interactor.transform.parent.gameObject = driverPosition;

        player.GetComponent<Interactor>().enabled = false;

        PlayerStatus.Instance.status = PlayerStatus.Status.IsInCar;
        player.GetComponentInChildren<CinemachineCameraOffset>().m_Offset = cameraOffset;
        player.GetComponent<CapsuleCollider>().enabled = false;
        player.GetComponent<CharacterLocomotion>().landActionMap.Disable();

        Debug.Log(PlayerStatus.Instance.status);

    }

    Transform WrapSeatToPlayer(Transform _driverPosition)
    {
        driverSeat = new GameObject();
        driverSeat.transform.parent = _driverPosition;
        return driverSeat.transform;
    }

    public void PlayerExitCar(PlayerCarLink player)
    {

        if (PlayerStatus.Instance.status == PlayerStatus.Status.IsInCar)
        {
            player.GetComponent<PlayerCarLink>().enabled = false;

            player.transform.localScale = new Vector3(1, 1, 1);
            player.GetComponent<Rigidbody>().isKinematic = false;
            player.GetComponent<Rigidbody>().useGravity = true;

            Debug.Log("From F Key");
            player.transform.SetParent(null);
            player.GetComponent<CapsuleCollider>().enabled = true;


            player.GetComponent<ActionMapManager>().SwitchActionMap("Land");
            player.GetComponent<CharacterLocomotion>().landActionMap.Enable();
            PlayerStatus.Instance.status = PlayerStatus.Status.IsOnLand;
            player.enabled = false;
            Debug.Log(PlayerStatus.Instance.status);
            player.GetComponentInChildren<CinemachineCameraOffset>().m_Offset = player.GetComponent<CharacterLocomotion>().defaultCameraPos;
            StartCoroutine(waiter(player));


        }


    }

    IEnumerator waiter(PlayerCarLink player)
    {
        isInCar = false;
        Destroy(driverSeat);
        //Wait for 4 seconds
        yield return new WaitForSeconds(1);
        player.GetComponent<Interactor>().enabled = true;
        Debug.Log("Exited and Interactor Enabled");

    }


    public void CarMovePerf(Vector2 input )
    {
        float motor = maxMotorTorque * input.y;
        float steering = maxSteeringAngles* input.x;
        foreach(Axilinfo axilinfo in axilinfos)
        {
            if (axilinfo.steering == true)
            {
                axilinfo.leftWheel.steerAngle = -steering;
                axilinfo.rightWheel.steerAngle = -steering;
            }
            if(axilinfo.motor == true)
            {
                axilinfo.leftWheel.motorTorque = motor;
                axilinfo.rightWheel.motorTorque = motor;
            }
        }
        //Debug.Log(input);
    }

    public void CarMoveCancel(Vector2 input)
    {
        float motor = maxMotorTorque * input.y;
        float steering = maxSteeringAngles * input.x;
        foreach (Axilinfo axilinfo in axilinfos)
        {
            if (axilinfo.steering == true)
            {
                axilinfo.leftWheel.steerAngle = -steering;
                axilinfo.rightWheel.steerAngle = -steering;
            }
            if (axilinfo.motor == true)
            {
                axilinfo.leftWheel.motorTorque = motor;
                axilinfo.rightWheel.motorTorque = motor;
            }
        }
    }


    #endregion



    [System.Serializable]
    public class Axilinfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;

        public bool motor;
        public bool steering;

    }

    public List<Axilinfo> axilinfos = new List<Axilinfo>();
    public float maxMotorTorque;
    public float maxSteeringAngles;




}
