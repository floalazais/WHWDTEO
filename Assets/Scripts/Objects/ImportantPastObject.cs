using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct WantedInteraction
{
    public string gamepadButton; //Make an enum
    public Enums.E_INTERACT_TYPE interactionType;
    public float delayBeforeNewAction;
}

public class ImportantPastObject : PastObject
{
    public WantedInteraction[] _test;
    int _index = 0;

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

    // Update is called once per frame
    void Update()
    {
        if (PastManager.instance.state == Enums.E_PAST_STATE.SEARCH_MODE) CheckPlayerDistance();
        if (PastManager.instance.state == Enums.E_PAST_STATE.INTERACT) CheckInputOrder();
    }

    public override void SetModeInteract()
    {
        print("mdlol");
        GameManager.instance.SetModeManipulation();
    }

    void CheckInputOrder()
    {
        WantedInteraction _currentInteraction = _test[_index];

        if (InputManager.instance.IsButtonPressed(_currentInteraction.gamepadButton))
        {
            Invoke("NextStep", _currentInteraction.delayBeforeNewAction);
        }
    }

    void NextStep()
    {
        _index++;
        if (_index >= _test.Length) print("end");
    }
}
