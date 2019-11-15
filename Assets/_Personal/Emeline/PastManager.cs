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

    void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("ALREADY INSTANCE CREATED " + name);
            Destroy(_instance);
        }

        _instance = this;

        AddListener();
    }

    void AddListener()
    {
        EventsManager.Instance.AddListener<ONR2ButtonDown>(DisplayPastZone);
        EventsManager.Instance.AddListener<ONR2ButtonUp>(RemovePastZone);
    }

    private void Start()
    {
        if (_pastZone == null) Debug.LogError("NO PAST ZONE AFFECTED IN " + _pastZone);

        _pastObjectsArray = GameObject.FindObjectsOfType<PastObject>();
    }

    void RemovePastZone(ONR2ButtonUp e)
    {
        if (GameManager.instance.state != Enums.E_GAMESTATE.PLAY) return;

        SetPresentMode();
        _pastZone.Remove();
    }

    void DisplayPastZone(ONR2ButtonDown e)
    {
        SetSearchMode();
        _pastZone.Display();
    }

    void SetPresentMode()
    {
        _state = Enums.E_PAST_STATE.PRESENT;

        int length = _pastObjectsArray.Length;

        for(int i = 0; i < length; i++)
        {
            _pastObjectsArray[i].SetModeNotDiscovered();
        }
    }

    void SetSearchMode()
    {
        _state = Enums.E_PAST_STATE.SEARCH_MODE;
    }

    private void OnDestroy()
    {
        EventsManager.Instance.RemoveListener<ONR2ButtonDown>(DisplayPastZone);
        EventsManager.Instance.RemoveListener<ONR2ButtonUp>(RemovePastZone);
    }
}
