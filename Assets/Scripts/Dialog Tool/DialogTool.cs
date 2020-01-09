using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using XNode;

[CreateAssetMenu]
public class DialogTool : NodeGraph {

    DialogNode currentNode = null;

    public Dictionary<string, bool> variablesDictionary = new Dictionary<string, bool>();
    public TimelineAsset timelineLaunched = null;

    public void StartDialog(string pDialogName)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] as BeginNode)
            {
                if ((nodes[i] as BeginNode).dialogName != pDialogName) continue;

                UIManager.instance.OnStartDialog();
                currentNode = nodes[i] as DialogNode;
                return;
            }
        }
    }

    public bool UpdateNodes()
    {
        if (currentNode.Update())
        {
            Debug.Log("lala");
            currentNode = currentNode.GetNextNode();
            if (currentNode != null)
            {
                currentNode.Activate();
            } else {
                Debug.Log("dialogue fini");
                return true;
            }
        }

        return false;
    }
}