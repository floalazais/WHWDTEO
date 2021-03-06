﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectViewable : ObjectInteractable
{
    protected Vector3 _originalPosition;
    protected Quaternion _originalRotation;

    [SerializeField] AK.Wwise.Event[] takeSoundEvents;
    [SerializeField] AK.Wwise.Event[] takeSoundEventsOnce;
    [SerializeField] public AK.Wwise.Event[] putSoundEvents;
    [SerializeField] public AK.Wwise.Event[] putSoundEventsOnce;
    bool takeSoundPlayed = false;
    public bool putSoundPlayed = false;

    private void Start()
    {
        //_meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();

        Init();

        SetModePresent();
    }

    protected override void Init()
    {
        base.Init();
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
    }

    public override void SetNearPlayerMode()
    {
        base.SetNearPlayerMode();
        transform.position = _originalPosition;
        transform.rotation = _originalRotation;
        gameObject.layer = Utils_Variables.LAYER_CAMERA_COLLISION;
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<CanvasObject>()) continue;

            child.gameObject.layer = Utils_Variables.LAYER_CAMERA_COLLISION;
        }
    }

    public override void Interact()
    {
        base.Interact();

        if (!takeSoundPlayed)
        {
            takeSoundPlayed = true;
            for (int i = 0; i < takeSoundEventsOnce.Length; i++)
            {
                SoundManager.instance.PlaySound(takeSoundEventsOnce[i].Id);
            }
        }

        for (int i = 0; i < takeSoundEvents.Length; i++)
        {
            SoundManager.instance.PlaySound(takeSoundEvents[i].Id);
        }

        transform.position = InspectionMode.instance.objectViewTransform.position;
        gameObject.layer = Utils_Variables.LAYER_OBJECT_INTERACT;

        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<CanvasObject>()) continue;

            child.gameObject.layer = Utils_Variables.LAYER_OBJECT_INTERACT;
        }
    }

    public override void SetModePresent()
    {
        base.SetModePresent();
        transform.position = _originalPosition;
        transform.rotation = _originalRotation;
    }
}
