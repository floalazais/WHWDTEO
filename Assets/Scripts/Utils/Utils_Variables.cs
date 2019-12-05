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
}
