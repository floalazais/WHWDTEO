using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Vector3 _moveVector = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        #region Right Buttons

        bool crossButton = Utils_Variables.REWIRED_PLAYER.GetButtonDown(Utils_Variables.CROSS_BUTTON_ACTION);
        if (crossButton) EventsManager.Instance.Raise(new OnCrossButton());
        print(crossButton);

        bool roundButton = Utils_Variables.REWIRED_PLAYER.GetButtonDown(Utils_Variables.ROUND_BUTTON_ACTION);
        if (roundButton) EventsManager.Instance.Raise(new OnRoundButton());

        bool triangleButton = Utils_Variables.REWIRED_PLAYER.GetButtonDown(Utils_Variables.TRIANGLE_BUTTON_ACTION);
        if (triangleButton) EventsManager.Instance.Raise(new OnTriangleButton());

        bool squareButton = Utils_Variables.REWIRED_PLAYER.GetButtonDown(Utils_Variables.SQUARE_BUTTON_ACTION);
        if (squareButton) EventsManager.Instance.Raise(new OnSquareButton());
        #endregion

        #region Behind Buttons

        bool R1Button = Utils_Variables.REWIRED_PLAYER.GetButtonDown(Utils_Variables.R1_BUTTON_ACTION);
        if (R1Button) EventsManager.Instance.Raise(new ONR1Button());

        bool R2Button = Utils_Variables.REWIRED_PLAYER.GetButtonDown(Utils_Variables.R2_BUTTON_ACTION);
        if (R2Button) EventsManager.Instance.Raise(new ONR2Button());

        bool L1Button = Utils_Variables.REWIRED_PLAYER.GetButtonDown(Utils_Variables.L1_BUTTON_ACTION);
        if (L1Button) EventsManager.Instance.Raise(new ONL1Button());

        bool L2Button = Utils_Variables.REWIRED_PLAYER.GetButtonDown(Utils_Variables.L2_BUTTON_ACTION);
        if (L2Button) EventsManager.Instance.Raise(new ONL2Button());
        #endregion

        #region Menu buttons

        bool playstationButton = Utils_Variables.REWIRED_PLAYER.GetButtonDown(Utils_Variables.MENU_BUTTON_ACTION);
        if (playstationButton) EventsManager.Instance.Raise(new OnMenuButton());

        bool shareButton = Utils_Variables.REWIRED_PLAYER.GetButtonDown(Utils_Variables.SHARE_BUTTON_ACTION);
        if (shareButton) EventsManager.Instance.Raise(new OnShareButton());

        bool optionsButton = Utils_Variables.REWIRED_PLAYER.GetButtonDown(Utils_Variables.OPTIONS_BUTTON_ACTION);
        if (optionsButton) EventsManager.Instance.Raise(new OnOptionsButton());
        #endregion

        #region Sticks

        float horizontalLeftStick = Utils_Variables.REWIRED_PLAYER.GetAxis(Utils_Variables.LEFT_STICK_HORIZONTAL); // get input by name or action id
        float verticalLeftStick = Utils_Variables.REWIRED_PLAYER.GetAxis(Utils_Variables.LEFT_STICK_VERTICAL);

        if(horizontalLeftStick != 0.0f || verticalLeftStick != 0.0f)
        {
            Vector3 lPosition = new Vector3(horizontalLeftStick, 0, verticalLeftStick);
            EventsManager.Instance.Raise(new OnLeftStickMove(lPosition));
        }

        float horizontalRightStick = Utils_Variables.REWIRED_PLAYER.GetAxis(Utils_Variables.RIGHT_STICK_HORIZONTAL); // get input by name or action id
        float verticalRightStick = Utils_Variables.REWIRED_PLAYER.GetAxis(Utils_Variables.RIGHT_STICK_VERTICAL);

        if (horizontalRightStick != 0.0f || verticalRightStick != 0.0f)
        {
            Vector3 lPosition = new Vector3(horizontalRightStick, 0, verticalRightStick);
            EventsManager.Instance.Raise(new OnRightStickMove(lPosition));
        }

        bool leftStickButton = Utils_Variables.REWIRED_PLAYER.GetButtonDown(Utils_Variables.LEFT_STICK_BUTTON_ACTION);
        if (leftStickButton) EventsManager.Instance.Raise(new OnLeftStickButton());

        bool rightStickButton = Utils_Variables.REWIRED_PLAYER.GetButtonDown(Utils_Variables.RIGHT_STICK_BUTTON_ACTION);
        if (rightStickButton) EventsManager.Instance.Raise(new OnRightStickButton());
        #endregion

        #region Left Buttons

        bool dPadRightButton = Utils_Variables.REWIRED_PLAYER.GetButtonDown(Utils_Variables.D_PAD_RIGHT_BUTTON_ACTION);
        if (dPadRightButton) EventsManager.Instance.Raise(new OnDPadRightButton());

        bool dPadLeftButton = Utils_Variables.REWIRED_PLAYER.GetButtonDown(Utils_Variables.D_PAD_LEFT_BUTTON_ACTION);
        if (dPadLeftButton) EventsManager.Instance.Raise(new OnDPadLeftButton());

        bool dPadUpButton = Utils_Variables.REWIRED_PLAYER.GetButtonDown(Utils_Variables.D_PAD_UP_BUTTON_ACTION);
        if (dPadUpButton) EventsManager.Instance.Raise(new OnDPadUpButton());

        bool dPadDownButton = Utils_Variables.REWIRED_PLAYER.GetButtonDown(Utils_Variables.D_PAD_DOWN_BUTTON_ACTION);
        if (dPadDownButton) EventsManager.Instance.Raise(new OnDPadBottomButton());
        #endregion
    }
}
