using Rewired;
using Rewired.ControllerExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to communicate with the PS4 controller.
/// Get specific values (acceleration, gyroscope...) and activate special features (lights, vibrations...)
/// </summary>
public class PS4Controller : MonoBehaviour
{
    IDualShock4Extension ds4 = null;
    bool _isFlashOn = true;

    #region Singleton

    static PS4Controller _instance;
    public static PS4Controller Instance {  get { return _instance; } }

    private void Awake()
    {
        if(_instance != null)
        {
            Debug.LogError("Already an instance of " + this.name);
            Destroy(_instance);
        }

        _instance = this;
    }
    #endregion

    Quaternion _rotation;
    public Quaternion rotation { get { return _rotation; } }

    Vector3 _acceleration;
    public Vector3 acceleration { get { return _acceleration; } }

    bool _wasFingerOnTouchpad = false;
    bool _isFingerOnTouchpad = false;

    Vector2 _fingerPositionOnTouchpad;

    /*public bool touch1isTouching { get { return _touch1isTouching; } }
    public bool touch2isTouching { get { return _touch2isTouching; } }*/

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.Instance.AddListener<OnVibrate>(SetVibrate);
        EventsManager.Instance.AddListener<OnLightSwitchColor>(SetRandomLightColor);
        EventsManager.Instance.AddListener<OnLightFlash>(FlashLight);
        EventsManager.Instance.AddListener<OnStopVibrate>(StopVibrate);
    }

    // Update is called once per frame
    void Update()
    {
        if (!ReInput.isReady) return;

        ds4 = GetFirstDS4(Utils_Variables.REWIRED_PLAYER);
        if(ds4 != null)
        {
            HandleTouchpad(ds4);
            GetRotation();
            GetAcceleration();
        }
    }

    #region PS4 Special Functions

    void SetVibrate(OnVibrate e)
    {
        if (ds4 != null)
        {
            if (e.both)
            {
                ds4.SetVibration(1f, 1f);
                return;
            }

            if (!e.right) ds4.SetVibration(0, 1f, 1f);
            else ds4.SetVibration(1, 1f, 1f);
        }
    }

    void StopVibrate(OnStopVibrate e)
    {
        if (ds4 != null)
        {
            ds4.StopVibration();
        }
    }

    private void SetRandomLightColor(OnLightSwitchColor e)
    {
        if (ds4 != null)
        {
            Color color = new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
            );
            ds4.SetLightColor(color);
        }
    }

    private void FlashLight(OnLightFlash e)
    {
        // This is not supported on PS4 so get the Standalone DualShock4Extension
        DualShock4Extension ds4 = GetFirstDS4(Utils_Variables.REWIRED_PLAYER) as DualShock4Extension;

        if (ds4 != null)
        {
            if (_isFlashOn)
            {
                ds4.SetLightFlash(0.5f, 0.5f);
                _isFlashOn = false;
            }

            else
            {
                ds4.StopLightFlash();
                _isFlashOn = true;
            }
        }
    }
    #endregion

    #region PS4 Special Inputs / Variables

    public bool IsTouchpadPressed()
    {
        return !_wasFingerOnTouchpad && _isFingerOnTouchpad;
    }

    public bool IsTouchpadReleased()
    {
        return _wasFingerOnTouchpad && !_isFingerOnTouchpad;
    }

    public Vector2 GetFingerPositionOnTouchpad()
    {
        return _fingerPositionOnTouchpad;
    }

    void HandleTouchpad(IDualShock4Extension ds4)
    {
        /*for(int i = 0; i < ds4.maxTouches; i++)
        {
            if (!ds4.IsTouching(i)) continue;

            Vector2 position;
            ds4.GetTouchPosition(i, out position);

            if (i == 0) _firstTouchPostion = position;
            else if (i == 1) _secondTouchPosition = position;
        }*/


        /*_exTouch1isTouching = ds4.GetTouchPosition(0, out _firstTouchPostion);
        if (_touch1isTouching != _exTouch1isTouching)
        {
            _touch1isTouching = _exTouch1isTouching;
            if (_touch1isTouching) EventsManager.Instance.Raise(new OnTouch());
            else EventsManager.Instance.Raise(new OnReleaseTouch());
        }
             
        //We won't need a second touch
        _touch2isTouching = ds4.GetTouchPosition(1, out _secondTouchPosition);*/

        _wasFingerOnTouchpad = _isFingerOnTouchpad;
        _isFingerOnTouchpad = ds4.IsTouching(0);
        if (_isFingerOnTouchpad)
        {
            ds4.GetTouchPosition(0, out _fingerPositionOnTouchpad);
        }
    }

    void GetRotation()
    {
        // Set the model's rotation to match the controller's
        _rotation = ds4.GetOrientation();
    }

    void GetAcceleration()
    {
        _acceleration = ds4.GetAccelerometerValue();
    }

    #endregion

    private IDualShock4Extension GetFirstDS4(Player player)
    {
        foreach (Joystick j in player.controllers.Joysticks)
        {
            // Use the interface because it works for both PS4 and desktop platforms
            IDualShock4Extension ds4 = j.GetExtension<IDualShock4Extension>();
            if (ds4 == null) continue;
            return ds4;
        }
        return null;
    }

    private void OnDestroy()
    {
        EventsManager.Instance.RemoveListener<OnVibrate>(SetVibrate);
        EventsManager.Instance.RemoveListener<OnLightSwitchColor>(SetRandomLightColor);
        EventsManager.Instance.RemoveListener<OnLightFlash>(FlashLight);
        EventsManager.Instance.RemoveListener<OnStopVibrate>(StopVibrate);
    }
}
