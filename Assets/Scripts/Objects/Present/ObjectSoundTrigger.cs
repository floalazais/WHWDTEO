using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSoundTrigger : ObjectInteractable
{
    public AK.Wwise.Event soundEvent;

    private void Start()
    {
        //_meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();

        Init();

        SetModePresent();
    }

    public override void Interact()
    {
        base.Interact();

        interactable = false;

        SoundManager.instance.PlaySound(soundEvent.Id);

        GameManager.instance.SetGameStateExploration();
    }
}