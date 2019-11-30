using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils_Variables
{
    public static string CROSS_BUTTON_ACTION = "CROSS_BUTTON";
    public static string ROUND_BUTTON_ACTION = "ROUND_BUTTON";
    public static string TRIANGLE_BUTTON_ACTION = "TRIANGLE_BUTTON";
    public static string SQUARE_BUTTON_ACTION = "SQUARE_BUTTON";

    public static string R1_BUTTON_ACTION = "R1_BUTTON";
    public static string R2_BUTTON_ACTION = "R2_BUTTON";
    public static string L1_BUTTON_ACTION = "L1_BUTTON";
    public static string L2_BUTTON_ACTION = "L2_BUTTON";

    public static string MENU_BUTTON_ACTION = "PS_BUTTON";
    public static string SHARE_BUTTON_ACTION = "SHARE_BUTTON";
    public static string OPTIONS_BUTTON_ACTION = "OPTIONS_BUTTON";

    public static string LEFT_STICK_BUTTON_ACTION = "LEFT_STICK_BUTTON";
    public static string RIGHT_STICK_BUTTON_ACTION = "RIGHT_STICK_BUTTON";

    public static string LEFT_STICK_HORIZONTAL = "LEFT_STICK_HORIZONTAL";
    public static string LEFT_STICK_VERTICAL = "LEFT_STICK_VERTICAL";

    public static string RIGHT_STICK_HORIZONTAL = "RIGHT_STICK_HORIZONTAL";
    public static string RIGHT_STICK_VERTICAL = "RIGHT_STICK_VERTICAL";

    public static string D_PAD_RIGHT_BUTTON_ACTION = "D_PAD_RIGHT_BUTTON";
    public static string D_PAD_LEFT_BUTTON_ACTION = "D_PAD_LEFT_BUTTON";
    public static string D_PAD_UP_BUTTON_ACTION = "D_PAD_UP_BUTTON";
    public static string D_PAD_DOWN_BUTTON_ACTION = "D_PAD_DOWN_BUTTON";

    public static int PLAYER_ID = 0;

    //public static Dictionary<Enums.E_GAMEPAD_BUTTON, string> GAMEPAD_BUTTONS= new Dictionary<Enums.E_GAMEPAD_BUTTON, string>
    //TO-DO : Initialiser valeur dans un Init GM
    public static Player REWIRED_PLAYER { get { return ReInput.players.GetPlayer(PLAYER_ID); } }
}
