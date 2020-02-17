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

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Controller.instance.transform.position - transform.position) * Quaternion.Euler(-23, 264, 47);
    }

    public void Interact()
    {
        _canvas.gameObject.SetActive(false);
        //_canvas.SetFarPlayerMode();
        //SoundManager.instance.PlaySound(soundEvent.Id);
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
