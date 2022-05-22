using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwimmingLink : MonoBehaviour
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
    void Start () {


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
        swimmingActionMap = GetComponent<ActionMapManager>().SwitchActionMap("Swimming");
        walkAnimator = GetComponent<Animator>();
        swimAnimator = GetComponent<Animator>();

    }


    float swimUp;
    float swimDown;

    // Update is called once per frame
    void Update () {
        input = swimmingActionMap["Move"].ReadValue<Vector2>();
        if (swimmingActionMap["SwimUp"].triggered) swimUp = 1;
        if (swimmingActionMap["SwimDown"].triggered) swimDown = -1;


        if (IsUnderwater())
        {
            SetRenderDiving();
        }
        else
        {
            SetRenderDefault();
        }

        //Handle Swimming
        if(_isInWater)
        {
            if(IsUnderwater())
            {
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Rigidbody>().isKinematic = true;
                swimAnimator.SetLayerWeight(3, 1);
                PlayerStatus.Instance.status = PlayerStatus.Status.IsSwimming;
                DoDiving();
            }
            else
            {
                swimAnimator.SetLayerWeight(3, 0);

                DoWalking();
            }
        }
        else
        {
            swimAnimator.SetLayerWeight(3, 0);
            DoWalking();
        }

    }

    void DoDiving()
    {
        transform.position += new Vector3(input.x, swimUp, input.y);
        swimAnimator.SetFloat("InputX", input.x);
        swimAnimator.SetFloat("InputY", input.y);
    }
    void DoWalking()
    {
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

        RenderSettings.fog = _defFogEnabled;
        RenderSettings.fogColor = _defFogColor;
        RenderSettings.fogDensity = _defFogDensity;
        RenderSettings.fogMode = _defFogMode;
    }
}