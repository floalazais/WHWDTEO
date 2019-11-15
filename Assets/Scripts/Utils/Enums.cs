using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums 
{
    public enum E_GAMESTATE
    {
        PLAY,
        INSPECTION,
        DESCRIPTION
    }

    public enum E_PAST_STATE
    {
        PRESENT,
        SEARCH_MODE
    }

    public enum E_PAST_OBJECT_STATE
    {
        NOT_DISCOVERED,
        DISCOVERED,
        NEAR_PLAYER,
        INSPECTED
    }
}
