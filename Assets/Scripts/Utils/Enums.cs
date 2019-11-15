using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums 
{
    public enum E_GAMESTATE
    {
        PLAY,
        NOT_PLAY
    }

    public enum E_PAST_STATE
    {
        PRESENT,
        SEARCH_MODE,
        INTERACT,
        DESCRIPTION
    }
}
