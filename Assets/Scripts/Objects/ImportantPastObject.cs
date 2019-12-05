using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[System.Serializable]
public struct WantedInteraction
{
    public Enums.E_INTERACT_TYPE interactionType;
    public Enums.E_GAMEPAD_BUTTON gamepadButton;
    public Enums.E_MOVE_DIRECTION moveDirection;
    public Enums.E_ROLL_DIRECTION rollDirection;
    public Enums.E_SWIPE_DIRECTION swipeDirection;
    [Range(0, 10)]
    public int spamCount;
    [Range(0.0f, 10.0f)]
    public float holdTime;
    [Range(0.0f, 10.0f)]
    public float delayBeforeNewInteraction;
}

[CustomPropertyDrawer(typeof(WantedInteraction))]
public class WantedInteractionDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create property container element.
        VisualElement container = new VisualElement();

        // Create property fields.
        PropertyField interactionTypeField = new PropertyField(property.FindPropertyRelative("interactionType"));
        PropertyField gamepadButtonField = new PropertyField(property.FindPropertyRelative("gamepadButton"));
        PropertyField moveDirectionField = new PropertyField(property.FindPropertyRelative("moveDirection"), "Move Direction");
        PropertyField rollDirectionField = new PropertyField(property.FindPropertyRelative("rollDirection"), "Roll Direction");
        PropertyField swipeDirectionField = new PropertyField(property.FindPropertyRelative("swipeDirection"), "Swipe Direction");
        PropertyField spamCountField = new PropertyField(property.FindPropertyRelative("spamCount"), "Spam Count");
        PropertyField holdTimeField = new PropertyField(property.FindPropertyRelative("holdTime"), "Hold Time");
        PropertyField delayBeforeNewInteractionField = new PropertyField(property.FindPropertyRelative("holdTime"), "Hold Time");

        // Add fields to the container.
        container.Add(interactionTypeField);
        container.Add(gamepadButtonField);

        return container;
    }
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

            case Enums.E_INTERACT_TYPE.CLICK:
                lIsValidated = InputManager.instance.IsButtonClicked(_currentInteraction.gamepadButton);
                break;

            case Enums.E_INTERACT_TYPE.MOVE:
                lIsValidated = InputManager.instance.IsStickMoving(Enums.E_MOVE_DIRECTION.RIGHT);
                break;

            case Enums.E_INTERACT_TYPE.ROLL:
                lIsValidated = InputManager.instance.IsStickRolling(Enums.E_ROLL_DIRECTION.LEFT);
                break;

            case Enums.E_INTERACT_TYPE.SWIPE:
                lIsValidated = InputManager.instance.IsSwiping(Enums.E_SWIPE_DIRECTION.LEFT);
                break;
        }

        if (lIsValidated)
        {
            Invoke("NextStep", _currentInteraction.delayBeforeNewInteraction);
        }
    }

    void NextStep()
    {
        _index++;
        if (_index >= _wantedInteractionArray.Length) print("end");
    }
}
