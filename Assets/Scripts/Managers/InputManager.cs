using System;
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

    #region Roll Input
    float _newAngle, _oldAngle;

    bool _isLSTurningRight = false;
    bool _isLSTurningLeft = false;

    bool _isRSTurningRight = false;
    bool _isRSTurningLeft = false;

    bool _isRollingRight = false;
    bool _isRollingLeft = false;

    #endregion

    #region Hold Input

    float[] _buttonsHoldTime = new float[(int)Enums.E_GAMEPAD_BUTTON.NB_BUTTONS];
    [SerializeField] float holdTime = 2.0f;

    #endregion

    #region Click Input

    float[] _buttonsClickValidityTime = new float[(int)Enums.E_GAMEPAD_BUTTON.NB_BUTTONS];
    [SerializeField] float clickMaximumHoldTime = 2.0f;
    [SerializeField] float clickValidityTime = 2.0f;

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

    #region Swipe Input

    [SerializeField] float _swipeValidityTime = 0.5f;

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
        InitializeButtonsClickValidityTime();
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

    #region Click Input Functions

    void InitializeButtonsClickValidityTime()
    {
        for (int i = 0; i < (int)Enums.E_GAMEPAD_BUTTON.NB_BUTTONS; i++)
        {
            _buttonsClickValidityTime[i] = -1.0f;
        }
    }

    public bool IsButtonClicked(Enums.E_GAMEPAD_BUTTON pButton)
    {
        return _buttonsClickValidityTime[(int)pButton] >= 0.0f;
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

    #region Roll Input Functions

    public bool IsStickRolling(Enums.E_ROLL_DIRECTION pRollDirection)
    {
        return pRollDirection == Enums.E_ROLL_DIRECTION.LEFT ? _isRollingRight : _isRollingLeft;
    }

    public bool IsStickTurning(Enums.E_ROLL_DIRECTION pRollDirection)
    {
        return pRollDirection == Enums.E_ROLL_DIRECTION.LEFT ? _isRSTurningRight : _isRSTurningLeft;
    }

    void UpdateRollAngle()
    {
        _oldAngle = _newAngle;
        _newAngle = Mathf.Atan2(rightHorizontalAxis, rightVerticalAxis) * Mathf.Rad2Deg;

        if (_newAngle < 0f) _newAngle += 360f;
        float lAngleDifference = _newAngle - _oldAngle;

        if (lAngleDifference > 0)
        {
            _isRSTurningRight = true;
            _isRSTurningLeft = false;
            _isRollingLeft = false;
        }

        if (lAngleDifference < 0)
        {
            _isRSTurningLeft = true;
            _isRSTurningRight = false;
            _isRollingRight = false;
        }

        //Complete turn
        if (_newAngle >= 250f && _isRSTurningRight) _isRollingRight = true;
        if (_newAngle <= 110f && _isRSTurningLeft) _isRollingLeft = true;

    }

    void ResetAngle()
    {
        _oldAngle = 0;
        _newAngle = 0;

        _isRSTurningLeft = false;
        _isRSTurningRight = false;

        _isRollingLeft = false;
        _isRollingRight = false;
    }

    #endregion

    #region Move Input Functions

    public bool IsStickMoving(Enums.E_MOVE_DIRECTION pMoveDirection)
    {
        if (pMoveDirection == Enums.E_MOVE_DIRECTION.LEFT)
        {
            return rightHorizontalAxis < -0.75f && Mathf.Abs(rightVerticalAxis) < 0.25f;
        }
        else if (pMoveDirection == Enums.E_MOVE_DIRECTION.RIGHT)
        {
            return rightHorizontalAxis > 0.75f && Mathf.Abs(rightVerticalAxis) < 0.25f;
        }
        else if (pMoveDirection == Enums.E_MOVE_DIRECTION.UP)
        {
            return rightVerticalAxis > 0.75f && Mathf.Abs(rightHorizontalAxis) < 0.25f;
        }
        else if (pMoveDirection == Enums.E_MOVE_DIRECTION.DOWN)
        {
            return rightVerticalAxis < -0.75f && Mathf.Abs(rightHorizontalAxis) < 0.25f;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region Swipe Input Functions

    Vector2 _startPosition;
    Vector2 _endPosition;

    float _swipeTimer = -1.0f;

    public bool IsSwiping(Enums.E_SWIPE_DIRECTION pSwipeDirection)
    {
        if (_swipeTimer <= 0.0f) return false;

        Vector2 _swipeMovement = _endPosition - _startPosition;

        if (pSwipeDirection == Enums.E_SWIPE_DIRECTION.LEFT)
        {
            return _swipeMovement.x < -0.5f && Mathf.Abs(_swipeMovement.y) < 0.25f;
        }
        else if (pSwipeDirection == Enums.E_SWIPE_DIRECTION.RIGHT)
        {
            return _swipeMovement.x > 0.5f && Mathf.Abs(_swipeMovement.y) < 0.25f;
        }
        else if (pSwipeDirection == Enums.E_SWIPE_DIRECTION.UP)
        {
            return _swipeMovement.y > 0.5f && Mathf.Abs(_swipeMovement.x) < 0.25f;
        }
        else if (pSwipeDirection == Enums.E_SWIPE_DIRECTION.DOWN)
        {
            return _swipeMovement.y < -0.5f && Mathf.Abs(_swipeMovement.x) < 0.25f;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #endregion

    // Update is called once per frame
    void Update()
    {
        UpdateInput();

        print(IsSwiping(Enums.E_SWIPE_DIRECTION.DOWN));
    }

    private void UpdateInput()
    {
        #region Update Sticks

        leftHorizontalAxis = Utils_Variables.REWIRED_PLAYER.GetAxis(Enums.E_GAMEPAD_BUTTON.LEFT_STICK_HORIZONTAL.ToString()); // get input by name or action id
        leftVerticalAxis = Utils_Variables.REWIRED_PLAYER.GetAxis(Enums.E_GAMEPAD_BUTTON.LEFT_STICK_VERTICAL.ToString());

        rightHorizontalAxis = Utils_Variables.REWIRED_PLAYER.GetAxis(Enums.E_GAMEPAD_BUTTON.RIGHT_STICK_HORIZONTAL.ToString()); // get input by name or action id
        rightVerticalAxis = Utils_Variables.REWIRED_PLAYER.GetAxis(Enums.E_GAMEPAD_BUTTON.RIGHT_STICK_VERTICAL.ToString());
        #endregion

        #region Update Special Joysticks Inputs

        if (rightHorizontalAxis != 0 || rightHorizontalAxis != 0) UpdateRollAngle();
        else ResetAngle();

        #endregion

        #region Update Special Button Inputs

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
                if (_buttonsHoldTime[i] <= clickMaximumHoldTime)
                {
                    _buttonsClickValidityTime[i] = clickValidityTime;
                }
                _buttonsHoldTime[i] = -1.0f;
            }

            if (_buttonsHoldTime[i] >= 0.0f)
            {
                _buttonsHoldTime[i] += Time.deltaTime;
            }

            if (_buttonsClickValidityTime[i] >= 0.0f)
            {
                _buttonsClickValidityTime[i] -= Time.deltaTime;
                if (_buttonsClickValidityTime[i] < 0.0f)
                {
                    _buttonsClickValidityTime[i] = -1.0f;
                }
            }
        }

        UpdateSpamButtonsTimer();

        #endregion

        #region Update Swipe

        if (PS4Controller.Instance.IsTouchpadPressed())
        {
            _startPosition = PS4Controller.Instance.GetFingerPositionOnTouchpad();
        } else if (PS4Controller.Instance.IsTouchpadReleased()) {
            _endPosition = PS4Controller.Instance.GetFingerPositionOnTouchpad();
            _swipeTimer = _swipeValidityTime;
        }

        if (_swipeTimer > 0.0f)
        {
            _swipeTimer -= Time.deltaTime;
            if (_swipeTimer <= 0.0f)
            {
                _swipeTimer = -1.0f;
            }
        }

        #endregion
    }
}
