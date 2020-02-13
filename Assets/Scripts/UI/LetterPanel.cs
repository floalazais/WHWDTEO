using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterPanel : MonoBehaviour
{
    public static LetterPanel instance { get; private set; }
    [SerializeField] [TextArea] string[] _letters;
    [SerializeField] Sprite[] _lettersImage;

    [SerializeField] AK.Wwise.Event[] soundEvents;
    [SerializeField] AK.Wwise.Event[] stopSoundEvents;
    [SerializeField] Text _letterText;
    [SerializeField] Image _letterImage;
    [SerializeField] Image _backgroundImage;
    int _index = 0;
    bool _isHandWriting = true;

    bool[] displayedLetters;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("ALREADY INSTANCE CREATED " + name);
            Destroy(instance);
        }

        instance = this;

        displayedLetters = new bool[_letters.Length];

        for (int i = 0; i < displayedLetters.Length; i++)
        {
            displayedLetters[i] = false;
        }
    }

    //void Start()
    //{
    //    displayedLetters = new bool[_letters.Length];

    //    for (int i = 0; i < displayedLetters.Length; i++)
    //    {
    //        displayedLetters[i] = false;
    //    }
    //}

    public void StartLetters()
    {
        GameManager.instance.SetGameStateNarration();
        _letterText.text = _letters[0];
        _letterImage.sprite = _lettersImage[0];
        RemoveTypeWriting();

        if (!displayedLetters[0])
        {
            displayedLetters[0] = true;
        }
        SoundManager.instance.PlaySound(soundEvents[0].Id);
        RemoveTypeWriting();
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

        if (_index == 0) _index = _letters.Length - 1;
        else _index--;

        SoundManager.instance.PlaySound(stopSoundEvents[_index].Id);
        DisplayCurrentLetter();
    }

    void DisplayNextLetter()
    {

        if (_index == _letters.Length - 1) _index = 0;
        else _index++;

        SoundManager.instance.PlaySound(stopSoundEvents[_index].Id);
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
        _letterImage.sprite = _lettersImage[_index];
    }

    void OnBack()
    {
        _isHandWriting = true;
        RemoveTypeWriting();

        GameManager.instance.SetGameStateExploration();
        UIManager.instance.RemoveScreen();
        PastManager.instance.Refresh();
    }

    void OnToggle()
    {
        if (_isHandWriting) RemoveTypeWriting();
        else EnableTypeWriting();

        _isHandWriting = !_isHandWriting;
    }

    void EnableTypeWriting()
    {
        _backgroundImage.enabled = true;
        _letterText.enabled = true;
    }

    void RemoveTypeWriting()
    {
        _backgroundImage.enabled = false;
        _letterText.enabled = false;
    }
}
