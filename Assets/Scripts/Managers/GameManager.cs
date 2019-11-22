using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get { return _instance; } }
    static GameManager _instance;

    public Enums.E_GAMESTATE state { get { return _state; } }
    Enums.E_GAMESTATE _state = Enums.E_GAMESTATE.PLAY;

    void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("ALREADY INSTANCE CREATED " + name);
            Destroy(_instance);
        }

        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("Event_Sine_Test", gameObject);
    }

    public void SetModePlay()
    {
        _state = Enums.E_GAMESTATE.PLAY;
    }

    public void SetModeNotPlay()
    {
        _state = Enums.E_GAMESTATE.NOT_PLAY;
    }

    public void SetModeManipulation()
    {
        _state = Enums.E_GAMESTATE.MANIPULATION;
    }
}
