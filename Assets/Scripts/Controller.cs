using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class Controller : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _cameraMovementSpeed;
    [SerializeField] private float _cameraAutomaticRotationSpeed;
    [SerializeField] private float _cameraManualRotationSpeed;
    [SerializeField] private Vector3 _cameraTargetOffset;
    [SerializeField] private float _cameraDeadZoneXAngle;
    [SerializeField] private float _cameraDeadZoneYAngle;
    [SerializeField] private float _cameraMaxDistanceToPlayer;
    [SerializeField] private float _cameraMinDistanceToPlayer;
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

        _camera.transform.position = transform.position + Vector3.forward * _cameraMinDistanceToPlayer + _cameraTargetOffset;
        _camera.transform.LookAt(transform.position + Vector3.back * _cameraMaxDistanceToPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lMoveVector = Vector3.zero;
        Quaternion lCameraRotationY = GetAxisRotation (_camera.transform.rotation, false, true, false);

        if (Input.GetKey(KeyCode.Z))
        {
            lMoveVector += lCameraRotationY * Vector3.forward;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            lMoveVector += lCameraRotationY * Vector3.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            lMoveVector += lCameraRotationY * Vector3.back;
        }
        if (Input.GetKey(KeyCode.D))
        {
            lMoveVector += lCameraRotationY * Vector3.right;
        }
        lMoveVector = lMoveVector.normalized * _playerSpeed * Time.deltaTime;
        lMoveVector.y = 0.0f;
        transform.position += lMoveVector;

        Vector3 lCameraFollow = _camera.transform.position - _cameraTargetOffset;

        RaycastHit wallHit = new RaycastHit ();
        if (Physics.Linecast (transform.position, lCameraFollow, out wallHit, ~_cameraLayerMask))
        {
            lCameraFollow = wallHit.point;
            print("collided wall at " + wallHit.point);
        }

        Quaternion lCameraRotation = _camera.transform.rotation;

        float lDistanceToCameraPlane = DistanceFromPointToLine (transform.position, lCameraFollow, lCameraFollow + lCameraRotation * Vector3.right);

        Vector3 translation = Vector3.zero;
        if (lDistanceToCameraPlane < _cameraMinDistanceToPlayer)
        {
            translation += lCameraRotationY * Vector3.back * (_cameraMinDistanceToPlayer - lDistanceToCameraPlane);
            print("camera too close from target, distance is : " + lDistanceToCameraPlane);
        }
        if (lDistanceToCameraPlane > _cameraMaxDistanceToPlayer)
        {
            translation += lCameraRotationY * Vector3.forward * (lDistanceToCameraPlane - _cameraMaxDistanceToPlayer);
            print ("camera too far from target, distance is : " + lDistanceToCameraPlane);
        }
        translation *= Time.deltaTime * _cameraMovementSpeed;
        _camera.transform.position = Vector3.Lerp (_camera.transform.position, lCameraFollow + translation + _cameraTargetOffset, Time.deltaTime * _cameraMovementSpeed);

        float lXAngleBetweenPlayerAndCamera = _camera.transform.rotation.eulerAngles.x;
        float lYAngleBetweenPlayerAndCamera = Quaternion.FromToRotation (lCameraRotation * Vector3.forward, transform.position - _camera.transform.position).eulerAngles.y;

        if (lXAngleBetweenPlayerAndCamera > 180)
        {
            lXAngleBetweenPlayerAndCamera -= 360;
        }
        if (lYAngleBetweenPlayerAndCamera > 180)
        {
            lYAngleBetweenPlayerAndCamera -= 360;
        }

        Vector3 lCameraFinalRotation = Vector3.zero;

        if (Input.GetKey (KeyCode.UpArrow) && lXAngleBetweenPlayerAndCamera > -_cameraDeadZoneXAngle)
        {
            lCameraFinalRotation += Vector3.left * _cameraDeadZoneXAngle * _cameraManualRotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey (KeyCode.LeftArrow) && lYAngleBetweenPlayerAndCamera < _cameraDeadZoneYAngle)
        {
            lCameraFinalRotation += Vector3.down * _cameraDeadZoneXAngle * _cameraManualRotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey (KeyCode.DownArrow) && lXAngleBetweenPlayerAndCamera < _cameraDeadZoneXAngle)
        {
            lCameraFinalRotation += Vector3.right * _cameraDeadZoneXAngle * _cameraManualRotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey (KeyCode.RightArrow) && lYAngleBetweenPlayerAndCamera > -_cameraDeadZoneYAngle)
        {
            lCameraFinalRotation += Vector3.up * _cameraDeadZoneXAngle * _cameraManualRotationSpeed * Time.deltaTime;
        }

        lYAngleBetweenPlayerAndCamera += lCameraFinalRotation.y;

        if (Math.Abs(lYAngleBetweenPlayerAndCamera) > _cameraDeadZoneYAngle)
        {
            if (lYAngleBetweenPlayerAndCamera > 0)
            {
                lCameraFinalRotation += new Vector3(0, (lYAngleBetweenPlayerAndCamera - _cameraDeadZoneYAngle) * _cameraAutomaticRotationSpeed * Time.deltaTime, 0);
                print("correcting y angle of " + lYAngleBetweenPlayerAndCamera + " degrees.");
            } else {
                lCameraFinalRotation += new Vector3(0, (lYAngleBetweenPlayerAndCamera + _cameraDeadZoneYAngle) * _cameraAutomaticRotationSpeed * Time.deltaTime, 0);
                print ("correcting negative y angle of " + (lYAngleBetweenPlayerAndCamera) + " degrees.");
            }
        }

        lXAngleBetweenPlayerAndCamera += lCameraFinalRotation.x;

        if (Math.Abs (lXAngleBetweenPlayerAndCamera) > _cameraDeadZoneXAngle)
        {
            if (lXAngleBetweenPlayerAndCamera > 0)
            {
                lCameraFinalRotation += new Vector3 ((_cameraDeadZoneXAngle - lXAngleBetweenPlayerAndCamera) * _cameraAutomaticRotationSpeed * Time.deltaTime, 0, 0);
                print ("correcting x angle of " + lXAngleBetweenPlayerAndCamera + " degrees.");
            } else {
                lCameraFinalRotation += new Vector3 ((-_cameraDeadZoneXAngle - lXAngleBetweenPlayerAndCamera) * _cameraAutomaticRotationSpeed * Time.deltaTime, 0, 0);
                print ("correcting negative x angle of " + (lXAngleBetweenPlayerAndCamera) + " degrees.");
            }
        }
        //print("Player is at " + lDistanceToCameraPlane + " from camera plane and turned " + lAbsoluteYAngleBetweenPlayerAndCamera + " degrees horizontally and " + lAbsoluteXAngleBetweenPlayerAndCamera + " degrees vertically.");

        _camera.transform.Rotate(lCameraFinalRotation);
        _camera.transform.rotation = Quaternion.Slerp (_camera.transform.rotation, _camera.transform.rotation * Quaternion.Euler (lCameraFinalRotation), Time.deltaTime * _cameraAutomaticRotationSpeed * 0.25f);
        _camera.transform.Rotate(0, 0, -lCameraRotation.eulerAngles.z);
    }
}