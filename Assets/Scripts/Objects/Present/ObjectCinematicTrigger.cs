using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCinematicTrigger : ObjectInteractable
{
    public string dialogName;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();

        Init();

        SetModePresent();
    }

    public override void Interact()
    {
        base.Interact();

        interactable = false;

        DialogManager.instance.StartDialog(dialogName);
        GameManager.instance.SetGameStateNarration();
    }
}