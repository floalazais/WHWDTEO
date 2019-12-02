using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance { get { return _instance; } }
    static InputManager _instance;

    #region Sticks
    public float rightHorizontalAxis { get; private set; }
    public float rightVerticalAxis { get; private set; }
    public float leftHorizontalAxis { get; private set; }
    public float leftVerticalAxis { get; private set; }
    #endregion

    #region Hold Input

    float[] _buttonsHoldTime = new float[(int)Enums.E_GAMEPAD_BUTTON.NB_BUTTONS];
    public float holdTime = 2.0f;

    #endregion

    #region Spam Input

    struct Spam_Button
    {
        public Enums.E_GAMEPAD_BUTTON buttonPressed;
        public float timer;
    }

    List<Spam_Button> _spamButtons = new List<Spam_Button>();

    [SerializeField] float _spamGap = 1f;
    [SerializeField] int _numberSpam = 3;

    #endregion

    private void Awake()
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
        InitializeButtonsHoldTime();
    }

    #region Basic Input Functions
    public bool IsButtonPressed(Enums.E_GAMEPAD_BUTTON pButton)
    {
        return Utils_Variables.REWIRED_PLAYER.GetButtonDown(pButton.ToString());
    }

    public bool IsButtonReleased(Enums.E_GAMEPAD_BUTTON pButton)
    {
        return Utils_Variables.REWIRED_PLAYER.GetButtonUp(pButton.ToString());
    }

    public bool IsButtonDown(Enums.E_GAMEPAD_BUTTON pButton)
    {
        return Utils_Variables.REWIRED_PLAYER.GetButton(pButton.ToString());
    }
    #endregion

    #region Special Input Functions

        #region Hold Input Functions

    void InitializeButtonsHoldTime()
    {
        for (int i = 0; i < (int)Enums.E_GAMEPAD_BUTTON.NB_BUTTONS; i++)
        {
            _buttonsHoldTime[i] = -1.0f;
        }
    }

    public bool IsButtonHold(Enums.E_GAMEPAD_BUTTON pButton)
    {
        return _buttonsHoldTime[(int)pButton] >= holdTime;
    }


    #endregion

        #region Spam Input Functions

    void UpdateSpamButtonsTimer()
    {
        for (int i = 0; i < _spamButtons.Count; i++)
        {
            Spam_Button lSpamButton = _spamButtons[i];
            lSpamButton.timer -= Time.deltaTime;

            _spamButtons[i] = lSpamButton;
            if (_spamButtons[i].timer <= 0.0f) _spamButtons.RemoveAt(i);
        }
    }

    public bool IsButtonSpam(Enums.E_GAMEPAD_BUTTON pButton)
    {
        int countButtonInArray = 0;

        for (int i = 0; i < _spamButtons.Count; i++)
        {
            if (_spamButtons[i].buttonPressed == pButton) countButtonInArray++;
        }

        if (countButtonInArray >= _numberSpam) return true;

        return false;
    }


    #endregion

    #endregion

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        #region Sticks

        leftHorizontalAxis = Utils_Variables.REWIRED_PLAYER.GetAxis(Enums.E_GAMEPAD_BUTTON.LEFT_STICK_HORIZONTAL.ToString()); // get input by name or action id
        leftVerticalAxis = Utils_Variables.REWIRED_PLAYER.GetAxis(Enums.E_GAMEPAD_BUTTON.LEFT_STICK_VERTICAL.ToString());

        rightHorizontalAxis = Utils_Variables.REWIRED_PLAYER.GetAxis(Enums.E_GAMEPAD_BUTTON.RIGHT_STICK_HORIZONTAL.ToString()); // get input by name or action id
        rightVerticalAxis = Utils_Variables.REWIRED_PLAYER.GetAxis(Enums.E_GAMEPAD_BUTTON.RIGHT_STICK_VERTICAL.ToString());
        #endregion

        #region Update Special Inputs

        for (int i = 0; i < (int)Enums.E_GAMEPAD_BUTTON.NB_BUTTONS; i++)
        {
            Enums.E_GAMEPAD_BUTTON lTestedButton = (Enums.E_GAMEPAD_BUTTON)i;

            if (IsButtonPressed(lTestedButton))
            {
                Spam_Button spamButton;
                spamButton.buttonPressed = lTestedButton;
                spamButton.timer = _spamGap;

                _spamButtons.Add(spamButton);

                _buttonsHoldTime[i] = 0.0f;
            }

            if (IsButtonReleased(lTestedButton))
            {
                _buttonsHoldTime[i] = -1.0f;
            }

            if (_buttonsHoldTime[i] != -1.0f)
            {
                _buttonsHoldTime[i] += Time.deltaTime;
            }
        }

        UpdateSpamButtonsTimer();

        #endregion
    }
}
