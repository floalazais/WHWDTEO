using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] private float _speed;
    [SerializeField] private float _cameraAutomaticRotationSpeed;
    [SerializeField] private float _cameraManualRotationSpeed;
    [SerializeField] private Vector3 _cameraTargetOffset;
    [SerializeField] private float _cameraDeadZoneXAngle;
    [SerializeField] private float _cameraDeadZoneYAngle;
    [SerializeField] private float _cameraMaxDistanceToPlayer;
    [SerializeField] private float _cameraMinDistanceToPlayer;
    [SerializeField] private Transform _cameraFollow;
    [SerializeField] private Transform _cameraLookAt;
    [SerializeField] private LayerMask _cameraLayerMask;

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

        _cameraFollow.position = transform.position + Vector3.forward * _cameraMinDistanceToPlayer;
        _cameraLookAt.position = transform.position + Vector3.back * _cameraMaxDistanceToPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        _camera.transform.position -= _cameraTargetOffset;

        Vector3 lMoveVector = Vector3.zero;
        Quaternion lCameraRotationX = GetAxisRotation (_camera.transform.rotation, true, false, false);
        Quaternion lCameraRotationY = GetAxisRotation (_camera.transform.rotation, false, true, false);
        Quaternion lCameraRotation = _camera.transform.rotation;

        bool blockMoveAndRotate = false;

        if (Input.GetKey(KeyCode.Z))
        {
            lMoveVector += lCameraRotationY * Vector3.forward;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            lMoveVector += lCameraRotationY * Vector3.left;
            blockMoveAndRotate = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            lMoveVector += lCameraRotationY * Vector3.back;
        }
        if (Input.GetKey(KeyCode.D))
        {
            lMoveVector += lCameraRotationY * Vector3.right;
            blockMoveAndRotate = true;
        }
        lMoveVector = lMoveVector.normalized * _speed * Time.deltaTime;
        
        if (Input.GetKey (KeyCode.UpArrow) && !blockMoveAndRotate)
        {
            //_cameraLookAt.position = RotatePointAroundPivot(_cameraLookAt.position, _cameraFollow.position, lCameraRotation * Vector3.left * _cameraDeadZoneXAngle * _cameraManualRotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey (KeyCode.LeftArrow))
        {
            _cameraLookAt.position = RotatePointAroundPivot (_cameraLookAt.position, _cameraFollow.position, Vector3.down * _cameraDeadZoneYAngle * _cameraManualRotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey (KeyCode.DownArrow) && !blockMoveAndRotate)
        {
            //_cameraLookAt.position = RotatePointAroundPivot (_cameraLookAt.position, _cameraFollow.position, lCameraRotation * Vector3.right * _cameraDeadZoneXAngle * _cameraManualRotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey (KeyCode.RightArrow))
        {
            _cameraLookAt.position = RotatePointAroundPivot (_cameraLookAt.position, _cameraFollow.position, Vector3.up * _cameraDeadZoneYAngle * _cameraManualRotationSpeed * Time.deltaTime);
        }

        float lDistanceToCameraPlane = DistanceFromPointToLine (transform.position, _cameraFollow.position, _cameraFollow.position + lCameraRotation * Vector3.right);
        Vector3 lTargetVectorXZ = transform.position - _cameraFollow.position;
        lTargetVectorXZ.y = 0;
        float lYAngleBetweenPlayerAndCamera = Quaternion.FromToRotation (lCameraRotation * Vector3.forward, lTargetVectorXZ).eulerAngles.y;
        float lAbsoluteYAngleBetweenPlayerAndCamera = Math.Abs(Math.Abs(lYAngleBetweenPlayerAndCamera - 180) - 180);

        if (Math.Abs(lAbsoluteYAngleBetweenPlayerAndCamera) > _cameraDeadZoneYAngle)
        {
            if (lYAngleBetweenPlayerAndCamera < 180)
            {
                _cameraLookAt.position = RotatePointAroundPivot(_cameraLookAt.position, _cameraFollow.position, new Vector3(0, lYAngleBetweenPlayerAndCamera * _cameraAutomaticRotationSpeed * Time.deltaTime, 0));
                //print("correcting y angle of " + lYAngleBetweenPlayerAndCamera + " degrees.");
            } else {
                _cameraLookAt.position = RotatePointAroundPivot(_cameraLookAt.position, _cameraFollow.position, new Vector3(0, (lYAngleBetweenPlayerAndCamera - 360) * _cameraAutomaticRotationSpeed * Time.deltaTime, 0));
                //print("correcting y angle of " + (lYAngleBetweenPlayerAndCamera - 360) + " degrees.");
            }
        }

        Vector3 lTargetVectorYZ = transform.position - _cameraFollow.position;
        lTargetVectorYZ.x = 0;
        float lXAngleBetweenPlayerAndCamera = Quaternion.FromToRotation (lCameraRotationX * Vector3.forward, lTargetVectorYZ).eulerAngles.x;
        float lAbsoluteXAngleBetweenPlayerAndCamera = Math.Abs (Math.Abs (lXAngleBetweenPlayerAndCamera - 180) - 180);

        if (Math.Abs (lAbsoluteXAngleBetweenPlayerAndCamera) > _cameraDeadZoneXAngle)
        {
            if (lXAngleBetweenPlayerAndCamera < 180)
            {
                _cameraLookAt.position = RotatePointAroundPivot (_cameraLookAt.position, _cameraFollow.position, new Vector3 (lXAngleBetweenPlayerAndCamera * _cameraAutomaticRotationSpeed * Time.deltaTime, 0, 0));
                print ("correcting x angle of " + lXAngleBetweenPlayerAndCamera + " degrees.");
            } else {
                _cameraLookAt.position = RotatePointAroundPivot (_cameraLookAt.position, _cameraFollow.position, new Vector3 ((360 - lXAngleBetweenPlayerAndCamera) * _cameraAutomaticRotationSpeed * Time.deltaTime, 0, 0));
                print ("correcting negative x angle of " + (360 - lXAngleBetweenPlayerAndCamera) + " degrees.");
            }
        }
        //print("Player is at " + lDistanceToCameraPlane + " from camera plane and turned " + lAbsoluteYAngleBetweenPlayerAndCamera + " degrees horizontally and " + lAbsoluteXAngleBetweenPlayerAndCamera + " degrees vertically.");

        RaycastHit wallHit = new RaycastHit ();
        if (Physics.Linecast (transform.position, _cameraFollow.position, out wallHit, ~_cameraLayerMask))
        {
            Vector3 translation = wallHit.point - _cameraFollow.position;
            _cameraFollow.position += translation;
            _cameraLookAt.position += translation;
        }

        if (lDistanceToCameraPlane < _cameraMinDistanceToPlayer)
        {
            _cameraFollow.position += lCameraRotationY * Vector3.back * _speed * Time.deltaTime;
            _cameraLookAt.position += lCameraRotationY * Vector3.back * _speed * Time.deltaTime;
        }
        if (lDistanceToCameraPlane > _cameraMaxDistanceToPlayer)
        {
            _cameraFollow.position += lCameraRotationY * Vector3.forward * _speed * Time.deltaTime;
            _cameraLookAt.position += lCameraRotationY * Vector3.forward * _speed * Time.deltaTime;
        }

        lMoveVector.y = 0.0f;
        transform.position += lMoveVector;

        if (_cameraLookAt.position.y - _cameraFollow.position.y > 1)
        {
            _cameraLookAt.position = new Vector3(_cameraLookAt.position.x, _cameraFollow.position.y + 1, _cameraLookAt.position.z);
        } else if (_cameraLookAt.position.y - _cameraFollow.position.y < -1) {
            _cameraLookAt.position = new Vector3(_cameraLookAt.position.x, _cameraFollow.position.y - 1, _cameraLookAt.position.z);
        }

        _camera.transform.position = Vector3.Lerp(_camera.transform.position, _cameraFollow.position + _cameraTargetOffset, _speed);
        _camera.transform.rotation = Quaternion.Slerp (_camera.transform.rotation, Quaternion.LookRotation (_cameraLookAt.position - _camera.transform.position), _cameraAutomaticRotationSpeed * Time.deltaTime);
        //_camera.transform.LookAt(_cameraLookAt);
    }
}