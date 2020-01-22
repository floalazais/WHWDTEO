using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterPanel : MonoBehaviour
{
    public static LetterPanel instance { get; private set; }
    [SerializeField] string[] _letters;
    [SerializeField] Text _letterText;
    int _index = 0;
    bool _isHandWriting = true;

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

    private void OnEnable()
    {
        GameManager.instance.SetGameStateNarration();
        _letterText.text = _letters[0];    
    }

    private void Update()
    {
        if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.R2_BUTTON)) DisplayNextLetter();
        if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.L2_BUTTON)) DisplayPreviousLetter();
        if (InputManager.instance.IsButtonReleased(Enums.E_GAMEPAD_BUTTON.ROUND_BUTTON)) OnBack();
        if (InputManager.instance.IsButtonReleased(Enums.E_GAMEPAD_BUTTON.CROSS_BUTTON)) OnToggle();
    }

    void DisplayPreviousLetter()
    {
        if (_index == 0) _index = _letters.Length - 1;
        else _index--;

        _letterText.text = _letters[_index];
    }

    void DisplayNextLetter()
    {
        if (_index == _letters.Length - 1) _index = 0;
        else _index++;

        _letterText.text = _letters[_index];
    }

    void OnBack()
    {
        GameManager.instance.SetGameStateExploration();
        UIManager.instance.RemoveScreen();
    }

    void OnToggle()
    {
        if (_isHandWriting) _letterText.GetComponent<Text>().font = _typoWriterFont;
        else _letterText.GetComponent<Text>().font = _handWriterFont;

        _isHandWriting = !_isHandWriting;
    }
}
