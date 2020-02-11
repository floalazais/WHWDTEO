using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get { return _instance; } }
    static GameManager _instance;

    public Enums.E_GAMESTATE state { get { return _state; } }
    Enums.E_GAMESTATE _state = Enums.E_GAMESTATE.EXPLORATION;

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
        SoundManager.instance.PlaySound(Utils_Variables.START_ROOM_TONE_SOUND);
        if (SceneManager.GetActiveScene().name == "MainScene") DialogManager.instance.StartDialog("introTL");

        Cursor.visible = false;
    }

    public void SetGameStateExploration()
    {
        _state = Enums.E_GAMESTATE.EXPLORATION;
    }

    public void SetGameStateManipulation()
    {
        _state = Enums.E_GAMESTATE.MANIPULATION;
    }

    public void SetGameStateImportantManipulation()
    {
        _state = Enums.E_GAMESTATE.IMPORTANT_MANIPULATION;
    }

    public void SetGameStateNarration()
    {
        _state = Enums.E_GAMESTATE.NARRATION;
    }
}
