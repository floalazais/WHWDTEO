using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeTint("#1061e3")]
public class CameraNode : DialogNode
{
    [Input(ShowBackingValue.Never)] public string previous;
    [Output(ShowBackingValue.Never)] public string next;

    public string moveName; 

    protected override void Init()
    {
        base.Init();
    }

    public override void Activate()
    {

    }

    public override bool Update()
    {
        return false;
    }

    public override DialogNode GetNextNode()
    {
        return null;
    }
}
