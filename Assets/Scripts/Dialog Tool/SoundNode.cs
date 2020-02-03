using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using XNode;

[NodeTint("#ff5555")]
public class SoundNode : DialogNode {

    [Input(ShowBackingValue.Never)] public string previous;
    [Output(ShowBackingValue.Never)] public string next;

    public AK.Wwise.Event sound;

    protected override void Init()
    {
        base.Init();
    }

    public override void Activate()
    {
        SoundManager.instance.LaunchEvent(sound.Id);
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