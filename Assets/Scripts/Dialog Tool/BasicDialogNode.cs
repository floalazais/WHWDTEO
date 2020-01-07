using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDialogNode : DialogNode
{
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
