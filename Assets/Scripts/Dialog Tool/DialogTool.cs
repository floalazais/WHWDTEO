using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class DialogTool : NodeGraph {

    DialogNode currentNode = null;

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
            UIManager.instance.OnEndDialog();
            return true;
        }

        if (currentNode.Update())
        {
            currentNode = currentNode.GetNextNode();
            if (currentNode as ChoiceNode)
            {
                UIManager.instance.OnChoiceScreen();
                ChoicePanel.instance.FillChoicesTextZone((currentNode as ChoiceNode).GetChoicesText());
            }

            else if(currentNode as CharacterTalkNode)
            {
                UIManager.instance.OnDialogScreen();
                DialogPanel.instance.FillTextZone((currentNode as CharacterTalkNode).GetDialogText());
            }

            else if (currentNode as EndNode)
            {
                UIManager.instance.OnDialogScreen();
                DialogPanel.instance.FillTextZone((currentNode as EndNode).GetEndText());
            }
        }

        return false;
    }
}