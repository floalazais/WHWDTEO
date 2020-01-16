using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums 
{
    public enum E_GAMESTATE
    {
        EXPLORATION,
        MANIPULATION,
        NARRATION
    }

    public enum E_LEVEL_STATE
    {
        PRESENT,
        MEMORY_MODE,
        INTERACT,
        DESCRIPTION
    }

    public enum E_MOVE_DIRECTION
    {
        NONE,
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    public enum E_SWIPE_DIRECTION
    {
        NONE,
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    public enum E_ROLL_DIRECTION
    {
        NONE,
        LEFT,
        RIGHT
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
        PS_BUTTON,
        SHARE_BUTTON,
        OPTIONS_BUTTON,
        LEFT_STICK_BUTTON,
        RIGHT_STICK_BUTTON,
        D_PAD_RIGHT_BUTTON,
        D_PAD_LEFT_BUTTON,
        D_PAD_UP_BUTTON,
        D_PAD_DOWN_BUTTON,
        NB_BUTTONS
    }

    public enum E_INTERACT_TYPE
    {
        HOLD,
        RELEASE_HOLD,
        SPAM,
        CLICK,
        MOVE,
        SWIPE,
        ROLL
    }

    public enum E_RP_ACTION
    {
        ADD,
        REMOVE
    }

    public enum E_CHARACTER
    {
        AVA,
        MIA,
        CALEB
    }
}
