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

    //Animator Controller
    public static string IS_HOLDING = "isHolding";
    public static string IS_FADING = "isFading";
    public static string IS_ROLLING_RIGHT = "isRollingRight";
    //Sounds
    public static string MOVEMENT_PLAYER_SOUND = "Play_Presence_Ex";
    public static string MOVEMENT_IDLE_SOUND = "Play_Presence_Idle_Mia";
    public static string MOVEMENT_OBJECT_SOUND = "Play_Moving_Object";
    public static string STEP_VOID_SOUND = "Play_CARRELAGE";
    public static string STEP_PARQUET_SOUND = "Play_PARQUET";
    public static string STEP_TAPIS_SOUND = "Play_TAPIS";
    public static string INTERACTION_TEL_SOUND = "Play_Bouton_Tel";
    public static string POSE_BOITE_SOUND = "Play_Pose_Boite";
    public static string POSE_LAMPE_SOUND = "Play_Pose_Lampe";
    public static string POSE_LEGO_SOUND = "Play_Pose_Lego";
    public static string POSE_PELUCHE_SOUND = "Play_Pose_Peluche";
    public static string POSE_VOITURE_SOUND = "Play_Pose_Voiture";
    public static string PRISE_BOITE_SOUND = "Play_Prise_Boite";
    public static string PRISE_LAMPE_SOUND = "Play_Prise_Lampe";
    public static string PRISE_LEGO_SOUND = "Play_Prise_Lego";
    public static string PRISE_PELUCHE_SOUND = "Play_Prise_Peluche";
    public static string PRISE_VOITURE_SOUND = "Play_Prise_Voiture";
    public static string LETTRES_SOUND = "Play_Random_Lettres";
    public static string POSE_FEUILLE_SOUND = "Play_Rd_Pose_Feuille";
    public static string PRISE_FEUILLE_SOUND = "Play_Rd_Prise_Feuille";
    public static string APPARITION_MAIN_SOUND = "Play_APP_MAIN";
    public static string DISPARITION_MAIN_SOUND = "Play_DISP_MAIN";
    public static string BEGIN_MEMORY_SOUND = "Play_Begin_Memory";
    public static string END_MEMORY_SOUND = "Stop_Memory";
    public static string BREATH_IDLE_SOUND = "Play_Breath_Idle";
    public static string START_HOPE_GLITCH_SOUND = "Play_Glitch_Rd_Hope";
    public static string STOP_HOPE_GLITCH_SOUND = "Stop_Glitch_Rd_Hope";
    public static string START_ROOM_TONE_SOUND = "Play_RoomTone";
    public static string STOP_ROOM_TONE_SOUND = "Stop_RoomTone";
}
