using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using XNode;

[NodeTint("#e056fd")]
[NodeWidth(300)]
public class ForceInteractionNode : DialogNode {

    [Input(ShowBackingValue.Never)] public string previous;

    public string _objectName;

    protected override void Init()
    {
        base.Init();
    }

    public override void Activate()
    {
        List<ObjectInteractable> objects = GameObject.FindObjectsOfType<ObjectInteractable>().ToList();

        foreach (ObjectInteractable obj in objects)
        {
            if (obj.name == _objectName)
            {
                ObjectInteractable objectInteractable = obj.GetComponent<ObjectInteractable>();
                GameManager.instance.SetGameStateManipulation();
                objectInteractable.SetInteractable(true);
                objectInteractable.Interact();
                return;
            }
        }
    }

    public override bool Update()
    {
        return true;
    }

    public override DialogNode GetNextNode()
    {
        return null;
    }
}