using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class W_SceneManager : MonoBehaviour
{
    public static W_SceneManager instance { get; private set; }

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
        SceneManager.LoadScene(pSceneName);
    }
}
