using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeadSwimmingLink : MonoBehaviour
{
    private InputActionMap swimmingActionMap;
    Vector2 input;
    Animator swimAnimator;
    Animator walkAnimator;


    //private bool _defaultFog = false;
    private bool _defFogEnabled;
    private FogMode _defFogMode;
    private float _defFogDensity = 0;
    private Color _defFogColor = Color.black;

    public Color _fogColorWater = Color.red;
    public Color bottomFogColor = Color.blue;
    public float fogDensity = 0.075f;


    public float _waterSurfacePosY = 0.0f;
    public float _aboveWaterTolerance = 0.5f;

    public bool _isInWater;

    // Use this for initialization
    void Start()
    {


        _fogColorWater = new Color(0.2f, 0.65f, 0.75f, 0.5f);

        _defFogMode = RenderSettings.fogMode;
        _defFogDensity = RenderSettings.fogDensity;
        _defFogColor = RenderSettings.fogColor;
        _defFogEnabled = RenderSettings.fog;

    }

    private void OnEnable()
    {
        PlayerStatus.Instance.status = PlayerStatus.Status.IsSwimming;
        _fogColorWater = new Color(0.2f, 0.65f, 0.75f, 0.5f);

        _defFogMode = RenderSettings.fogMode;
        _defFogDensity = RenderSettings.fogDensity;
        _defFogColor = RenderSettings.fogColor;
        _defFogEnabled = RenderSettings.fog;
        //Debug.Log(transform.root.gameObject.GetComponent<ActionMapManager>().currentActionMap.ToString());
        swimmingActionMap = transform.root.gameObject.GetComponent<ActionMapManager>().SwitchActionMap("Swimming");
        walkAnimator = transform.root.gameObject.GetComponent<Animator>();
        swimAnimator = transform.root.gameObject.GetComponent<Animator>();
        swimmingActionMap["SwimUp"].performed += SwimUpPerformed;
        swimmingActionMap["SwimUp"].canceled += SwimUpCanceled;

        swimmingActionMap["SwimDown"].performed += SwimDownPerformed;
        swimmingActionMap["SwimDown"].canceled += SwimDownCanceled;
    }


    float swimYDirection;
    bool btnSpace, btnShift;

    private void SwimUpPerformed(InputAction.CallbackContext context)
    {
        swimYDirection = 1; btnSpace = true;
    }

    private void SwimUpCanceled(InputAction.CallbackContext context)
    {
        swimYDirection = 0; btnSpace = false;
    }

    private void SwimDownPerformed(InputAction.CallbackContext context)
    {
        swimYDirection = -1; btnShift = true;
    }

    private void SwimDownCanceled(InputAction.CallbackContext context)
    {
        swimYDirection = 0; btnShift = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        _isInWater = true;
        swimAnimator.SetLayerWeight(3, 1);
    }

    private void OnTriggerExit(Collider other)
    {
        _isInWater = false;

        transform.root.gameObject.GetComponent<ActionMapManager>().SwitchActionMap("Land");
        transform.root.gameObject.GetComponent<Animator>().SetLayerWeight(3, 0);
    }
    // Update is called once per frame
    void Update()
    {
        input = swimmingActionMap["Move"].ReadValue<Vector2>();

        if (IsUnderwater())
        {
            SetRenderDiving();
        }
        else
        {
            SetRenderDefault();
        }

        //Handle Swimming
        if (_isInWater)
        {
                //transform.root.gameObject.GetComponent<Rigidbody>().useGravity = false;
                //transform.root.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                PlayerStatus.Instance.status = PlayerStatus.Status.IsSwimming;
                DoDiving();
        }
        else
        {
            DoWalking();
        }

    }

    void DoDiving()
    {
        swimAnimator.SetLayerWeight(3, 1);
        transform.root.position += new Vector3(input.x, swimYDirection, input.y) * 0.001f;
        swimAnimator.SetFloat("InputX", input.x);
        swimAnimator.SetFloat("InputY", input.y);
        swimAnimator.SetBool("BtnSpace", btnSpace);
        swimAnimator.SetBool("BtnShift", btnShift);
    }
    void DoWalking()
    {
        swimAnimator.SetLayerWeight(3, 0);

        transform.root.position += new Vector3(input.x, 0, input.y);
        walkAnimator.SetFloat("InputX", input.x);
        walkAnimator.SetFloat("InputY", input.y);
    }

    private bool IsUnderwater()
    {
        return Camera.main.transform.position.y <= _waterSurfacePosY;
    }

    private void SetRenderDiving()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = _fogColorWater;
        RenderSettings.fogDensity = 0.1f;
        RenderSettings.fogMode = FogMode.Exponential;
    }

    private void SetRenderDefault()
    {
        RenderSettings.fog = _defFogEnabled;
        RenderSettings.fogColor = _defFogColor;
        RenderSettings.fogDensity = _defFogDensity;
        RenderSettings.fogMode = _defFogMode;
    }
    private void OnDisable()
    {
        input = Vector2.zero;
        swimYDirection = 0;
        btnSpace = false;
        btnShift = false;
        _isInWater = false;
        transform.root.gameObject.GetComponent<Rigidbody>().useGravity = true;
        transform.root.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        RenderSettings.fog = _defFogEnabled;
        RenderSettings.fogColor = _defFogColor;
        RenderSettings.fogDensity = _defFogDensity;
        RenderSettings.fogMode = _defFogMode;
    }
}
