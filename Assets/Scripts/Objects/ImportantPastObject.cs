using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct WantedInteraction
{
    public Enums.E_GAMEPAD_BUTTON gamepadButton;
    public Enums.E_INTERACT_TYPE interactionType;
    public float delayBeforeNewAction;
}

public class ImportantPastObject : PastObject
{
    public WantedInteraction[] _wantedInteractionArray;
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
        WantedInteraction _currentInteraction = _wantedInteractionArray[_index];
        bool lIsValidated = false;

        switch (_currentInteraction.interactionType)
        {
            case Enums.E_INTERACT_TYPE.HOLD:
                lIsValidated = InputManager.instance.IsButtonHold(_currentInteraction.gamepadButton);
                break;

            case Enums.E_INTERACT_TYPE.SPAM:
                lIsValidated = InputManager.instance.IsButtonSpam(_currentInteraction.gamepadButton);
                break;

            case Enums.E_INTERACT_TYPE.PRESSED:
                lIsValidated = InputManager.instance.IsButtonPressed(_currentInteraction.gamepadButton);
                break;

            case Enums.E_INTERACT_TYPE.ROLL_RIGHT:
                lIsValidated = InputManager.instance.IsStickRolling(Enums.E_ROLL_DIRECTION.RIGHT);
                break;

            case Enums.E_INTERACT_TYPE.ROLL_LEFT:
                lIsValidated = InputManager.instance.IsStickRolling(Enums.E_ROLL_DIRECTION.LEFT);
                break;
        }

        if (lIsValidated)
        {
            Invoke("NextStep", _currentInteraction.delayBeforeNewAction);
        }
    }

    void NextStep()
    {
        _index++;
        if (_index >= _wantedInteractionArray.Length) print("end");
    }
}
