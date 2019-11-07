using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils_Variables
{
    public static string CROSS_BUTTON_ACTION = "Cross Button";
    public static string ROUND_BUTTON_ACTION = "Round Button";
    public static string TRIANGLE_BUTTON_ACTION = "Triangle Button";
    public static string SQUARE_BUTTON_ACTION = "Square Button";

    public static string R1_BUTTON_ACTION = "R1 Button";
    public static string R2_BUTTON_ACTION = "R2 Button";
    public static string L1_BUTTON_ACTION = "L1 Button";
    public static string L2_BUTTON_ACTION = "L2 Button";

    public static string MENU_BUTTON_ACTION = "PlayStation Button";
    public static string SHARE_BUTTON_ACTION = "Share Button";
    public static string OPTIONS_BUTTON_ACTION = "Options Button";

    public static string LEFT_STICK_BUTTON_ACTION = "Left Stick Button";
    public static string RIGHT_STICK_BUTTON_ACTION = "Right Stick Button";

    public static string LEFT_STICK_HORIZONTAL = "Left Stick Horizontal";
    public static string LEFT_STICK_VERTICAL = "Left Stick Vertical";

    public static string RIGHT_STICK_HORIZONTAL = "Right Stick Horizontal";
    public static string RIGHT_STICK_VERTICAL = "Right Stick Vertical";

    public static string D_PAD_RIGHT_BUTTON_ACTION = "D-Pad Right";
    public static string D_PAD_LEFT_BUTTON_ACTION = "D-Pad Left";
    public static string D_PAD_UP_BUTTON_ACTION = "D-Pad Up";
    public static string D_PAD_DOWN_BUTTON_ACTION = "D-Pad Down";

    public static int PLAYER_ID = 0;
    //TO-DO : Initialiser valeur dans un Init GM
    public static Player REWIRED_PLAYER { get { return ReInput.players.GetPlayer(PLAYER_ID); } }
}
