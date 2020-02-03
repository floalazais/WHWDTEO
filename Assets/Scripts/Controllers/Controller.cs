using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Experimental.Rendering.HDPipeline;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    private Camera _camera;

    private Rigidbody _rigidbody;

    bool _onCarpet = false;

    [SerializeField] private float _joystickDeadZone = 0.1f;
    [SerializeField] private float _CharacterSpeed = 2.0f;
    [SerializeField] private float _CharacterRotationSpeed = 5.0f;
    [SerializeField] private float _CharacterAnimationSpeed = 1.0f;
    [SerializeField] private float _cameraMovementSpeed = 100.0f;
    [SerializeField] private float _cameraVerticalRotationSpeed = 5.0f;
    [SerializeField] private float _cameraHorizontalRotationSpeed = 250.0f;
    [SerializeField] private float _cameraMinHeight = 0.5f;
    [SerializeField] private float _cameraMaxHeight = 2.3f;
    [SerializeField] private float _cameraBackDistanceToPlayer = 1.0f;
    [SerializeField] private float _cameraRightDistanceToPlayer = 0.01f;
    [SerializeField] private float _cameraUpDistanceToPlayer = 1.5f;
    [SerializeField] private LayerMask _cameraLayerMask = 0;

    [SerializeField] private Transform _lookAt = null;
    [SerializeField] private Transform _follow = null;
    [SerializeField] private Transform _floorRaycast = null;

    Vector3 wallHitPosition;

    float DistanceFromPointToLine(Vector3 point, Vector3 linePointA, Vector3 linePointB)
    {
        return Math.Abs((linePointB.z - linePointA.z) * point.x - (linePointB.x - linePointA.x) * point.z + linePointB.x * linePointA.z - linePointB.z * linePointA.x) / (linePointB - linePointA).magnitude;
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 rotationEulerAngles)
    {
        return pivot + Quaternion.Euler (rotationEulerAngles) * (point - pivot);
    }

    Quaternion GetAxisRotation(Quaternion initialRotation, bool xAxis, bool yAxis, bool zAxis)
    {
        return Quaternion.Euler (xAxis ? initialRotation.eulerAngles.x : 0, yAxis ? initialRotation.eulerAngles.y : 0, zAxis ? initialRotation.eulerAngles.z : 0);
    }

    public static Controller instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("INSTANCE ALREADY CREATED " + name);
            Destroy(instance);
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        if (_camera == null)
        {
            Debug.LogError("could not get main camera.");
        }

        _rigidbody = GetComponent<Rigidbody>();
    }

    Vector3 _cameraOffset = Vector3.zero;

    Vector3 _moveVector = Vector3.zero;

    float _blendValue = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (_blendValue < 0.025f)
        {
            _blendValue = 0.0f;
        }
        else if (_blendValue > 0.975f)
        {
            _blendValue = 1.0f;
        }

        if (GameManager.instance.state != Enums.E_GAMESTATE.EXPLORATION)
        {
            _blendValue = Mathf.Lerp(_blendValue, 0.0f, Time.deltaTime * _CharacterAnimationSpeed);
            GetComponent<Animator>().SetFloat("Blend", _blendValue);
            AkSoundEngine.SetRTPCValue("Volume_Presence", _blendValue * 100);
            AkSoundEngine.SetRTPCValue("Blend_Idle_Walk", _blendValue);
            return;
        }

        /* --- Input --- */

        Vector2 leftJoystick = new Vector2(InputManager.instance.leftHorizontalAxis, InputManager.instance.leftVerticalAxis );
        Vector2 rightJoystick = new Vector2(InputManager.instance.rightHorizontalAxis, InputManager.instance.rightVerticalAxis );

        if (Mathf.Abs (leftJoystick.x) < _joystickDeadZone)
        {
            leftJoystick.x = 0.0f;
        }
        if (Mathf.Abs (leftJoystick.y) < _joystickDeadZone)
        {
            leftJoystick.y = 0.0f;
        }
        if (Mathf.Abs (rightJoystick.x) < _joystickDeadZone)
        {
            rightJoystick.x = 0.0f;
        }
        if (Mathf.Abs (rightJoystick.y) < _joystickDeadZone)
        {
            rightJoystick.y = 0.0f;
        }

        /* --- Movement --- */

        RaycastHit wallHit = new RaycastHit ();

        if (Physics.Linecast(_floorRaycast.position, _floorRaycast.position + Vector3.down, out wallHit))
        {
            _onCarpet = wallHit.collider.tag == "Carpet";
        }

        _moveVector = Vector3.zero;
        Quaternion lCameraRotationY = GetAxisRotation (_camera.transform.rotation, false, true, false);

        if (Mathf.Abs(leftJoystick.x) >= _joystickDeadZone || Mathf.Abs(leftJoystick.y) >= _joystickDeadZone)
        {
            _moveVector += lCameraRotationY * (Vector3.forward * leftJoystick.y + Vector3.right * leftJoystick.x);
            transform.rotation = Quaternion.Slerp (transform.rotation, lCameraRotationY * Quaternion.LookRotation (new Vector3 (leftJoystick.x, 0, leftJoystick.y)), _CharacterRotationSpeed * Time.deltaTime);
            _moveVector.y = 0.0f;
            _moveVector = _moveVector.normalized * _CharacterSpeed * Time.deltaTime;
            
            if (Physics.SphereCast(transform.position, 0.3f, _moveVector, out wallHit, _CharacterSpeed * Time.deltaTime, ~_cameraLayerMask))
            {
                _blendValue = Mathf.Lerp(_blendValue, 0.0f, Time.deltaTime * _CharacterAnimationSpeed);
            } else {
                _blendValue = Mathf.Lerp(_blendValue, 1.0f, Time.deltaTime * _CharacterAnimationSpeed);
                transform.position += _moveVector;
                //SoundManager.instance.LaunchEvent(Utils_Variables.MOVEMENT_PLAYER_SOUND);
            }
        } else {
            _blendValue = Mathf.Lerp(_blendValue, 0.0f, Time.deltaTime * _CharacterAnimationSpeed);
        }
        GetComponent<Animator>().SetFloat("Blend", _blendValue);
        AkSoundEngine.SetRTPCValue("Volume_Presence", _blendValue * 100);
        AkSoundEngine.SetRTPCValue("Blend_Idle_Walk", _blendValue);

        /* --- Camera --- */

        /* Camera control */

        Vector3 lCameraLookAt = transform.position + lCameraRotationY * Vector3.right * _cameraRightDistanceToPlayer + Vector3.up * _cameraUpDistanceToPlayer;

        if (rightJoystick.y >= _joystickDeadZone && _camera.transform.position.y > _cameraMinHeight + _lookAt.position.y - _cameraUpDistanceToPlayer)
        {
            _cameraOffset += Vector3.up * -rightJoystick.y * _cameraVerticalRotationSpeed * Time.deltaTime;
        }
        if (rightJoystick.x <= -_joystickDeadZone)
        {
            lCameraLookAt = RotatePointAroundPivot (lCameraLookAt, transform.position, Vector3.up * rightJoystick.x * _cameraHorizontalRotationSpeed * Time.deltaTime);
        }
        if (rightJoystick.y <= -_joystickDeadZone && _camera.transform.position.y < _cameraMaxHeight + _lookAt.position.y - _cameraUpDistanceToPlayer)
        {
            _cameraOffset += Vector3.up * -rightJoystick.y * _cameraVerticalRotationSpeed * Time.deltaTime;
        }
        if (rightJoystick.x >= _joystickDeadZone)
        {
            lCameraLookAt = RotatePointAroundPivot (lCameraLookAt, transform.position, Vector3.up * rightJoystick.x * _cameraHorizontalRotationSpeed * Time.deltaTime);
        }

        /* Camera collisions */

        Quaternion lNewCameraRotation = GetAxisRotation (Quaternion.LookRotation (transform.position - lCameraLookAt), false, true, false) * Quaternion.Euler (0, 90, 0);

        Vector3 lLineCastCameraPoint = transform.position + lNewCameraRotation * Vector3.right * _cameraRightDistanceToPlayer + Vector3.up * _cameraUpDistanceToPlayer;

        if (Physics.Linecast (transform.position + Vector3.up * _cameraUpDistanceToPlayer, lLineCastCameraPoint, out wallHit, ~_cameraLayerMask))
        {
            wallHitPosition = wallHit.point;

            lCameraLookAt += lNewCameraRotation * Vector3.left * Mathf.Clamp((transform.position + Vector3.up * _cameraUpDistanceToPlayer - lCameraLookAt).magnitude - wallHit.distance + 0.1f, 0.0f, _cameraRightDistanceToPlayer);
        }

        Vector3 lCameraFollow = lCameraLookAt + lNewCameraRotation * Vector3.back * _cameraBackDistanceToPlayer + _cameraOffset;
        lNewCameraRotation = GetAxisRotation(Quaternion.LookRotation(lCameraLookAt - lCameraFollow), true, true, false);

        lLineCastCameraPoint = lCameraFollow;
        
        if (Physics.Linecast (lCameraLookAt, lLineCastCameraPoint, out wallHit, ~_cameraLayerMask))
        {
            wallHitPosition = wallHit.point;
            
            lCameraFollow += lNewCameraRotation * Vector3.forward * Mathf.Clamp((lCameraLookAt - lCameraFollow).magnitude - wallHit.distance + 0.1f, 0.0f, _cameraBackDistanceToPlayer);
            lCameraLookAt = lCameraFollow + lNewCameraRotation * Vector3.forward * _cameraBackDistanceToPlayer;
        }

        _lookAt.position = Vector3.Lerp(_lookAt.position, lCameraLookAt, _cameraMovementSpeed * Time.deltaTime);
        _follow.position = Vector3.Lerp(_follow.position, lCameraFollow, _cameraMovementSpeed * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + Vector3.up * _cameraUpDistanceToPlayer, _lookAt.position);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_follow.position, _lookAt.position);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(wallHitPosition, 0.01f);
    }

    public void FootStepEvent()
    {
        if (GameManager.instance.state != Enums.E_GAMESTATE.EXPLORATION) return;

        if (_blendValue < 0.1f) return;

        if (SceneManager.GetActiveScene().name == "TestHandsScene")
        {
            SoundManager.instance.PlaySound(Utils_Variables.STEP_VOID_SOUND);
            return;
        }

        if (_onCarpet)
        {
            SoundManager.instance.PlaySound(Utils_Variables.STEP_TAPIS_SOUND);
        }
        else
        {
            SoundManager.instance.PlaySound(Utils_Variables.STEP_PARQUET_SOUND);
        }
    }

    public void ClothPresenceEvent()
    {
        if (SceneManager.GetActiveScene().name == "TestHandsScene") return;

        if (GameManager.instance.state != Enums.E_GAMESTATE.EXPLORATION) return;

        if (_blendValue < 0.1f) return;

        SoundManager.instance.PlaySound(Utils_Variables.MOVEMENT_PLAYER_SOUND);
    }

    public void IdleClothPresenceEvent()
    {
        if (SceneManager.GetActiveScene().name == "TestHandsScene") return;

        if (GameManager.instance.state != Enums.E_GAMESTATE.EXPLORATION) return;

        if (_blendValue > 0.0f) return;

        SoundManager.instance.PlaySound(Utils_Variables.MOVEMENT_IDLE_SOUND);
    }

    public void BreathPresenceEvent()
    {
        if (SceneManager.GetActiveScene().name == "TestHandsScene") return;

        if (GameManager.instance.state != Enums.E_GAMESTATE.EXPLORATION) return;

        if (_blendValue > 0.0f) return;

        SoundManager.instance.PlaySound(Utils_Variables.BREATH_IDLE_SOUND);
    }
}