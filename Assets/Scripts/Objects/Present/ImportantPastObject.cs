using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;
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

public class ImportantPastObject : ObjectInteractable
{
    public WantedInteraction[] _wantedInteractionArray;
    int _index = 0;
    bool _isEnd = false;
    bool _isStepValidated = false;
    List<HoldConstraint> _holdConstraintList = new List<HoldConstraint>();

    [SerializeField] AK.Wwise.Event[] soundEvents;
    [SerializeField] AK.Wwise.Event[] stopSoundEvents;

    bool _isManipulated = false;

    float _timerBetweenTwoSteps = 0.0f;

    [SerializeField] PlayableAsset loopTimeline = null;
    [SerializeField] string _endDialogName;

    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();

        Init();

        SetModePresent();
    }

    // Update is called once per frame
    void Update()
    {
        //if (PastManager.instance.state == Enums.E_PAST_STATE.SEARCH_MODE) CheckPlayerDistance();
        if (!interactable) return;

        if (_isManipulated) CheckInputOrder();
    }

    public override void Interact()
    {
        base.Interact();
        _isManipulated = true;
        TimelineManager.instance.SetLoopMode(true);
        TimelineManager.instance.PlayTimeline(loopTimeline);
        UIManager.instance.OnManipulationScreen();
        ManipulationPanel.instance.DesactivateUI();
        ManipulationPanel.instance.ActivateUI(_wantedInteractionArray[0].gamepadButton, _wantedInteractionArray[0].interactionType);
    }

    void CheckInputOrder()
    {
        WantedInteraction _currentInteraction = _wantedInteractionArray[_index];

        for (int i = 0; i < _holdConstraintList.Count; i++)
        {
            if (_currentInteraction.interactionType == Enums.E_INTERACT_TYPE.RELEASE_HOLD && _currentInteraction.gamepadButton == _holdConstraintList[i].gamepadButton) continue;
            if (!InputManager.instance.IsButtonDown(_holdConstraintList[i].gamepadButton))
            {
                /*for (int j = _holdConstraintList[i].index; j < _index; i++)
                {
                    SoundManager.instance.PlaySound(stopSoundEvents[j].Id);
                }*/
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

                ManipulationPanel.instance.DesactivateUI();
                ManipulationPanel.instance.ActivateUI(_wantedInteractionArray[_index].gamepadButton, _wantedInteractionArray[_index].interactionType, _wantedInteractionArray[_index].rollDirection);
                _isStepValidated = false;
                return;
            }
        }

        if (_isEnd) return;

        if (_isStepValidated)
        {
            _timerBetweenTwoSteps -= Time.deltaTime;
            if (_timerBetweenTwoSteps < 0.0f)
            {
                _timerBetweenTwoSteps = 0.0f;
                NextStep();
            } else {
                return;
            }
        }

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

                    ManipulationPanel.instance.StartHoldingAnimation();
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
            _timerBetweenTwoSteps = _currentInteraction.delayBeforeNewInteraction;
            //Invoke("NextStep", _currentInteraction.delayBeforeNewInteraction);

            if (_wantedInteractionArray[_index].interactionType == Enums.E_INTERACT_TYPE.RELEASE_HOLD)
            {
                SoundManager.instance.PlaySound(stopSoundEvents[_index].Id);
            }

            else
            {
                SoundManager.instance.PlaySound(soundEvents[_index].Id);
            }
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
            _isManipulated = false;
            interactable = false;

            //Disable the object at the end of manipulation
            _meshRenderer.enabled = false;

            UIManager.instance.RemoveScreen();

            TimelineManager.instance.SetLoopMode(false);
            DialogManager.instance.StartDialog(_endDialogName);
        }

        else
        {
            ManipulationPanel.instance.ActivateUI(_wantedInteractionArray[_index].gamepadButton, _wantedInteractionArray[_index].interactionType, _wantedInteractionArray[_index].rollDirection);
        }
    }
}
