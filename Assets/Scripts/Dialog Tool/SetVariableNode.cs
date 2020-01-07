using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeTint("#f5bd25")]
public class SetVariableNode : DialogNode
{
    [Input(ShowBackingValue.Never)] public string previous;
    [Output(ShowBackingValue.Never)] public string next;

    public string condition;
    public bool value;

    protected override void Init()
    {
        base.Init();
    }

    public override void Activate()
    {
        Dictionary<string, bool> variablesDictionary = (graph as DialogTool).variablesDictionary;

        if (!variablesDictionary.ContainsKey(condition))
        {
            variablesDictionary.Add(condition, value);
        }

        else
        {
            variablesDictionary[condition] = value;
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
