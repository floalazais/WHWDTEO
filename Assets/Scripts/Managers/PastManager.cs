using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastManager : MonoBehaviour
{
    [SerializeField] PastZone _pastZone = null;

    public static PastManager instance { get { return _instance; } }
    static PastManager _instance;

    public Enums.E_LEVEL_STATE state { get { return _state; } }
    Enums.E_LEVEL_STATE _state = Enums.E_LEVEL_STATE.PRESENT;

    W_Object[] _objectsArray;
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

        _objectsArray = GameObject.FindObjectsOfType<W_Object>();
    }

    private void Update()
    {
        if (InputManager.instance.IsButtonReleased(Enums.E_GAMEPAD_BUTTON.ROUND_BUTTON)) GoToPreviousState();
        if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.CROSS_BUTTON)) SetInteractMode();

        if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.R2_BUTTON)) DisplayPastZone();
        if (InputManager.instance.IsButtonReleased(Enums.E_GAMEPAD_BUTTON.R2_BUTTON)) RemovePastZone();

        CheckPlayerDistance();
    }

    protected void CheckPlayerDistance()
    {
        float lShortestDistance = 1.5f;
        W_Object lNearestObject = null;

        for (int i = 0; i < _objectsArray.Length; i++)
        {
            W_Object lObject = _objectsArray[i];

            float distance = Vector3.Distance(lObject.transform.position, Controller.instance.transform.position);

            if (distance > 3f)
            {
                if (GameManager.instance.state != Enums.E_GAMESTATE.PLAY) return;
                lObject.SetModePresent();
            }

            else
            {
                if (state == Enums.E_LEVEL_STATE.MEMORY_MODE) lObject.SetModeMemory();

                if (distance < 1.5f)
                {
                    if (distance <= lShortestDistance)
                    {
                        lShortestDistance = distance;
                        lNearestObject = lObject;
                    } 
                }

                else
                {
                    if (_objectNearPlayer == lObject)
                    {
                        _objectNearPlayer.SetFarPlayerMode();
                        _objectNearPlayer = null;
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
        GameManager.instance.SetModePlay();

        if (_objectNearPlayer != null)
        {
            _objectNearPlayer.SetFarPlayerMode();
            _objectNearPlayer = null;
        }

        int length = _objectsArray.Length;
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
        GameManager.instance.SetModeNotPlay();

        _objectNearPlayer.Interact();
    }

    void GoToPreviousState()
    {
        if(_state == Enums.E_LEVEL_STATE.INTERACT)
        {
            PutNearObject();
            GameManager.instance.SetModePlay();
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

        if (lObjectInteractable == null) return;
        if (lObjectInteractable == _objectNearPlayer) return;

        _objectNearPlayer = lObjectInteractable;
        _objectNearPlayer.SetNearPlayerMode();
    }
}
