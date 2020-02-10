using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterPanel : MonoBehaviour
{
    public static LetterPanel instance { get; private set; }
    [SerializeField] string[] _letters;
    [SerializeField] AK.Wwise.Event[] soundEvents;
    [SerializeField] AK.Wwise.Event[] stopSoundEvents;
    [SerializeField] Text _letterText;
    int _index = 0;
    bool _isHandWriting = true;
    bool _firstCrossRelease = true;

    bool[] displayedLetters;

    [SerializeField] Font _handWriterFont;
    [SerializeField] Font _typoWriterFont;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("ALREADY INSTANCE CREATED " + name);
            Destroy(instance);
        }

        instance = this;
    }

    void Start()
    {
        displayedLetters = new bool[_letters.Length];

        for (int i = 0; i < displayedLetters.Length; i++)
        {
            displayedLetters[i] = false;
        }
    }

    public void StartLetters()
    {
        GameManager.instance.SetGameStateNarration();
        _letterText.text = _letters[0];
        if (!displayedLetters[0])
        {
            displayedLetters[0] = true;
        }
        SoundManager.instance.PlaySound(soundEvents[0].Id);
    }

    private void Update()
    {
        if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.R1_BUTTON) || Input.GetKeyDown(KeyCode.Space)) DisplayNextLetter();
        if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.L1_BUTTON) || Input.GetKeyDown(KeyCode.LeftShift)) DisplayPreviousLetter();
        if (InputManager.instance.IsButtonReleased(Enums.E_GAMEPAD_BUTTON.ROUND_BUTTON) || Input.GetKeyDown(KeyCode.Mouse1)) OnBack();
        if (InputManager.instance.IsButtonReleased(Enums.E_GAMEPAD_BUTTON.SQUARE_BUTTON) || Input.GetKeyDown(KeyCode.Mouse0)) OnToggle();
    }

    void DisplayPreviousLetter()
    {
        SoundManager.instance.PlaySound(stopSoundEvents[_index].Id);

        if (_index == 0) _index = _letters.Length - 1;
        else _index--;

        DisplayCurrentLetter();
    }

    void DisplayNextLetter()
    {
        SoundManager.instance.PlaySound(stopSoundEvents[_index].Id);

        if (_index == _letters.Length - 1) _index = 0;
        else _index++;

        DisplayCurrentLetter();
    }

    void DisplayCurrentLetter()
    {
        SoundManager.instance.PlaySound(Utils_Variables.LETTRES_SOUND);

        if (!displayedLetters[_index])
        {
            displayedLetters[_index] = true;
        }
        SoundManager.instance.PlaySound(soundEvents[_index].Id);

        _letterText.text = _letters[_index];
    }

    void OnBack()
    {
        _isHandWriting = true;
        _firstCrossRelease = true;
        _letterText.GetComponent<Text>().font = _handWriterFont;

        GameManager.instance.SetGameStateExploration();
        UIManager.instance.RemoveScreen();
        PastManager.instance.Refresh();
    }

    void OnToggle()
    {
        if (_firstCrossRelease)
        {
            _firstCrossRelease = false;
            return;
        }

        if (_isHandWriting) _letterText.GetComponent<Text>().font = _typoWriterFont;
        else _letterText.GetComponent<Text>().font = _handWriterFont;

        _isHandWriting = !_isHandWriting;
    }
}
