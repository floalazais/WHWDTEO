using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeTint("#bf37de")]
[NodeWidth(250)]
public class RPNode : DialogNode
{
    [Input(ShowBackingValue.Never)] public string previous;
    [Output(ShowBackingValue.Never)] public string next;

    public Enums.E_RP_ACTION RP = Enums.E_RP_ACTION.ADD;
    public int POINTS = 0;
    public Enums.E_CHARACTER char1 = Enums.E_CHARACTER.AVA;
    public Enums.E_CHARACTER char2 = Enums.E_CHARACTER.MIA;

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
