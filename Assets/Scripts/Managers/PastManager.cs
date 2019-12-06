using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastManager : MonoBehaviour
{
    [SerializeField] PastZone _pastZone;

    public static PastManager instance { get { return _instance; } }
    static PastManager _instance;

    public Enums.E_PAST_STATE state { get { return _state; } }
    Enums.E_PAST_STATE _state = Enums.E_PAST_STATE.PRESENT;

    PastObject[] _pastObjectsArray;
    PastObject _pastObjectNearPlayer;

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

        _pastObjectsArray = GameObject.FindObjectsOfType<PastObject>();
    }

    private void Update()
    {
        if (InputManager.instance.IsButtonReleased(Enums.E_GAMEPAD_BUTTON.ROUND_BUTTON)) GoToPreviousState();
        if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.CROSS_BUTTON)) SetInteractMode();

        if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.R2_BUTTON)) DisplayPastZone();
        if (InputManager.instance.IsButtonReleased(Enums.E_GAMEPAD_BUTTON.R2_BUTTON)) RemovePastZone();
    }

    #region State Methods
    void SetPresentMode()
    {
        _state = Enums.E_PAST_STATE.PRESENT;
        GameManager.instance.SetModePlay();

        //If we desactive the past zone when reading an object description
        if(_state == Enums.E_PAST_STATE.DESCRIPTION) UIManager.instance.RemoveScreen();

        if (_pastObjectNearPlayer != null)
        {
            _pastObjectNearPlayer.SetModeNotDiscovered();
            _pastObjectNearPlayer = null;
        }

        int length = _pastObjectsArray.Length;
        for (int i = 0; i < length; i++)
        {
            _pastObjectsArray[i].SetModeNotDiscovered();
        }
    }

    void SetSearchMode()
    {
        _state = Enums.E_PAST_STATE.SEARCH_MODE;
        _pastObjectNearPlayer = null;
    }

    void SetInteractMode()
    {
        //Can't interact anymore when there is a description
        if (_state == Enums.E_PAST_STATE.DESCRIPTION) return;

        if (_state == Enums.E_PAST_STATE.INTERACT)
        {
            if (_pastObjectNearPlayer.GetComponent<ImportantPastObject>() != null) return;

            UIManager.instance.OnDescriptionObject();
            _state = Enums.E_PAST_STATE.DESCRIPTION;

            return;
        }

        if (_pastObjectNearPlayer == null) return;

        _state = Enums.E_PAST_STATE.INTERACT;
        _pastObjectNearPlayer.SetModeInteract();

        GameManager.instance.SetModeNotPlay();
    }

    void GoToPreviousState()
    {
        if (_state == Enums.E_PAST_STATE.DESCRIPTION)
        {
            UIManager.instance.RemoveScreen();
            _state = Enums.E_PAST_STATE.INTERACT;

            return;
        }

        if (_state == Enums.E_PAST_STATE.INTERACT)
        {
            SetNearObjectToInteractionState(_pastObjectNearPlayer);
            _state = Enums.E_PAST_STATE.SEARCH_MODE;
            GameManager.instance.SetModePlay();

            return;
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
        SetSearchMode();
        _pastZone.Display();
    }

    public void SetNearPastObjectInDiscoveredMode(PastObject pObject)
    {
        if(_pastObjectNearPlayer == pObject)
        {
            _pastObjectNearPlayer.SetModeDiscovered();
            _pastObjectNearPlayer = null;
        }
    }

    //TO-DO BE SURE THERE IS ONLY ONE CLOSE OBJECT
    public void SetNearObjectToInteractionState(PastObject pObject)
    {
        _pastObjectNearPlayer = pObject;
        _pastObjectNearPlayer.SetModeNearPlayer();
    }  
}
