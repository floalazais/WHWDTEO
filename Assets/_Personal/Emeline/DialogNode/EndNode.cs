using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class EndNode : DialogNode {

    [Input(ShowBackingValue.Never)] public int End;
    [SerializeField] string EndText;

    // Use this for initialization
    protected override void Init()
    {
        base.Init();
    }

    public override bool Update()
    {
        Debug.Log(EndText);
        return true;
    }

    public override DialogNode GetNextNode()
    {
        return null;
    }
}