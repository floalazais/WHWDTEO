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
        if (Input.GetKey(KeyCode.Return)) return true;

        Debug.Log(EndText);
        return false;
    }

    public string GetEndText()
    {
        return EndText;
    }

    public override DialogNode GetNextNode()
    {
        return null;
    }
}