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

    // Update is called once per frame
    void Update()
    {
        lCameraOffset.x = 0.0f;
        lCameraOffset.z = 0.0f;
        Vector3 lMoveVector = Vector3.zero;
        Quaternion lCameraRotationY = GetAxisRotation (_camera.transform.rotation, false, true, false);

        float oldYCameraOffset = lCameraOffset.y;
        if (Input.GetKey(KeyCode.Z))
        {
            lMoveVector += lCameraRotationY * Vector3.forward;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            lMoveVector += lCameraRotationY * Vector3.left;
            lCameraOffset += lCameraRotationY * Vector3.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            lMoveVector += lCameraRotationY * Vector3.back;
        }
        if (Input.GetKey(KeyCode.D))
        {
            lMoveVector += lCameraRotationY * Vector3.right;
            lCameraOffset += lCameraRotationY * Vector3.right;
        }
        lMoveVector.y = 0.0f;
        lMoveVector = lMoveVector.normalized * _playerSpeed * Time.deltaTime;
        transform.position += lMoveVector;
        lCameraOffset.y = 0.0f;
        lCameraOffset = lCameraOffset.normalized * _playerSpeed * Time.deltaTime;
        lCameraOffset.y = oldYCameraOffset;

        Vector3 lCameraLookAt = transform.position + lCameraRotationY * Vector3.right * _cameraRightDistanceToPlayer + Vector3.up * _cameraUpDistanceToPlayer;

        Vector3 lLineCastCameraPoint = lCameraLookAt;
        lLineCastCameraPoint.y = transform.position.y;

        RaycastHit wallHit = new RaycastHit ();
        if (Physics.Linecast(transform.position, lLineCastCameraPoint, out wallHit, ~_cameraLayerMask))
        {
            lCameraLookAt += lCameraRotationY * Vector3.left * Mathf.Clamp (_cameraRightDistanceToPlayer - wallHit.distance, 0.1f, _cameraRightDistanceToPlayer);
        }

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

        lLineCastCameraPoint = lCameraLookAt;
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
            float rate = Mathf.Clamp (_cameraRightDistanceToPlayer - (transform.position - lCameraLookAt).magnitude, 0.0f, _cameraRightDistanceToPlayer) / _cameraRightDistanceToPlayer;
            lCameraHeightAfterCollision = rate * (_cameraMaxHeight + transform.position.y);
            lCameraFollow += lNewCameraRotation * Vector3.forward * rate * _cameraBackDistanceToPlayer + Vector3.up * lCameraHeightAfterCollision;
        }

        _camera.transform.position = Vector3.Lerp (_camera.transform.position, lCameraFollow, Time.deltaTime * _cameraMovementSpeed);
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