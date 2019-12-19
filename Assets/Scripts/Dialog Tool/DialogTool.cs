using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class DialogTool : NodeGraph {

    DialogNode currentNode = null;

    public Dictionary<string, bool> variablesDictionary = new Dictionary<string, bool>();

    public void StartDialog()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] as BeginNode)
            {
                UIManager.instance.OnStartDialog();
                currentNode = nodes[i] as DialogNode;
                return;
            }
        }
    }

    public bool UpdateNodes()
    {
        if (currentNode == null)
        {
            Debug.Log("dialogue fini");
            return true;
        }

        if (currentNode.Update())
        {
            currentNode = currentNode.GetNextNode();
            currentNode.Activate();
        }

        return false;
    }
}