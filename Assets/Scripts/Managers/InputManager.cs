using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance { get { return _instance; } }
    static InputManager _instance;

    private Vector3 _moveVector = Vector3.zero;

    #region Sticks
    public float rightHorizontalAxis { get; private set; }
    public float rightVerticalAxis { get; private set; }
    public float leftHorizontalAxis { get; private set; }
    public float leftVerticalAxis { get; private set; }
    #endregion

    bool[] _buttonsStatesLastFrame = new bool[(int)Enums.E_GAMEPAD_BUTTON.NB_BUTTONS];
    bool[] _buttonsStatesCurrentFrame = new bool[(int)Enums.E_GAMEPAD_BUTTON.NB_BUTTONS];

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("ALREADY INSTANCE CREATED " + name);
            Destroy(_instance);
        }

        _instance = this;
    }

    public bool IsButtonPressed(string pButton)
    {
        return Utils_Variables.REWIRED_PLAYER.GetButtonDown(pButton);
    }

    public bool IsButtonReleased(string pButton)
    {
        return Utils_Variables.REWIRED_PLAYER.GetButtonUp(pButton);
    }

    public bool IsButtonDown(string pButton)
    {
        return Utils_Variables.REWIRED_PLAYER.GetButton(pButton);
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        #region Sticks

        leftHorizontalAxis = Utils_Variables.REWIRED_PLAYER.GetAxis(Utils_Variables.LEFT_STICK_HORIZONTAL); // get input by name or action id
        leftVerticalAxis = Utils_Variables.REWIRED_PLAYER.GetAxis(Utils_Variables.LEFT_STICK_VERTICAL);

        rightHorizontalAxis = Utils_Variables.REWIRED_PLAYER.GetAxis(Utils_Variables.RIGHT_STICK_HORIZONTAL); // get input by name or action id
        rightVerticalAxis = Utils_Variables.REWIRED_PLAYER.GetAxis(Utils_Variables.RIGHT_STICK_VERTICAL);
        #endregion
    }
}
