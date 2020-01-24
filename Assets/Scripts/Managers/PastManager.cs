using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PastManager : MonoBehaviour
{
    [SerializeField] PastZone _pastZone = null;
    [SerializeField] float _interactionRadius = 1.5f;
    [SerializeField] float _closeRadius = 2.0f;
    [SerializeField] float _memoryZoneRadius = 3.0f;

    public static PastManager instance { get; private set; }

    public Enums.E_LEVEL_STATE state { get { return _state; } }
    Enums.E_LEVEL_STATE _state = Enums.E_LEVEL_STATE.PRESENT;

    List<W_Object> _objectsArray = new List<W_Object>();
    ObjectInteractable _objectNearPlayer;

    bool _pastZoneDisplayed = false;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("ALREADY INSTANCE CREATED " + name);
            Destroy(instance);
        }

        instance = this;
    }

    private void Start()
    {
        if (_pastZone == null) Debug.LogError("NO PAST ZONE AFFECTED IN " + _pastZone);
        _pastZone.transform.localScale = new Vector3(_memoryZoneRadius * 2, _pastZone.transform.localScale.y, _memoryZoneRadius * 2);

        _objectsArray = GameObject.FindObjectsOfType<W_Object>().ToList();
    }

    private void Update()
    {
        if (GameManager.instance.state == Enums.E_GAMESTATE.NARRATION) return;

        if (InputManager.instance.IsButtonReleased(Enums.E_GAMEPAD_BUTTON.ROUND_BUTTON)) GoToPreviousState();
        if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.CROSS_BUTTON)) SetInteractMode();

        if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.R2_BUTTON)) DisplayPastZone();
        if (InputManager.instance.IsButtonReleased(Enums.E_GAMEPAD_BUTTON.R2_BUTTON)) RemovePastZone();

        if (GameManager.instance.state == Enums.E_GAMESTATE.EXPLORATION) CheckPlayerDistance();
    }

    protected void CheckPlayerDistance()
    {
        float lShortestDistance = _interactionRadius;
        W_Object lNearestObject = null;

        for (int i = 0; i < _objectsArray.Count; i++)
        {
            W_Object lObject = _objectsArray[i];

            float distance = Vector3.Distance(lObject.transform.position, Controller.instance.transform.position);

            //If we're too far from the player
            if (distance > _memoryZoneRadius)
            {
                lObject.SetModePresent();
            }

            //If we're not
            else
            {
                if (state == Enums.E_LEVEL_STATE.MEMORY_MODE) lObject.SetModeMemory();

                ObjectInteractable lObjectInteractable = lObject as ObjectInteractable;

                if (lObjectInteractable == null) continue;
                if (!lObjectInteractable.interactable) continue;

                //Conditions to avoid being interactable when we're not in the good time period
                if (lObjectInteractable.interactionTime == ObjectInteractable.InteractionTime.PAST && _state == Enums.E_LEVEL_STATE.PRESENT)
                {
                    lObjectInteractable.SetFarPlayerMode();
                    continue;
                }

                else if (lObjectInteractable.interactionTime == ObjectInteractable.InteractionTime.PRESENT && _state == Enums.E_LEVEL_STATE.MEMORY_MODE)
                {
                    lObjectInteractable.SetFarPlayerMode();
                    continue;
                }

                //If the object is too far from being interactable with player
                if (distance > _interactionRadius)
                {
                    //If the closest object is now too far
                    if (_objectNearPlayer == lObject)
                    {
                        _objectNearPlayer.SetFarPlayerMode();
                        _objectNearPlayer = null;
                    }

                    //If we are close but can't interact
                    if (distance <= _closeRadius)
                    {
                        lObjectInteractable.SetClosePlayerMode();
                        continue;
                    }

                    //If we are a bit close but can't interact
                    if (distance <= _memoryZoneRadius)
                    {
                        lObjectInteractable.SetMediumPlayerMode();
                        continue;
                    }

                    //lObjectInteractable.SetFarPlayerMode();
                }

                //If we can interact with the object
                else
                {

                    //Searching for the closest object
                    if (distance <= lShortestDistance)
                    {
                        lShortestDistance = distance;
                        if(lNearestObject != null) (lNearestObject as ObjectInteractable).SetClosePlayerMode();
                        lNearestObject = lObject;
                    } 

                    else lObjectInteractable.SetClosePlayerMode();
                }
            }
        }

        if(lNearestObject != null)
        {
            SetNearObject(lNearestObject);
        }
    }

    #region State Methods
    void SetPresentMode()
    {
        //If we desactive the past zone when reading an object description
        if (_state == Enums.E_LEVEL_STATE.DESCRIPTION) UIManager.instance.RemoveScreen();

        _state = Enums.E_LEVEL_STATE.PRESENT;
        GameManager.instance.SetGameStateExploration();

        if (_objectNearPlayer != null)
        {
            _objectNearPlayer.SetFarPlayerMode();
            _objectNearPlayer = null;
        }

        int length = _objectsArray.Count;
        for (int i = 0; i < length; i++)
        {
            _objectsArray[i].SetModePresent();
        }
    }

    void SetMemoryMode()
    {
        _state = Enums.E_LEVEL_STATE.MEMORY_MODE;
        _objectNearPlayer = null;
    }

    void SetInteractMode()
    {
        if (_objectNearPlayer == null) return;

        _state = Enums.E_LEVEL_STATE.INTERACT;
        GameManager.instance.SetGameStateManipulation();
        UIManager.instance.OnInspectionScreen();

        _objectNearPlayer.Interact();
    }

    void GoToPreviousState()
    {
        if(_state == Enums.E_LEVEL_STATE.INTERACT)
        {
            if ((_objectNearPlayer as ImportantPastObject) != null) return;
            PutNearObject();
            GameManager.instance.SetGameStateExploration();
            UIManager.instance.RemoveScreen();
            Refresh();
        }
    }
    #endregion

    void RemovePastZone()
    {
        SetPresentMode();
        _pastZone.Remove();

        SoundManager.instance.PlaySound(Utils_Variables.END_MEMORY_SOUND);

        UIManager.instance.RemoveScreen();

        _pastZoneDisplayed = false;
    }

    void DisplayPastZone()
    {
        SetMemoryMode();
        _pastZone.Display();

        SoundManager.instance.PlaySound(Utils_Variables.BEGIN_MEMORY_SOUND);

        _pastZoneDisplayed = true;
    }

    void PutNearObject()
    {
        _objectNearPlayer.SetNearPlayerMode();
    }

    public void SetNearPastObjectInMemoryMode(W_Object pObject)
    {
        if (_objectNearPlayer == pObject)
        {
            _objectNearPlayer.SetModeMemory();
            _objectNearPlayer = null;
        }
    }

    public void SetNearObject(W_Object pObject)
    {
        ObjectInteractable lObjectInteractable = pObject as ObjectInteractable;
        
        if (lObjectInteractable == _objectNearPlayer) return;

        if (lObjectInteractable.interactionTime == ObjectInteractable.InteractionTime.PAST && _state == Enums.E_LEVEL_STATE.PRESENT)
        {
            return;
        }

        else if (lObjectInteractable.interactionTime == ObjectInteractable.InteractionTime.PRESENT && _state == Enums.E_LEVEL_STATE.MEMORY_MODE)
        {
            return;
        }

        _objectNearPlayer = lObjectInteractable;
        _objectNearPlayer.SetNearPlayerMode();
    }

    public void RemoveObject(W_Object pObject)
    {
        _objectsArray.Remove(pObject);
    }

    public void Refresh()
    {
        if (InputManager.instance.IsButtonDown(Enums.E_GAMEPAD_BUTTON.R2_BUTTON))
        {
            if (!_pastZoneDisplayed) DisplayPastZone();
        }
        else
        {
            if (_pastZoneDisplayed) RemovePastZone();
            else SetPresentMode();
        }
    }
}
