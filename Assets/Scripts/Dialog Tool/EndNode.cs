using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class EndNode : DialogNode {

    [Input(ShowBackingValue.Never)] public int End;

    // Use this for initialization
    protected override void Init()
    {
        base.Init();
    }

    public override void Activate()
    {
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