using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance { get; private set; }

    [SerializeField] Cinemachine.CinemachineVirtualCamera gameplayCamera;
    [SerializeField] GameObject Mia;
    [SerializeField] GameObject Mia2;
    [SerializeField] GameObject GhostMia;
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
            if (Mia != null) Mia.SetActive(false);
            if (GhostMia != null) GhostMia.SetActive(false);
            if(Mia2 != null) Mia2.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!dialogRunning) return;

        if (_dialogGraph.currentSound != "")
        {
            SoundManager.instance.PlaySound(_dialogGraph.currentSound);
            _dialogGraph.currentSound = "";
        }

        if (_dialogGraph.currentTimeline != null)
        {
            TimelineManager.instance.SetLoopMode(_dialogGraph.currentTimelineLoop);
            TimelineManager.instance.PlayTimeline(_dialogGraph.currentTimeline);
            gameplayCamera.Priority = 0;
            _dialogGraph.currentTimeline = null;
            _dialogGraph.currentTimelineLoop = false;
        }

        if ((InputManager.instance.IsButtonReleased(Enums.E_GAMEPAD_BUTTON.ROUND_BUTTON) || Input.GetKeyUp(KeyCode.Space)) && (_dialogGraph.currentNode as PlayTimelineNode) != null)
        {
            (_dialogGraph.currentNode as PlayTimelineNode).timer = 0.0f;
            TimelineManager.instance.StopTimeline();
        }

        if (_dialogGraph.UpdateNodes())
        {
            dialogRunning = false;

            if (GameManager.instance.state == Enums.E_GAMESTATE.NARRATION)
            {
                UIManager.instance.OnEndDialog();
                GameManager.instance.SetGameStateExploration();
                if(PastManager.instance != null) PastManager.instance.Refresh();
                gameplayCamera.Priority = 15;
                if (Mia != null) Mia.SetActive(true);
                if (GhostMia != null) GhostMia.SetActive(true);
                if(Mia2 != null) Mia2.SetActive(true);
            }
        }
    }

    public void SetVariableValue(string variableName, bool value)
    {
        if (!_dialogGraph.variablesDictionary.ContainsKey(variableName))
        {
            _dialogGraph.variablesDictionary.Add(variableName, value);
        }

        else
        {
            _dialogGraph.variablesDictionary[variableName] = value;
        }
    }

    public void KillDialog()
    {
        if (_dialogGraph.killable)
        {
            _dialogGraph.killable = false;
            dialogRunning = false;

            _dialogGraph.FastForward();

            if (GameManager.instance.state == Enums.E_GAMESTATE.NARRATION)
            {
                UIManager.instance.OnEndDialog();
                GameManager.instance.SetGameStateExploration();
                if (PastManager.instance != null) PastManager.instance.Refresh();
                gameplayCamera.Priority = 15;
                if (Mia != null) Mia.SetActive(true);
                if (GhostMia != null) GhostMia.SetActive(true);
                if (Mia2 != null) Mia2.SetActive(true);
            }
        }
    }
}