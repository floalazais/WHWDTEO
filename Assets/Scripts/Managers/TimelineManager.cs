using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager instance { get; private set; }
    [SerializeField] PlayableDirector playableDirector = null;
    List<CanvasObject> canvasses = null;
    bool loop = false;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Already an instance of " + name);
            Destroy(instance);
        }

        instance = this;
    }

    void Start()
    {
        playableDirector.stopped += DisplayCanvasses;
        canvasses = GameObject.FindObjectsOfType<CanvasObject>().ToList();
    }

    public void PlayTimeline(PlayableAsset playableAsset)
    {
        foreach (CanvasObject canvas in canvasses)
        {
            canvas.gameObject.SetActive(false);
        }

        if (GameManager.instance.state == Enums.E_GAMESTATE.NARRATION) UIManager.instance.RemoveScreen();
        if (loop) playableDirector.extrapolationMode = DirectorWrapMode.Loop;
        playableDirector.playableAsset = playableAsset;
        playableDirector.Play();
    }

    void DisplayCanvasses(PlayableDirector pDirector)
    {
        foreach (CanvasObject canvas in canvasses)
        {
            canvas.gameObject.SetActive(true);
        }
    }

    public void SetLoopMode(bool value)
    {
        if (value)
        {
            playableDirector.extrapolationMode = DirectorWrapMode.Loop;
        } else {
            playableDirector.extrapolationMode = DirectorWrapMode.None;
        }
    }
}