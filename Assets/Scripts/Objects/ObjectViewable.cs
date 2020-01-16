using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectViewable : ObjectInteractable
{
    protected Vector3 _originalPosition;
    protected Quaternion _originalRotation;

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
    }

    public override void Interact()
    {
        transform.position = InspectionMode.instance.objectViewTransform.position;
        gameObject.layer = Utils_Variables.LAYER_OBJECT_INTERACT;

        _text.enabled = false;
    }

    public override void SetModePresent()
    {
        base.SetModePresent();
        transform.position = _originalPosition;
        transform.rotation = _originalRotation;
    }
}
