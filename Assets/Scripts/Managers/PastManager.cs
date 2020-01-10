using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastManager : MonoBehaviour
{
    [SerializeField] PastZone _pastZone = null;

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

        if (state == Enums.E_PAST_STATE.SEARCH_MODE) CheckPlayerDistance();
    }

    protected void CheckPlayerDistance()
    {
        float lShortestDistance = 0;

        for (int i = 0; i < _pastObjectsArray.Length; i++)
        {
            PastObject lPastObject = _pastObjectsArray[i];

            float distance = Vector3.Distance(lPastObject.transform.position, Controller.instance.transform.position);
            if (i == 0) lShortestDistance = distance;
            //Debug.DrawLine(MyCharacter.instance.transform.position, transform.position);

            if (distance < 1.5f)
            {
                if (distance <= lShortestDistance)
                {
                    lShortestDistance = distance;
                    SetNearObjectToInteractionState(lPastObject);
                }
            }

            else if (distance > 3f)
            {
                if (GameManager.instance.state != Enums.E_GAMESTATE.EXPLORATION) return;
                lPastObject.SetModeNotDiscovered();
            }

            else
            {
                if (_pastObjectNearPlayer == lPastObject) _pastObjectNearPlayer = null;
                lPastObject.SetModeDiscovered();
            }

        }
    }

    #region State Methods
    void SetPresentMode()
    {
        //If we desactive the past zone when reading an object description
        if (_state == Enums.E_PAST_STATE.DESCRIPTION) UIManager.instance.RemoveScreen();

        _state = Enums.E_PAST_STATE.PRESENT;
        GameManager.instance.SetGameStateExploration();

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

        GameManager.instance.SetGameStateNarration();
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
            GameManager.instance.SetGameStateExploration();

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
        if (_pastObjectNearPlayer == pObject)
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
