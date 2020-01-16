using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using XNode;

[NodeTint("#e056fd")]
[NodeWidth(300)]
public class SetInteractableObjectActiveStateNode : DialogNode {

    [Input(ShowBackingValue.Never)] public string previous;
    [Output(ShowBackingValue.Never)] public string next;

    public string _objectName;
    public bool _activeState;

    protected override void Init()
    {
        base.Init();
    }

    public override void Activate()
    {
        List<ObjectInteractable> interactables = GameObject.FindObjectsOfType<ObjectInteractable>().ToList();

        foreach (ObjectInteractable interactable in interactables)
        {
            if (interactable.name == _objectName)
            {
                interactable.SetInteractable(_activeState);
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
        return GetOutputPort("next").Connection.node as DialogNode;
    }
}