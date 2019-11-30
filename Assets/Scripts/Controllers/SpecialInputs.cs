using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct Spam_Button
{
    public Enums.E_GAMEPAD_BUTTON buttonPressed;
    public float timer;
}

public class SpecialInputs : MonoBehaviour
{
    float _buttonDownTime = 0;
    float _buttonUpTime = 0;
    int _spamCount = 0;

    [SerializeField] float _spamGap = 1f;
    [SerializeField] int _numberSpam = 3;

    [SerializeField] List<Spam_Button> _spamButtons = new List<Spam_Button>();

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        /*if(InputManager.instance.IsButtonPressed(Utils_Variables.CROSS_BUTTON_ACTION))
        {
            Spam_Button spamButton;
            spamButton.buttonPressed = Enums.E_GAMEPAD_BUTTON.CROSS_BUTTON;
            spamButton.timer = _spamGap;
        }*/

        print(IsButtonSpam(Enums.E_GAMEPAD_BUTTON.CROSS_BUTTON));

        for(int i = 0; i < (int)Enums.E_GAMEPAD_BUTTON.NB_BUTTONS; i++)
        {
            Enums.E_GAMEPAD_BUTTON pressedButton = (Enums.E_GAMEPAD_BUTTON)i;

            if (InputManager.instance.IsButtonPressed(pressedButton.ToString()))
            {
                print(pressedButton.ToString());
                Spam_Button spamButton;
                spamButton.buttonPressed = pressedButton;
                spamButton.timer = _spamGap;

                _spamButtons.Add(spamButton);
            }
        }

        RefreshPressedButton();
    }

    void RefreshPressedButton()
    {
        for(int i = 0; i < _spamButtons.Count; i++)
        {
            Spam_Button lSpamButton = _spamButtons[i];
            lSpamButton.timer -= Time.deltaTime;

            _spamButtons[i] = lSpamButton;
            if (_spamButtons[i].timer <= 0.0f) _spamButtons.RemoveAt(i);
        }
    }

    bool IsButtonSpam(Enums.E_GAMEPAD_BUTTON pButton)
    {
        int countButtonInArray = 0;

        for(int i = 0; i < _spamButtons.Count; i++)
        {
            if (_spamButtons[i].buttonPressed == pButton) countButtonInArray++;
        }

        if (countButtonInArray >= _numberSpam) return true;

        return false;
    }

    void ButtonDown()
    {
        _buttonDownTime = Time.fixedTime;
    }

    void ButtonUp()
    {
        _buttonUpTime = Time.fixedTime;
        if (_buttonUpTime - _buttonDownTime < _spamGap)
        {
            _spamCount++;
            GetSpamLenght();
        }

        else _spamCount = 0;
    }

    void GetSpamLenght()
    {
        if (_spamCount < 3) return;
        else if (_spamCount >= 3 && _spamCount < 15) print("spam");
        else print("spam long");
    }
}
