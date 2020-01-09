using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PastObject : MonoBehaviour
{
    protected MeshRenderer _meshRenderer = null;
    [SerializeField] protected Text _text = null;
    protected Vector3 _originalPosition;
    protected Quaternion _originalRotation;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        _originalPosition = transform.position;
        _originalRotation = transform.rotation;

        _meshRenderer.enabled = false;
        if(_text != null) _text.enabled = false;
    }

    public virtual void SetModeInteract()
    {
        transform.position = InspectionMode.instance.objectViewTransform.position;
        _meshRenderer.enabled = true;
        gameObject.layer = Utils_Variables.LAYER_OBJECT_INTERACT;

        if(_text != null) _text.enabled = false;
    }

    public void SetModeNearPlayer()
    {
        if(_text != null) _text.enabled = true;
        _meshRenderer.enabled = true;
        transform.position = _originalPosition;
        transform.rotation = _originalRotation;

        gameObject.layer = Utils_Variables.LAYER_CAMERA_COLLISION;
    }

    public void SetModeDiscovered()
    {
        if(_text != null) _text.enabled = false;
        _meshRenderer.enabled = true;
    }

    public void SetModeNotDiscovered()
    {
        //If we desactive the past zone when interacting 
        transform.position = _originalPosition;
        transform.rotation = _originalRotation;

        _meshRenderer.enabled = false;
        if(_text != null) _text.enabled = false;

        gameObject.layer = Utils_Variables.LAYER_CAMERA_COLLISION;
    }
}
