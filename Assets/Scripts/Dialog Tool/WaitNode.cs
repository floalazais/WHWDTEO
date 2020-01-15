using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using XNode;

public class WaitNode : DialogNode {

    [Input(ShowBackingValue.Never)] public string previous;
    [Output(ShowBackingValue.Never)] public string next;

    public float waitTime = 0.0f;
    float timer = 0.0f;

    protected override void Init()
    {
        base.Init();
    }

    public override void Activate()
    {
        timer = 0.0f;
    }

    public override bool Update()
    {
        timer += Time.deltaTime;
        if (timer >= waitTime)
        {
            return true;
        } else {
            return false;
        }
    }

    public override DialogNode GetNextNode()
    {
        return GetOutputPort("next").Connection.node as DialogNode;
    }
}