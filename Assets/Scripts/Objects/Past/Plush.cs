using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plush : MonoBehaviour
{
    [SerializeField] GameObject _partToFind = null;
    [SerializeField] Transform _cameraFollow;
    [SerializeField] Transform _cameraLookAt;

    [SerializeField] protected Image _iconUI = null;

    bool _inspected = false;

    Vector3 wallHitPosition;

    Vector3 _originalPosition;
    Quaternion _originalRotation;

    Collider _collider;

    void Start()
    {
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;

        _iconUI.enabled = false;
        _collider = GetComponent<Collider>();

        SetFarPlayerMode();
    }

    public void Interact()
    {
        _iconUI.enabled = false;
        _collider.isTrigger = true;

        transform.position = InspectionMode.instance.objectViewTransform.position;
        gameObject.layer = Utils_Variables.LAYER_OBJECT_INTERACT;

        foreach (Transform child in transform)
        {
            child.gameObject.layer = Utils_Variables.LAYER_OBJECT_INTERACT;
        }

        GameManager.instance.SetGameStateManipulation();
    }

    public void SetNearPlayerMode()
    {
        _iconUI.enabled = true;
        _collider.isTrigger = false;

        transform.position = _originalPosition;
        transform.rotation = _originalRotation;

        gameObject.layer = Utils_Variables.LAYER_CAMERA_COLLISION;

        foreach (Transform child in transform)
        {
            child.gameObject.layer = Utils_Variables.LAYER_CAMERA_COLLISION;
        }
    }

    public void SetFarPlayerMode()
    {
        _iconUI.enabled = false;
        _collider.isTrigger = false;

        transform.position = _originalPosition;
        transform.rotation = _originalRotation;
    }

    void Update()
    {
        if (gameObject.layer != Utils_Variables.LAYER_OBJECT_INTERACT || _inspected) return;

        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Linecast(_cameraFollow.position, _cameraFollow.position + (_cameraLookAt.position - _cameraFollow.position) * 5, out hitInfo))
        {
            if (hitInfo.collider.gameObject == _partToFind)
            {
                print("trouvé !");
                _inspected = true;
            }
        }
    }
}
