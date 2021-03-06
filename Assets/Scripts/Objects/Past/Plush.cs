﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Plush : MonoBehaviour
{
    [SerializeField] AK.Wwise.Event soundEventInteract;
    [SerializeField] AK.Wwise.Event soundEventInspect;
    [SerializeField] AK.Wwise.Event soundEventVoiceInteract;
    [SerializeField] AK.Wwise.Event soundEventVoiceInspect;
    [SerializeField] public AK.Wwise.Event soundEventMusicBoxNotePut;
    [SerializeField] GameObject _partToFind = null;
    [SerializeField] Transform _cameraFollow;
    [SerializeField] Transform _cameraLookAt;

    [SerializeField] protected CanvasObject _canvas = null;

    public bool inspected { get; private set; }

    Vector3 wallHitPosition;

    Vector3 _originalPosition;
    Quaternion _originalRotation;

    Collider _collider;

    bool _touchedGround = false;

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
        SoundManager.instance.PlaySound(soundEventVoiceInteract.Id);

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
    }

    public void Put()
    {
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
    }

    public void SetNewPosition(Vector3 newPosition)
    {
        _originalPosition = newPosition;
        transform.position = newPosition;
    }

    void Update()
    {
        if (gameObject.layer != Utils_Variables.LAYER_OBJECT_INTERACT)
        {
            transform.position = _originalPosition + new Vector3(0, Mathf.Cos(Time.time), 0) * 0.1f;
            transform.Rotate(Vector3.up, Time.deltaTime * 10);
        }
        else if (!inspected)
        {
            Vector3 vectorTarget = _partToFind.transform.position - transform.position;
            Vector3 vectorCamera = transform.position - _cameraFollow.position;
            RaycastHit hitInfo = new RaycastHit();
            print(Vector3.Angle(vectorCamera, vectorTarget));
            if (Vector3.Angle(vectorCamera, vectorTarget) > 150)
            {
                SoundManager.instance.PlaySound(soundEventInspect.Id);
                SoundManager.instance.PlaySound(soundEventVoiceInspect.Id);
                inspected = true;
                gameObject.GetComponent<ObjectManipulation>().stop = true;
                /*if (Physics.Linecast(_cameraFollow.position, _cameraFollow.position + (_partToFind.transform.position - _cameraFollow.position) * 5, out hitInfo))
                {
                    print(hitInfo.collider.name);
                    if (hitInfo.collider.gameObject == _partToFind)
                    {
                    }
                }*/
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!_touchedGround && collision.collider.name == "walls")
        {
            _touchedGround = true;
            SoundManager.instance.PlaySound("Play_Tombe_Peluche");
        }
    }
}
