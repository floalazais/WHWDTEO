﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class Controller : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] private float _joystickDeadZone;
    [SerializeField] private float _CharacterSpeed;
    [SerializeField] private float _CharacterRotationSpeed;
    [SerializeField] private float _cameraMovementSpeed;
    [SerializeField] private float _cameraVerticalRotationSpeed;
    [SerializeField] private float _cameraHorizontalRotationSpeed;
    [SerializeField] private float _cameraMinHeight;
    [SerializeField] private float _cameraMaxHeight;
    [SerializeField] private float _cameraBackDistanceToPlayer;
    [SerializeField] private float _cameraRightDistanceToPlayer;
    [SerializeField] private float _cameraUpDistanceToPlayer;
    [SerializeField] private LayerMask _cameraLayerMask;

    [SerializeField] private Transform _lookAt;
    [SerializeField] private Transform _follow;

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

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        if (_camera == null)
        {
            Debug.LogError("could not get main camera.");
        }

        _camera.transform.position = transform.position + Vector3.right * _cameraRightDistanceToPlayer;
        _camera.transform.LookAt(_camera.transform.position + Vector3.back * _cameraBackDistanceToPlayer);
    }

    enum Direction { Front, Left, Back, Right};

    Direction _characterRotation = Direction.Front;

    Vector3 lCameraOffset = Vector3.zero;
    Vector3 lerpLookAt = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.state != Enums.E_GAMESTATE.PLAY) return;

        RaycastHit wallHit = new RaycastHit ();
        
        Vector3 lMoveVector = Vector3.zero;
        Quaternion lCameraRotationY = GetAxisRotation (_camera.transform.rotation, false, true, false);

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

        if (Mathf.Abs(leftJoystick.x) >= _joystickDeadZone || Mathf.Abs(leftJoystick.y) >= _joystickDeadZone)
        {
            lMoveVector += lCameraRotationY * (Vector3.forward * leftJoystick.y + Vector3.right * leftJoystick.x);
            transform.rotation = Quaternion.Slerp (transform.rotation, lCameraRotationY * Quaternion.LookRotation (new Vector3 (-leftJoystick.x, 0, -leftJoystick.y)), _CharacterRotationSpeed * Time.deltaTime);
        }

        lMoveVector.y = 0.0f;
        lMoveVector = lMoveVector.normalized * _CharacterSpeed * Time.deltaTime;

        //TO-DO : Refaire les collisions avec le personnage
        if (Physics.Linecast(transform.position, transform.position + lMoveVector, out wallHit, ~_cameraLayerMask))
        {
            //transform.position = wallHit.point;
        } else {
            transform.position += lMoveVector;
        }
        

        Vector3 lCameraLookAt = transform.position + lCameraRotationY * Vector3.right * _cameraRightDistanceToPlayer + Vector3.up * _cameraUpDistanceToPlayer;

        if (rightJoystick.y <= -_joystickDeadZone && _camera.transform.position.y > _cameraMinHeight + transform.position.y)
        {
            lCameraOffset += Vector3.up * rightJoystick.y * _cameraVerticalRotationSpeed * Time.deltaTime;
        }
        if (rightJoystick.x <= -_joystickDeadZone)
        {
            lCameraLookAt = RotatePointAroundPivot (lCameraLookAt, transform.position, Vector3.up * rightJoystick.x * _cameraHorizontalRotationSpeed * Time.deltaTime);
        }
        if (rightJoystick.y >= _joystickDeadZone && _camera.transform.position.y < _cameraMaxHeight + transform.position.y)
        {
            lCameraOffset += Vector3.up * rightJoystick.y * _cameraVerticalRotationSpeed * Time.deltaTime;
        }
        if (rightJoystick.x >= _joystickDeadZone)
        {
            lCameraLookAt = RotatePointAroundPivot (lCameraLookAt, transform.position, Vector3.up * rightJoystick.x * _cameraHorizontalRotationSpeed * Time.deltaTime);
        }

        if (Mathf.Abs(rightJoystick.y) < _joystickDeadZone)
        {
            //lCameraOffset.y *= 0.75f;
        }

        Quaternion lNewCameraRotation = GetAxisRotation (Quaternion.LookRotation (transform.position - lCameraLookAt), false, true, false) * Quaternion.Euler (0, 90, 0);

        Vector3 lLineCastCameraPoint = transform.position + lNewCameraRotation * Vector3.right * _cameraBackDistanceToPlayer;
        lLineCastCameraPoint.y = transform.position.y;

        float lCameraHeightAfterCollision;
        if (Physics.Linecast (transform.position, lLineCastCameraPoint, out wallHit, ~_cameraLayerMask))
        {
            lCameraLookAt += lCameraRotationY * Vector3.left * Mathf.Clamp(_cameraRightDistanceToPlayer - wallHit.distance, 0.0f, _cameraRightDistanceToPlayer);

            float rate = Mathf.Clamp (_cameraBackDistanceToPlayer - wallHit.distance, 0.0f, _cameraBackDistanceToPlayer) / _cameraBackDistanceToPlayer;
            lCameraHeightAfterCollision = rate * (_cameraMaxHeight + transform.position.y);
            lCameraLookAt += Vector3.up * lCameraHeightAfterCollision;
            lCameraOffset.y = 0.0f;
        }

        Vector3 lCameraFollow = lCameraLookAt + lNewCameraRotation * Vector3.back * _cameraBackDistanceToPlayer + lCameraOffset;

        lLineCastCameraPoint = lCameraFollow;
        lLineCastCameraPoint.y = lCameraLookAt.y;

        if (Physics.Linecast (lCameraLookAt, lLineCastCameraPoint, out wallHit, ~_cameraLayerMask))
        {
            lCameraHeightAfterCollision = (Mathf.Clamp (_cameraBackDistanceToPlayer - wallHit.distance, 0.0f, _cameraBackDistanceToPlayer) / _cameraBackDistanceToPlayer) * (_cameraMaxHeight + transform.position.y);
            lCameraFollow.y -= lCameraOffset.y;
            lCameraOffset.y = 0.0f;
            lCameraFollow += lNewCameraRotation * Vector3.forward * (_cameraBackDistanceToPlayer - wallHit.distance + 0.1f) + Vector3.up * lCameraHeightAfterCollision;
            lCameraLookAt.y = lCameraFollow.y;
        }

        _lookAt.position = Vector3.Lerp(_lookAt.position, lCameraLookAt, _cameraMovementSpeed * Time.deltaTime);
        _follow.position = Vector3.Lerp(_follow.position, lCameraFollow, _cameraMovementSpeed * Time.deltaTime);

        _camera.transform.position = _follow.position;
        //_camera.transform.rotation = Quaternion.Slerp (_camera.transform.rotation, Quaternion.LookRotation (_lookAt.position - _camera.transform.position), Time.deltaTime * _cameraMovementSpeed);
        _camera.transform.rotation = Quaternion.LookRotation (_lookAt.position - _camera.transform.position);
        _camera.transform.Rotate (0, 0, -_camera.transform.rotation.eulerAngles.z);
    }
}