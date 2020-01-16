﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCinematicTrigger : ObjectInteractable
{
    public string dialogName;

    public override void Interact()
    {
        base.Interact();

        if (!_interactable)
        {
            GameManager.instance.SetGameStateExploration();
            return;
        }

        _interactable = false;

        DialogManager.instance.StartDialog(dialogName);
        GameManager.instance.SetGameStateNarration();
    }
}