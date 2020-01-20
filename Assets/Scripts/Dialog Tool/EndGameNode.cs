using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameNode : DialogNode
{
    [Input(ShowBackingValue.Never)] public string previous;

    protected override void Init()
    {
        base.Init();
    }

    public override void Activate()
    {
        Application.Quit();
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
