using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PastObject : MonoBehaviour
{
    protected MeshRenderer _meshRenderer = null;
    protected Collider _collider = null;
    protected TextMeshPro _text = null;
    protected Vector3 _originalPosition;
    protected Quaternion _originalRotation;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        _text = GetComponentInChildren<TextMeshPro>();

        _originalPosition = transform.position;
        _originalRotation = transform.rotation;

        _meshRenderer.enabled = false;
        _collider.isTrigger = true;
        if(_text != null) _text.enabled = false;
    }

    private void Update()
    {
        if(PastManager.instance.state == Enums.E_PAST_STATE.SEARCH_MODE) CheckPlayerDistance();
    }

    public virtual void SetModeInteract()
    {
        transform.position = InspectionMode.instance.objectViewTransform.position;
        _meshRenderer.enabled = true;
        if(_text != null) _text.enabled = false;
    }

    public void SetModeNearPlayer()
    {
        if(_text != null) _text.enabled = true;
        _meshRenderer.enabled = true;
        transform.position = _originalPosition;
        transform.rotation = _originalRotation;
    }

    public void SetModeDiscovered()
    {
        if(_text != null) _text.enabled = false;
        _meshRenderer.enabled = true;
    }

    public void SetModeNotDiscovered()
    {
        _meshRenderer.enabled = false;
        if(_text != null) _text.enabled = false;
        _collider.isTrigger = true;
    }

    protected void CheckPlayerDistance()
    {
        float distance = Vector3.Distance(transform.position, Controller.instance.transform.position);
        //Debug.DrawLine(MyCharacter.instance.transform.position, transform.position);

        if (distance < 1.5f)
        {
            PastManager.instance.SetNearObject(this);
        }

        else if(distance > 3f)
        {
            if (GameManager.instance.state != Enums.E_GAMESTATE.PLAY) return;
            SetModeNotDiscovered();
        }

        else
        {
            SetModeDiscovered();
        }
    }
}
