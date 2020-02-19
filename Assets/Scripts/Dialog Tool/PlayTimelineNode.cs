using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using XNode;

[NodeTint("#aaffaa")]
public class PlayTimelineNode : DialogNode {

    [Input(ShowBackingValue.Never)] public string previous;
    [Output(ShowBackingValue.Never)] public string next;

    public TimelineAsset timelineAsset;
    [HideInInspector] public float timer = 0.0f;

    protected override void Init()
    {
        base.Init();
    }

    public override void Activate()
    {
        (graph as DialogTool).currentTimeline = timelineAsset;
        timer = (float)timelineAsset.duration;
    }

    public override bool Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override DialogNode GetNextNode()
    {
        return GetOutputPort("next").Connection.node as DialogNode;
    }
}