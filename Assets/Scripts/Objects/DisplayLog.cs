using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayLog: MonoBehaviour
{
    public void DebugSignal(string s)
    {
        Debug.Log("Signal: " + s);
    }

    public void LaunchSound(AK.Wwise.Event e){
        SoundManager.instance.PlaySound(e.Id);
    }
}
