using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using XNode;

public class IfNode : DialogNode {

    [Input(ShowBackingValue.Never)] public string previous;
    [Output(ShowBackingValue.Never)] public string verified;
    [Output(ShowBackingValue.Never)] public string notVerified;

    [SerializeField] string condition;

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
        Dictionary<string, bool> variablesDictionary = (graph as DialogTool).variablesDictionary;

        if (variablesDictionary.ContainsKey(condition))
        {
            if (variablesDictionary[condition]) return GetOutputPort("verified").Connection.node as DialogNode;
            else return GetOutputPort("notVerified").Connection.node as DialogNode;
        }

        else return GetOutputPort("notVerified").Connection.node as DialogNode;
    }
}