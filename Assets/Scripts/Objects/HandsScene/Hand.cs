using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    [SerializeField] protected CanvasObject _canvas = null;
    [SerializeField] AK.Wwise.Event soundEvent;

    void Start()
    {
        SetFarPlayerMode();
    }

    public void Interact()
    {
        _canvas.SetFarPlayerMode();
        SoundManager.instance.LaunchEvent(soundEvent.Id);
    }

    public void SetClosePlayerMode()
    {
        _canvas.SetClosePlayerMode();
    }

    public void SetMediumPlayerMode()
    {
        _canvas.SetMediumPlayerMode();
    }

    public void SetNearPlayerMode()
    {
        _canvas.SetClosestPlayerMode();
    }

    public void SetFarPlayerMode()
    {
        _canvas.SetFarPlayerMode();
    }
}
