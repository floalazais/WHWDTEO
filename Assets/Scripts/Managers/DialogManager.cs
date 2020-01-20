﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance { get; private set; }

    [SerializeField] Cinemachine.CinemachineVirtualCamera gameplayCamera;
    [SerializeField] GameObject Mia;
    public DialogTool _dialogGraph;
    bool dialogRunning = false;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("ALREADY INSTANCE CREATED " + name);
            Destroy(instance);
        }

        instance = this;
    }

    public void StartDialog(string pDialogName)
    {
        if (dialogRunning) return;

        dialogRunning = _dialogGraph.StartDialog(pDialogName);

        if (dialogRunning)
        {
            GameManager.instance.SetGameStateNarration();
            Mia.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!dialogRunning) return;

        if (_dialogGraph.currentTimeline != null)
        {
            TimelineManager.instance.PlayTimeline(_dialogGraph.currentTimeline);
            gameplayCamera.Priority = 0;
            _dialogGraph.currentTimeline = null;
            _dialogGraph.staticTimeline = false;
        }

        if (_dialogGraph.UpdateNodes())
        {
            dialogRunning = false;

            UIManager.instance.OnEndDialog();
            GameManager.instance.SetGameStateExploration();
            gameplayCamera.Priority = 10;
            if(PastManager.instance != null) PastManager.instance.Refresh();
            Mia.SetActive(true);
        }
    }
}