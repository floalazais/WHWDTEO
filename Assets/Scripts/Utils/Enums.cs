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
        L1,
        L2,
        R1,
        R2,
        CROSS,
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
