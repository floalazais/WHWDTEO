using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using XNode;

[NodeWidth(400)]
[NodeTint("#8888ff")]
public class SubtitleNode : DialogNode {

    [Input(ShowBackingValue.Never)] public string previous;
    [Output(ShowBackingValue.Never)] public string next;

    [SerializeField] [TextArea(3, 5)] string subtitle = "";

    public float displayTime = 0.0f;
    float timer = 0.0f;

    protected override void Init()
    {
        base.Init();
    }

    public override void Activate()
    {
        timer = 0.0f;
        UIManager.instance.OnDialogScreen();
        DialogPanel.instance.FillTextZone(subtitle);
    }

    public override bool Update()
    {
        timer += Time.deltaTime;
        if (timer >= displayTime)
        {
            if (GetOutputPort("next").Connection.node as ChoiceNode == null) UIManager.instance.OnEndDialog();
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