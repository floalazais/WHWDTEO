using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class DialogTool : NodeGraph {

    DialogNode currentNode = null;

    public Dictionary<string, bool> variablesDictionary = new Dictionary<string, bool>();

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
        if (currentNode == null)
        {
            Debug.Log("dialogue fini");
            return true;
        }

        if (currentNode.Update())
        {
            Debug.Log("lala");
            currentNode = currentNode.GetNextNode();
            currentNode.Activate();
        }

        return false;
    }
}