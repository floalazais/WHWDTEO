using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PastObject : MonoBehaviour
{
    MeshRenderer _meshRenderer = null;
    Collider _collider = null;
    TextMeshPro _text = null;
    Vector3 _originalPosition;
    Quaternion _originalRotation;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        _text = GetComponentInChildren<TextMeshPro>();

        _originalPosition = transform.position;
        _originalRotation = transform.rotation;

        _meshRenderer.enabled = false;
        _collider.isTrigger = true;
        _text.enabled = false;
    }

    private void Update()
    {
        if(PastManager.instance.state == Enums.E_PAST_STATE.SEARCH_MODE) CheckPlayerDistance();
    }

    public void SetModeInteract()
    {
        transform.position = InspectionMode.instance.objectViewTransform.position;
        _meshRenderer.enabled = true;
        _text.enabled = false;
    }

    public void SetModeNearPlayer()
    {
        _text.enabled = true;
        _meshRenderer.enabled = true;
        transform.position = _originalPosition;
        transform.rotation = _originalRotation;
    }

    public void SetModeDiscovered()
    {
        _text.enabled = false;
        _meshRenderer.enabled = true;
    }

    public void SetModeNotDiscovered()
    {
        _meshRenderer.enabled = false;
        _text.enabled = false;
        _collider.isTrigger = true;
    }

    void CheckPlayerDistance()
    {
        float distance = Vector3.Distance(transform.position, MyCharacter.instance.transform.position);
        Debug.DrawLine(MyCharacter.instance.transform.position, transform.position);

        if (distance < 3f)
        {
            PastManager.instance.SetNearObject(this);
        }

        else if(distance > 5.5f)
        {
            if (GameManager.instance.state != Enums.E_GAMESTATE.PLAY) return;
            SetModeNotDiscovered();
        }

        else
        {
            SetModeDiscovered();
            //PastManager.instance.ResetNearPastObject(this);
        }
    }
}
