using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using XNode;

[NodeTint("#aaffaa")]
public class LaunchTimelineNode : DialogNode {

    [Input(ShowBackingValue.Never)] public string previous;
    [Output(ShowBackingValue.Never)] public string next;

    [SerializeField] TimelineAsset timelineAsset;
    [SerializeField] bool loop;

    protected override void Init()
    {
        base.Init();
    }

    public override void Activate()
    {
        (graph as DialogTool).currentTimeline = timelineAsset;
        (graph as DialogTool).currentTimelineLoop = loop;
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