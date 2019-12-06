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

        //if (GameManager.instance.state != Enums.E_GAMESTATE.PLAY) return;

        if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.R2_BUTTON)) DisplayPastZone();
        if (InputManager.instance.IsButtonReleased(Enums.E_GAMEPAD_BUTTON.R2_BUTTON)) RemovePastZone();
    }

    void RemovePastZone()
    {
        //if (GameManager.instance.state != Enums.E_GAMESTATE.PLAY) return;

        SetPresentMode();
        _pastZone.Remove();
    }

    void DisplayPastZone()
    {
        SetSearchMode();
        _pastZone.Display();
    }

    void SetPresentMode()
    {
        _state = Enums.E_PAST_STATE.PRESENT;
        GameManager.instance.SetModePlay();

        int length = _pastObjectsArray.Length;
        if (_pastObjectNearPlayer != null)
        {
            _pastObjectNearPlayer.SetModeNearPlayer();
            _pastObjectNearPlayer.SetModeNotDiscovered();
            _pastObjectNearPlayer = null;
        }

        for(int i = 0; i < length; i++)
        {
            _pastObjectsArray[i].SetModeNotDiscovered();
        }
    }

    public void ResetNearPastObject(PastObject pObject)
    {
        if(_pastObjectNearPlayer == pObject)
        {
            _pastObjectNearPlayer.SetModeDiscovered();
            _pastObjectNearPlayer = null;
        }
    }

    void SetSearchMode()
    {
        _state = Enums.E_PAST_STATE.SEARCH_MODE;
        _pastObjectNearPlayer = null;
    }

    //TO-DO BE SURE THERE IS ONLY ONE CLOSE OBJECT
    public void SetNearObject(PastObject pObject)
    {
        _pastObjectNearPlayer = pObject;
        _pastObjectNearPlayer.SetModeNearPlayer();
    }

    void SetInteractMode()
    {
        if(_state == Enums.E_PAST_STATE.INTERACT)
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
            SetInteractMode();
            UIManager.instance.RemoveScreen();

            return;
        }

        if (_state == Enums.E_PAST_STATE.INTERACT)
        {
            SetNearObject(_pastObjectNearPlayer);
            _state = Enums.E_PAST_STATE.SEARCH_MODE;
            GameManager.instance.SetModePlay();

            return;
        }
    }
}
