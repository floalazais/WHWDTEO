using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class W_SceneManager : MonoBehaviour
{
    public static W_SceneManager instance { get; private set; }
    public string sceneToLoad = Utils_Variables.PAST_SCENE_NAME;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("ALREADY INSTANCE CREATED " + name);
            Destroy(instance);
        }

        instance = this;
    }

    public void SwitchScene(string pSceneName)
    {
        sceneToLoad = pSceneName;
        SceneManager.LoadScene(Utils_Variables.LOAD_SCENE_NAME);
    }

    public void PlaySound(uint eventId)
    {
        AkSoundEngine.PostEvent(eventId, gameObject);
    }

    public void PlaySound(string eventName)
    {
        AkSoundEngine.PostEvent(eventName, gameObject);
    }
}
