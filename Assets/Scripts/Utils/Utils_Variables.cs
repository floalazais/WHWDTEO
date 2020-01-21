using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils_Variables
{
    public static int PLAYER_ID = 0;

    //public static Dictionary<Enums.E_GAMEPAD_BUTTON, string> GAMEPAD_BUTTONS= new Dictionary<Enums.E_GAMEPAD_BUTTON, string>
    //TO-DO : Initialiser valeur dans un Init GM
    public static Player REWIRED_PLAYER { get { return ReInput.players.GetPlayer(PLAYER_ID); } }

    public static int LAYER_CAMERA_COLLISION = 9;
    public static int LAYER_OBJECT_INTERACT = 10;

    public static string PRESENT_SCENE_NAME = "MainScene";
    public static string LOAD_SCENE_NAME = "LoadScene";
    public static string PAST_SCENE_NAME = "PastScene";

    public static string OBJECT_CANVAS_TEXT = "Text";
    public static string OBJECT_CANVAS_INTERACTION = "artInteractionButton";
    public static string OBJECT_CANVAS_ARROW = "artInteractionIndicator";
}
