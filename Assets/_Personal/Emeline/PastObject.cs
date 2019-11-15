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

    Enums.E_PAST_OBJECT_STATE _state = Enums.E_PAST_OBJECT_STATE.NOT_DISCOVERED;

    private void Awake()
    {
        EventsManager.Instance.AddListener<OnCrossButton>(SetModeInteract);
        EventsManager.Instance.AddListener<OnRoundButton>(SetModeNearPlayer);
    }
    // Start is called before the first frame update
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

    void SetModeInteract(OnCrossButton e)
    {
        if(_state == Enums.E_PAST_OBJECT_STATE.INSPECTED)
        {
            UIManager.instance.OnDescriptionObject();
            GameManager.instance.SetModeDescription();
            return;
        }

        if (_state != Enums.E_PAST_OBJECT_STATE.NEAR_PLAYER) return;

        _state = Enums.E_PAST_OBJECT_STATE.INSPECTED;

        transform.position = InspectionMode.instance.objectViewTransform.position;
        _meshRenderer.enabled = true;
        _text.enabled = false;

        GameManager.instance.SetModeInspection(); //TO-DO : Architecture
    }

    void SetModeNearPlayer(OnRoundButton e = null)
    {
        if (e != null && GameManager.instance.state != Enums.E_GAMESTATE.INSPECTION) return;

        _state = Enums.E_PAST_OBJECT_STATE.NEAR_PLAYER;

        _text.enabled = true;
        _meshRenderer.enabled = true;
        transform.position = _originalPosition;
        transform.rotation = _originalRotation;

        GameManager.instance.SetModePlay();
    }

    void SetModeDiscovered()
    {
        _state = Enums.E_PAST_OBJECT_STATE.DISCOVERED;
        _text.enabled = false;
        _meshRenderer.enabled = true;
    }

    public void SetModeNotDiscovered()
    {
        _state = Enums.E_PAST_OBJECT_STATE.NOT_DISCOVERED;
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
            SetModeNearPlayer();
        }

        else if(distance > 5.5f)
        {
            if (GameManager.instance.state != Enums.E_GAMESTATE.PLAY) return;
            SetModeNotDiscovered();
        }

        else
        {
            SetModeDiscovered();
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<SphereCollider>()) return;

        SetModeDiscovered();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<SphereCollider>() || _state == Enums.E_PAST_OBJECT_STATE.INSPECTED) return;

        SetModeNotDiscovered();
    }*/
}
