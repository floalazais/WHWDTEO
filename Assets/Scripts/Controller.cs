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

    Vector3 lCameraOffset = Vector3.zero;
    Vector3 lerpLookAt = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        RaycastHit wallHit = new RaycastHit ();
        
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
        lMoveVector.y = 0.0f;
        lMoveVector = lMoveVector.normalized * _playerSpeed * Time.deltaTime;
        if (Physics.Linecast(transform.position, transform.position + lMoveVector, out wallHit, ~_cameraLayerMask))
        {
            transform.position = wallHit.point;
        }
        transform.position += lMoveVector;

        Vector3 lCameraLookAt = transform.position + lCameraRotationY * Vector3.right * _cameraRightDistanceToPlayer + Vector3.up * _cameraUpDistanceToPlayer;

        if (Input.GetKey (KeyCode.UpArrow) && _camera.transform.position.y < _cameraMaxHeight + transform.position.y)
        {
            lCameraOffset += Vector3.up * _cameraVerticalRotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey (KeyCode.LeftArrow))
        {
            lCameraLookAt = RotatePointAroundPivot (lCameraLookAt, transform.position, Vector3.down * _cameraHorizontalRotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey (KeyCode.DownArrow) && _camera.transform.position.y > _cameraMinHeight + transform.position.y)
        {
            lCameraOffset += Vector3.down * _cameraVerticalRotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey (KeyCode.RightArrow))
        {
            lCameraLookAt = RotatePointAroundPivot (lCameraLookAt, transform.position, Vector3.up * _cameraHorizontalRotationSpeed * Time.deltaTime);
        }

        if (!Input.GetKey (KeyCode.UpArrow) && !Input.GetKey (KeyCode.DownArrow))
        {
            lCameraOffset.y *= 0.75f;
        }

        Quaternion lNewCameraRotation = GetAxisRotation (Quaternion.LookRotation (transform.position - lCameraLookAt), false, true, false) * Quaternion.Euler (0, 90, 0);

        Vector3 lLineCastCameraPoint = lCameraLookAt;
        lLineCastCameraPoint.y = transform.position.y;

        if (Physics.Linecast (transform.position, lLineCastCameraPoint, out wallHit, ~_cameraLayerMask))
        {
            lCameraLookAt += lCameraRotationY * Vector3.left * Mathf.Clamp(_cameraRightDistanceToPlayer - wallHit.distance, 0.1f, _cameraRightDistanceToPlayer);
        }

        Vector3 lCameraFollow = lCameraLookAt + lNewCameraRotation * Vector3.back * _cameraBackDistanceToPlayer + lCameraOffset;

        lLineCastCameraPoint = lCameraFollow;
        lLineCastCameraPoint.y = lCameraLookAt.y;

        float lCameraHeightAfterCollision;
        if (Physics.Linecast (lCameraLookAt, lLineCastCameraPoint, out wallHit, ~_cameraLayerMask))
        {
            lCameraHeightAfterCollision = (Mathf.Clamp (_cameraBackDistanceToPlayer - wallHit.distance, 0.0f, _cameraBackDistanceToPlayer) / _cameraBackDistanceToPlayer) * (_cameraMaxHeight + transform.position.y);
            lCameraFollow += lNewCameraRotation * Vector3.forward * (_cameraBackDistanceToPlayer - wallHit.distance) + Vector3.up * lCameraHeightAfterCollision;
            lCameraLookAt.y = lCameraFollow.y;
        } else {
            lLineCastCameraPoint = transform.position + lNewCameraRotation * Vector3.right * _cameraBackDistanceToPlayer;
            lLineCastCameraPoint.y = transform.position.y;
            if (Physics.Linecast(transform.position, lLineCastCameraPoint, out wallHit, ~_cameraLayerMask))
            {
                float rate = Mathf.Clamp (_cameraBackDistanceToPlayer - wallHit.distance, 0.0f, _cameraBackDistanceToPlayer) / _cameraBackDistanceToPlayer;
                lCameraHeightAfterCollision = rate * (_cameraMaxHeight + transform.position.y);
                lCameraFollow += lNewCameraRotation * Vector3.forward * rate * _cameraBackDistanceToPlayer + Vector3.up * lCameraHeightAfterCollision;
                lCameraLookAt.y = lCameraFollow.y;
            }
        }

        //lerpLookAt = new Vector3 (Mathf.Lerp (lerpLookAt.x, lCameraLookAt.x, Time.deltaTime * _cameraMovementSpeed), Mathf.Lerp (lerpLookAt.y, lCameraLookAt.y, Time.deltaTime * _cameraMovementSpeed / 100), Mathf.Lerp (lerpLookAt.z, lCameraLookAt.z, Time.deltaTime * _cameraMovementSpeed));

        _camera.transform.position = Vector3.Lerp (_camera.transform.position, lCameraFollow, Time.deltaTime * _cameraMovementSpeed);
        //_camera.transform.position = new Vector3(Mathf.Lerp(_camera.transform.position.x, lCameraFollow.x, Time.deltaTime * _cameraMovementSpeed), Mathf.Lerp (_camera.transform.position.y, lCameraFollow.y, Time.deltaTime * _cameraMovementSpeed / 100), Mathf.Lerp (_camera.transform.position.z, lCameraFollow.z, Time.deltaTime * _cameraMovementSpeed));
        _camera.transform.rotation = Quaternion.Slerp (_camera.transform.rotation, Quaternion.LookRotation (lCameraLookAt - _camera.transform.position), Time.deltaTime * _cameraMovementSpeed);
        _camera.transform.Rotate (0, 0, -_camera.transform.rotation.eulerAngles.z);

        _lookAt.position = lCameraLookAt;
        _follow.position = lCameraFollow;
    }

    void FixedUpdate()
    {
        ;
    }
}