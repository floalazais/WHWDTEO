using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Already an instance of " + name);
            Destroy(instance);
        }

        instance = this;
    }

    public void PlaySound(string pSoundName)
    {
        print(pSoundName);
    }
}
