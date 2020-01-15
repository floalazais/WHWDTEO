using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEditor;
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
    public float delayBeforeNewInteraction;
}

[System.Serializable]
public struct HoldConstraint
{
    public int index;
    public Enums.E_GAMEPAD_BUTTON gamepadButton;
}

/*[CustomPropertyDrawer(typeof(WantedInteraction))]
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
}*/

public class ImportantPastObject : PastObject
{
    /*public WantedInteraction[] _wantedInteractionArray;
    int _index = 0;
    bool _isEnd = false;
    bool _isStepValidated = false;
    List<HoldConstraint> _holdConstraintList = new List<HoldConstraint>();

    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        _originalPosition = transform.position;
        _originalRotation = transform.rotation;

        _meshRenderer.enabled = false;
        _text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (PastManager.instance.state == Enums.E_PAST_STATE.SEARCH_MODE) CheckPlayerDistance();
        if (PastManager.instance.state == Enums.E_LEVEL_STATE.INTERACT) CheckInputOrder();
    }

    public override void SetModeInteract()
    {
        GameManager.instance.SetModeManipulation();
    }

    void CheckInputOrder()
    {
        WantedInteraction _currentInteraction = _wantedInteractionArray[_index];

        for (int i = 0; i < _holdConstraintList.Count; i++)
        {
            if (_currentInteraction.interactionType == Enums.E_INTERACT_TYPE.RELEASE_HOLD && _currentInteraction.gamepadButton == _holdConstraintList[i].gamepadButton) continue;
            if (!InputManager.instance.IsButtonDown(_holdConstraintList[i].gamepadButton))
            {
                _index = _holdConstraintList[i].index;
                _holdConstraintList.RemoveAll(h2 => h2.index >= _holdConstraintList.Find(h => h.gamepadButton == _holdConstraintList[i].gamepadButton).index);
                for (int j = 0; j < _index; j++)
                {
                    if (_wantedInteractionArray[j].interactionType == Enums.E_INTERACT_TYPE.HOLD)
                    {
                        HoldConstraint newHoldConstraint;
                        newHoldConstraint.gamepadButton = _wantedInteractionArray[j].gamepadButton;
                        newHoldConstraint.index = j;
                        _holdConstraintList.Add(newHoldConstraint);
                    } else if (_wantedInteractionArray[j].interactionType == Enums.E_INTERACT_TYPE.RELEASE_HOLD) {
                        _holdConstraintList.Remove(_holdConstraintList.Find(h => h.gamepadButton == _wantedInteractionArray[j].gamepadButton));
                    }
                }
                return;
            }
        }

        if (_isEnd || _isStepValidated) return;

        bool lIsValidated = false;

        print("button : " + _currentInteraction.gamepadButton + " interaction wanted : " + _currentInteraction.interactionType);

        switch (_currentInteraction.interactionType)
        {
            case Enums.E_INTERACT_TYPE.HOLD:
                lIsValidated = InputManager.instance.IsButtonPressed(_currentInteraction.gamepadButton);
                if (lIsValidated)
                {
                    HoldConstraint newHoldConstraint;
                    newHoldConstraint.gamepadButton = _currentInteraction.gamepadButton;
                    newHoldConstraint.index = _index;
                    _holdConstraintList.Add(newHoldConstraint);
                }
                break;

            case Enums.E_INTERACT_TYPE.SPAM:
                lIsValidated = InputManager.instance.IsButtonSpam(_currentInteraction.gamepadButton);
                break;

            case Enums.E_INTERACT_TYPE.CLICK:
                lIsValidated = InputManager.instance.IsButtonClicked(_currentInteraction.gamepadButton);
                break;

            case Enums.E_INTERACT_TYPE.MOVE:
                Enums.E_MOVE_DIRECTION lMoveDirection = _currentInteraction.moveDirection;
                lIsValidated = InputManager.instance.IsStickMoving(lMoveDirection);
                break;

            case Enums.E_INTERACT_TYPE.ROLL:
                Enums.E_ROLL_DIRECTION lRollDirection = _currentInteraction.rollDirection;
                lIsValidated = InputManager.instance.IsStickRolling(lRollDirection);
                break;

            case Enums.E_INTERACT_TYPE.SWIPE:
                Enums.E_SWIPE_DIRECTION lSwipeDirection = _currentInteraction.swipeDirection;
                lIsValidated = InputManager.instance.IsSwiping(lSwipeDirection);
                break;

            case Enums.E_INTERACT_TYPE.RELEASE_HOLD:
                lIsValidated = InputManager.instance.IsButtonReleased(_currentInteraction.gamepadButton);
                if (lIsValidated)
                {
                    _holdConstraintList.Remove(_holdConstraintList.Find(h => h.gamepadButton == _currentInteraction.gamepadButton));
                }
                break;
        }

        if (lIsValidated)
        {
            _isStepValidated = true;
            Invoke("NextStep", _currentInteraction.delayBeforeNewInteraction);
        }
    }

    void NextStep()
    {
        _index++;
        _isStepValidated = false;
        if (_index >= _wantedInteractionArray.Length)
        {
            _isEnd = true;
            print("end sequence");
        }
    }*/
}
