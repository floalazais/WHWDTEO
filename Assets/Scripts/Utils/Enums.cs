using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums 
{
    public enum E_GAMESTATE
    {
        PLAY,
        NOT_PLAY,
        MANIPULATION
    }

    public enum E_PAST_STATE
    {
        PRESENT,
        SEARCH_MODE,
        INTERACT,
        DESCRIPTION
    }

    public enum E_GAMEPAD_BUTTON
    {
        L1_BUTTON,
        L2_BUTTON,
        R1_BUTTON,
        R2_BUTTON,
        CROSS_BUTTON,
        ROUND_BUTTON,
        TRIANGLE_BUTTON,
        SQUARE_BUTTON,
        LEFT_STICK_HORIZONTAL,
        LEFT_STICK_VERTICAL,
        RIGHT_STICK_HORIZONTAL,
        RIGHT_STICK_VERTICAL,
        NB_BUTTONS

    }

    public enum E_INTERACT_TYPE
    {
        HOLD,
        SPAM,
        ROLL,
        PRESSED
    }
}
