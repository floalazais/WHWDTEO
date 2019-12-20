using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance { get; private set; }

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

        dialogRunning = true;
        _dialogGraph.StartDialog(pDialogName);
    }

    // Update is called once per frame
    void Update()
    {
        if (!dialogRunning) return;

        if (_dialogGraph.UpdateNodes())
        {
            dialogRunning = false;

            UIManager.instance.OnEndDialog();
            GameManager.instance.SetModePlay();
        }
    }
}