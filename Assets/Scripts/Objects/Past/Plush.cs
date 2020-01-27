using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plush : MonoBehaviour
{
    [SerializeField] AK.Wwise.Event soundEventInteract;
    [SerializeField] AK.Wwise.Event soundEventInspect;
    [SerializeField] GameObject _partToFind = null;
    [SerializeField] Transform _cameraFollow;
    [SerializeField] Transform _cameraLookAt;

    [SerializeField] protected CanvasObject _canvas = null;

    public bool inspected { get; private set; }

    Vector3 wallHitPosition;

    Vector3 _originalPosition;
    Quaternion _originalRotation;

    Collider _collider;

    void Start()
    {
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;

        inspected = false;
        _collider = GetComponent<Collider>();

        SetFarPlayerMode();
    }

    public void Interact()
    {
        _canvas.SetFarPlayerMode();

        _collider.isTrigger = true;

        SoundManager.instance.PlaySound(soundEventInteract.Id);

        transform.position = InspectionMode.instance.objectViewTransform.position;
        gameObject.layer = Utils_Variables.LAYER_OBJECT_INTERACT;

        foreach (Transform child in transform)
        {
            child.gameObject.layer = Utils_Variables.LAYER_OBJECT_INTERACT;
        }

        GameManager.instance.SetGameStateManipulation();
    }

    public void SetClosePlayerMode()
    {
        _canvas.SetClosePlayerMode();
    }

    public void SetMediumPlayerMode()
    {
        _canvas.SetMediumPlayerMode();
    }

    public void SetNearPlayerMode()
    {
        _canvas.SetClosestPlayerMode();

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
        _canvas.SetFarPlayerMode();

        _collider.isTrigger = false;

        transform.position = _originalPosition;
        transform.rotation = _originalRotation;
    }

    void Update()
    {
        if (gameObject.layer != Utils_Variables.LAYER_OBJECT_INTERACT || inspected) return;

        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Linecast(_cameraFollow.position, _cameraFollow.position + (_cameraLookAt.position - _cameraFollow.position) * 5, out hitInfo))
        {
            print(hitInfo.collider.name);
            if (hitInfo.collider.gameObject == _partToFind)
            {
                SoundManager.instance.PlaySound(soundEventInspect.Id);
                inspected = true;
            }
        }
    }
}
