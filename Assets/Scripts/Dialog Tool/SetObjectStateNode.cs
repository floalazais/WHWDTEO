using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using XNode;

[NodeTint("#e056fd")]
[NodeWidth(300)]
public class SetObjectStateNode : DialogNode {

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
        List<ObjectInteractable> objects = GameObject.FindObjectsOfType<ObjectInteractable>().ToList();

        foreach (ObjectInteractable obj in objects)
        {
            if (obj.name == _objectName)
            {
                obj.SetInteractable(_activeState);
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