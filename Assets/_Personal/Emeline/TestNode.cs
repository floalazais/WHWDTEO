using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class TestNode : Node
{
    [Input] public float value;
    [Output] public float result;

    protected override void Init()
    {
        base.Init();
    }

    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "result")
        {
            return GetInputValue<float>("value") + 1;
        }

        else return null;
    }
}
