using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.LaunchEvent(Utils_Variables.START_ROOM_TONE_SOUND);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
