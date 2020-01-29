using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using XNode;

[CreateAssetMenu]
public class DialogTool : NodeGraph {

    public DialogNode currentNode = null;

    public Dictionary<string, bool> variablesDictionary = new Dictionary<string, bool>();
    public TimelineAsset currentTimeline = null;
    public bool staticTimeline = false;

    public bool StartDialog(string pDialogName)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] as BeginNode)
            {
                if ((nodes[i] as BeginNode).dialogName != pDialogName) continue;

                currentNode = nodes[i] as DialogNode;
                return true;
            }
        }
        return false;
    }

    public bool UpdateNodes()
    {
        if (currentNode.Update())
        {
            currentNode = currentNode.GetNextNode();
            if (currentNode != null)
            {
                currentNode.Activate();
            } else {
                return true;
            }
        }

        return false;
    }
}