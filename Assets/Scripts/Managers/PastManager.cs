using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PastManager : MonoBehaviour
{
    [SerializeField] PastZone _pastZone = null;
    [SerializeField] float _interactionRadius = 1.5f;
    [SerializeField] float _memoryZoneRadius = 3.0f;

    public static PastManager instance { get { return _instance; } }
    static PastManager _instance;

    public Enums.E_LEVEL_STATE state { get { return _state; } }
    Enums.E_LEVEL_STATE _state = Enums.E_LEVEL_STATE.PRESENT;

    List<W_Object> _objectsArray = new List<W_Object>();
    ObjectInteractable _objectNearPlayer;

    void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("ALREADY INSTANCE CREATED " + name);
            Destroy(_instance);
        }

        _instance = this;
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

            if (distance > _memoryZoneRadius)
            {
                lObject.SetModePresent();
            }

            else
            {
                if (state == Enums.E_LEVEL_STATE.MEMORY_MODE) lObject.SetModeMemory();

                if (lObject as ObjectInteractable == null) continue;
                if (!(lObject as ObjectInteractable).interactable) continue;

                if (distance > _interactionRadius)
                {
                    if (_objectNearPlayer == lObject)
                    {
                        _objectNearPlayer.SetFarPlayerMode();
                        _objectNearPlayer = null;
                    }
                }

                else
                {
                    if (distance <= lShortestDistance)
                    {
                        lShortestDistance = distance;
                        lNearestObject = lObject;
                    } 
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

        _objectNearPlayer.Interact();
    }

    void GoToPreviousState()
    {
        if(_state == Enums.E_LEVEL_STATE.INTERACT)
        {
            PutNearObject();
            GameManager.instance.SetGameStateExploration();
            _state = Enums.E_LEVEL_STATE.PRESENT;
        }
    }
    #endregion

    void RemovePastZone()
    {
        SetPresentMode();
        _pastZone.Remove();
    }

    void DisplayPastZone()
    {
        SetMemoryMode();
        _pastZone.Display();
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
            if (_objectNearPlayer != null)
            {
                _objectNearPlayer.SetModeMemory();
            }
        } else {
            RemovePastZone();
        }
    }
}
