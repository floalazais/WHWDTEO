using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager instance { get; private set; }
    [SerializeField] PlayableDirector playableDirector = null;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Already an instance of " + name);
            Destroy(instance);
        }

        instance = this;
    }

    public void PlayTimeline(PlayableAsset playableAsset)
    {
        UIManager.instance.RemoveScreen();
        playableDirector.playableAsset = playableAsset;
        playableDirector.Play();
    }
}