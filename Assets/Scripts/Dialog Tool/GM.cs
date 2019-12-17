using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    public DialogTool _dialogGraph;
    bool dialogRunning = true;

    // Start is called before the first frame update
    void Start()
    {
        _dialogGraph.StartDialog();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dialogRunning) return;

        if (_dialogGraph.UpdateNodes())
        {
            dialogRunning = false;
        }
    }
}